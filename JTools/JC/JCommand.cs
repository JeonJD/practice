using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net; // IPAddress
using System.Net.Sockets; //TcpListener 클래스사용
using System.Threading; //스레드 클래스 사용
using System.IO; //파일 클래스 사용
using System.Xml; //XML 클래스 사용
using System.Collections.Specialized; //XML 문서 생성
using System.Security.Principal; //윈도우즈 권한체크 및 상승


namespace JC
{
    public partial class frmJCommand : Form
    {

        private struct structIdNameList
        {
            public string id;
            public string name_dev;
            public string name;
        }

        private struct structPreset
        {
            public string nameBt;
            public string cmdName;
            public string toggle;
            public string argument;
            public string target;
            public string confirmation;
            public string commandLine_1;
            public string commandLine_2;
        }


        event KeyboardHooker.HookedKeyboardUserEventHandler HookedKeyboardNofity; //후킹이벤트


        private const int tab_count = 10; //커맨드 버튼셋 페이지 수 
        private const int bt_count = 30; //커맨드 버튼 개수 
        private const string addr = "127.0.0.1"; //루프백
        private int Port_slot1 = 0; //포트
        
        //여기서부터 클래스 전역 변수 남발!!
        structIdNameList[] listItem, listVehicle, listNpc; //콤보박스의 자동완성에 사용할 리스트
        private TcpClient tcClient; //TCP 네트워크 서비스에 대한 클라이언트 연결 제공
        private NetworkStream jcStream; //네트워크 스트림
        private Thread slot1_Reader; //TCP 연결 스레드
        private bool ClientCon = false; //클라이언트 접속 여부 플래그
        private bool setMinimum = false; //접기 펼치기 플래그
        
        private delegate void AddTextDelegate(string strText);
        private AddTextDelegate AddText = null; //메시지 로그의 출력에 사용할 델리게이트
        private AddTextDelegate AddPort = null;
        private AddTextDelegate AddConn = null;
        private AddTextDelegate AddTargetName = null;

        //doitdoit 관련
        frmDoItDoIt doitdoit = new frmDoItDoIt();

        private XmlDocument xmlSavePreset = new XmlDocument(); //프리셋 저장 정보를 담을 XML
        //presetData presetDataList = new presetData();

        //아무곳이나 클릭하여 이동을 위해 DLL Import
        [DllImport("user32.dll")] 
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public readonly int WM_NLBUTTONDOWN = 0xA1;
        public readonly int HT_CAPTION = 0x2;
        public readonly int HT_BOTTOM = 0x0F;
        //

        //디자이너로 생성한 버튼을 배열처럼 쓰기 위해 배열로 참조
        Button[,] btRef = new Button[30, 10];

        public frmJCommand()
        {
            InitializeComponent();
        }

        private void JCommand_Load(object sender, EventArgs e)
        {
            
            //후크 이벤트를 연결
            HookedKeyboardNofity += new KeyboardHooker.HookedKeyboardUserEventHandler(JC_HookedKeyboardNofity);
            //자동으로 훅을 시작, 훅에 대한 이벤트 연결
            KeyboardHooker.Hook(HookedKeyboardNofity);
            
            //메인 쓰레드 생성 컨트롤 접근을 위한 델리게이트 객체
            AddText = new AddTextDelegate(messageView); 
            AddPort = new AddTextDelegate(portView);
            AddConn = new AddTextDelegate(buttonView);
            AddTargetName = new AddTextDelegate(targetView);

            checkUserCfg(); //User.cfg파일을 확인하고 없으면 생성

            loadPreset(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JC\preset.xml"); //설정파일 자동 로드
            referenceButton(); //디자이너로 생성한 버튼을 배열처럼 사용하기 위헤 버튼 배열로 참조
            runSetButtonName(); //설정파일에서 명령이름을 불러와 각 버튼 텍스트로 설정
            loadAllList(); //자동완성에 필요한 모든 리스트를 파일에서 로드
            btConn_Click(sender, e); //연결 버튼 자동실행
            this.cbCommand.Focus(); //명령어 입력창에 포커스

            
        }

        private void referenceButton()
        {
            //디자이너로 생성한 버튼을 배열로 참조하는 일을 할껀데 일단 나중에..

            

        }

        protected override void OnMouseDown(MouseEventArgs e) //메인폼의 아무곳이나 클릭하여 이동
        {

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();// 다른 컨트롤에 묶여있을 수 있을 수 있으므로 마우스캡쳐 해제
                SendMessage(this.Handle, WM_NLBUTTONDOWN, HT_CAPTION, 0);// 타이틀 바의 다운 이벤트처럼 보냄
            }

//            base.OnMouseDown(e);            

        }



        private void checkUserCfg() // user.cfg 파일을 검사하고 'dev_ex_commander_listen = 127.0.0.1:9111' 내용이 없으면 추가함
        {
            string pathMyDouments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string valueUsreCfg = "dev_ex_commander_listen = ";
            string valuePort = "127.0.0.1:9111";
            if (File.Exists(pathMyDouments + @"\CivOnline\user.cfg"))
            {
                string textUserCfg = File.ReadAllText(pathMyDouments + @"\CivOnline\user.cfg");

                if (!(textUserCfg.Contains(valueUsreCfg)))
                {
                    textUserCfg += Environment.NewLine + valueUsreCfg + valuePort + Environment.NewLine;
                    File.WriteAllText(pathMyDouments + @"\CivOnline\user.cfg", textUserCfg);
                    MessageBox.Show(pathMyDouments + @"\CivOnline\user.cfg에" + Environment.NewLine + valueUsreCfg + valuePort + "를 추가하였습니다.");
                }
            }
            else
            {
                DirectoryInfo pathCivOnline = new DirectoryInfo(pathMyDouments + @"\CivOnline");

                if (!(pathCivOnline.Exists))
                    pathCivOnline.Create();
                File.WriteAllText(pathMyDouments + @"\CivOnline\user.cfg", valueUsreCfg + valuePort);
                MessageBox.Show(pathMyDouments + @"\CivOnline\user.cfg를 생성하였습니다.");
            }
        }

        private void loadAllList() //파일에서 리스트를 로드해 자동완성 후보로 등록
        {
            loadListCommand(Application.StartupPath + @"\autoCompleteResource\consolecommandsandvars.txt"); //콘솔 명령어 리스트를 파싱해서 자동완성 후보로 등록
            listItem = loadListFromFile(Application.StartupPath + @"\autoCompleteResource\item.list", ref cbGetItem); //아이템 리스트 등록
            listVehicle = loadListFromFile(Application.StartupPath + @"\autoCompleteResource\vehicle.list", ref cbGetVehicle); //탈것 리스트 등록
            listNpc = loadListFromFile(Application.StartupPath + @"\autoCompleteResource\npc.list", ref cbGetNpc); //NPC 리스트 등록
        }



        private void loadListCommand(string nameFile) //파일에서 클라이언트에서 사용가능한 전체 콘솔명령어 리스트를 불러와 자동완성 후보로 등록
        {
            try
            {

                UInt16 indexCommand = 0;
                string[] listCommand = new string[10000];
                string[] listCommandSource = File.ReadAllLines(nameFile);

                foreach (string readerLine in listCommandSource)
                {
                    string[] splitLine = readerLine.Split(':');
                    if (splitLine[0].ToLower().Equals("command") || splitLine[0].ToLower().Equals("variable"))
                    {
                        listCommand[indexCommand++] = splitLine[1].Trim();
                    }
                }

                string[] returnListCommand = new string[indexCommand];
                
                for (int index = 0; index < indexCommand; index++)
                {
                    returnListCommand[index] = listCommand[index];
                }

                if (returnListCommand != null)
                    this.cbCommand.AutoCompleteCustomSource.AddRange(returnListCommand);
}
            catch
            {
                Invoke(AddText, "콘솔명령어 리스트를 불러오지 못했습니다.");
            }
        }

        private structIdNameList[] loadListFromFile(string nameFile, ref ComboBox cbNameList) //파일에서 항목리스트를 불러와 콤보박스에 아이템컬렉션으로 추가하고 구조체 배열로 반환
        {

            try
            {
                structIdNameList[] readerFromFile;
                string[] bufferLine;
                string[] splitLine;
                string[] listValue;

                bufferLine = File.ReadAllLines(nameFile, Encoding.UTF8);
                readerFromFile = new structIdNameList[bufferLine.Length];
                listValue = new string[bufferLine.Length];
                for (int i = 0; i < bufferLine.Length; i++)
                {
                    splitLine = bufferLine[i].Split('|');
                    readerFromFile[i].id = splitLine[0].Trim();
                    readerFromFile[i].name_dev = splitLine[1].Trim();
                    readerFromFile[i].name = splitLine[2].Trim();
                    listValue[i] = readerFromFile[i].name + "(" + readerFromFile[i].id + ":" + readerFromFile[i].name_dev + ")";
                }


                cbNameList.Items.Clear(); //참조로 전달받은 콤보박스의 리스트를 클리어
                cbNameList.Items.AddRange(listValue); //참조로 전달받은 콤보박스에 이름 리스트를 추가

                return readerFromFile;
            }
            catch
            {
                Invoke(AddText, nameFile + "파일을 열 수 없습니다.");
                return null;
            }
            
        }

        private void cbDevNameItems_CheckedChanged(object sender, EventArgs e) //Item Dev자동완성에 체크가 되면 콤보박스 내용 교첸
        {
            try
            {
                string[] listValue = new string[listItem.Length];
                this.cbGetItem.Items.Clear();

                if (this.cbDevNameItems.Checked)
                {

                    for (int i = 0; i < listItem.Length; i++)
                    {
                        listValue[i] = listItem[i].name_dev + "(" + listItem[i].id + ":" + listItem[i].name + ")";
                    }
                    this.cbGetItem.Items.AddRange(listValue);
                }
                else
                {
                    for (int i = 0; i < listItem.Length; i++)
                    {
                        listValue[i] = listItem[i].name + "(" + listItem[i].id + ":" + listItem[i].name_dev + ")";
                    }
                    this.cbGetItem.Items.AddRange(listValue);
                }
            }
            catch
            {
                Invoke(AddText, "Item리스트가 비어있습니다.");
            }

        }

        private void cbHotkey_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbHotkey.Checked)
            {
                HookedKeyboardNofity = null;
                //훅 이벤트를 연결한다.
                HookedKeyboardNofity += new KeyboardHooker.HookedKeyboardUserEventHandler(JC_HookedKeyboardNofity);
                //자동으로 훅을 시작한다. 여기서 훅에 의한 이벤트를 연결시킨다.
                KeyboardHooker.Hook(HookedKeyboardNofity);
            }
            else
            {
                KeyboardHooker.UnHook();
            }

        }
       
        private string loadNameBt(Button btName) //버튼의 명령 이름 불러오기
        {
            
            XmlAttributeCollection loadPrestLine = getCommandLine(btName.Name);
            
            if (loadPrestLine.GetNamedItem("cmdName") == null || loadPrestLine.GetNamedItem("cmdName").Value == "")
            {
                return "+"; //사용하지 않는 버튼일 경우 '+' 출력
            }
            else
            {
                return loadPrestLine.GetNamedItem("cmdName").Value; //사용중인 버튼일 경우 명령이름 출력
            }
            
        }

        private string[] loadNameTabpage() //모든 탭의 이름 불러오기
        {
            string[] nameSetTabpage = new string[tab_count];

            if (xmlSavePreset.SelectSingleNode("Preset/nameTabPage") == null)
            {
                setCommandLine(nameSetTabpage); //노드가 없으면 모든 속성을 null로 초기화하여 새로 생성함
            }    
            XmlAttributeCollection loadPrestLine = xmlSavePreset.SelectSingleNode("Preset/nameTabPage").Attributes;
            
            for (int i = 0; i < tab_count; i++)
            {
                string nameTabpage = (loadPrestLine.GetNamedItem("tabPage_" + (i + 1).ToString())).Value;
                if (nameTabpage == null || nameTabpage == "")
                {
                    nameSetTabpage[i] = "";
                }
                else
                {
                    nameSetTabpage[i] = nameTabpage;
                }
            }
            

            return nameSetTabpage;
        }

        private void loadPreset(string fileName) //설정 파일 불러오기
        {
            try
            {
                xmlSavePreset.Load(fileName);
                
            }
            catch
            {
                createPreset(fileName);
                Invoke(AddText, "설정파일이 존재하지 않습니다." + Environment.NewLine + "새로운 설정파일 " + fileName + "을 생성하였습니다.");
                //Invoke(AddText, "새로운 설정파일 " + fileName+ "을 생성하였습니다.");
            }

        }

        private void savePreset(string fileName) //설정 파일 내보내기
        {   
            xmlSavePreset.Save(fileName);
        }

        private void createPreset(string fileName) //설정 파일 생성하기
        {
            XmlNode sourceNode = null;
            XmlElement sourceElement = null;
            

            xmlSavePreset.AppendChild(xmlSavePreset.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            sourceNode = xmlSavePreset.CreateElement("Preset");// 최상위 노드를 만든다.
            xmlSavePreset.AppendChild(sourceNode);
            
            for (int i = 0; i < tab_count;i++ )
            {
                for (int j = 0; j < bt_count; j++)
                {
                    string nameBT = "btSet" + (i + 1).ToString() + "_" + (j + 1).ToString();
                    //sourceNode.AppendChild(xmlSavePreset.CreateElement(nameBT));

                    sourceElement = xmlSavePreset.CreateElement(nameBT);

                    sourceElement.SetAttribute("cmdName", "");
                    sourceElement.SetAttribute("toggle", "false");
                    sourceElement.SetAttribute("argument", "false");
                    sourceElement.SetAttribute("target", "false");
                    sourceElement.SetAttribute("confirmation", "false");
                    sourceElement.SetAttribute("commandLine_1", "");
                    sourceElement.SetAttribute("commandLine_2", "");

                    sourceNode.AppendChild(sourceElement);
                    xmlSavePreset.AppendChild(sourceNode);
                }
            }
            
            //sourceNode = xmlSavePreset.SelectSingleNode("Preset"); 
            //sourceNode = xmlSavePreset.DocumentElement;

            DirectoryInfo pathPreset = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JC");

            if (!(pathPreset.Exists))
                pathPreset.Create();

            savePreset(fileName); //생성된 XML 파일로 저장
            
        }


        private void disconnect() //연결 종료시마다 호출
        {

            if (this.ClientCon)
            {
                this.ClientCon = false;
                Invoke(AddConn, "연결");
                Invoke(AddText, "클라이언트와 연결이 종료되었습니다.");
                Invoke(AddPort, "true"); //포트 입력창 활성화

            }

   
            try
            {
                if (!(slot1_Reader == null))
                {
                    this.slot1_Reader.Abort(); //통신 스레드 종료
                }
                if (!(jcStream == null))
                {
                    jcStream.Close(); //NetworkStream 클래스 개체 리소스 해제
                }
                if (!(tcClient == null))
                {
                    tcClient.Close(); //TcpClient 클래스 개체 리소스 해제
                }
            }
            catch { }



        }

        private void buttonAllClose()
        {
            
        }

        private void btExit_Click(object sender, EventArgs e) // 종료 버튼
        {
            disconnect(); 
            this.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e) //폼 투명도 조절
        {
            this.Opacity = (double)tbOpacity.Value/10;
        }

        private void messageView(string strText) //로그 텍스트 박스에 메시지 한줄 출력
        {
            var dt = Convert.ToString(DateTime.Now);

            this.tbLog.AppendText(strText + "\n");
            this.tbLog.AppendText("<" + dt + ">" + "\n");
        }

        private void portView(string textPort)
        {
            switch (textPort)
            {
                case "true":
                    this.tbPort.Enabled = true;
                    break;
                case "false":
                    this.tbPort.Enabled = false;
                    break;
                default:
                    this.tbPort.Text = textPort;
                    break;
            
            }
            
        }

        private void buttonView(string textConn)
        {
            this.btConn.Text = textConn;

            if (this.btConn.Text == "해제")
            {
                this.btConn.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                this.btConn.BackColor = Button.DefaultBackColor;
            }
        }

        private void targetView(string targetName)
        {
            this.btGetTarget.Text = targetName;
            Clipboard.SetText(targetName);
            Invoke(AddText, "타겟 이름 '" + targetName + "'이(가) 클립보드에 복사되었습니다.");
        }

        public void msgSend(String msgLine, UInt16 operType) //메시지 전송
        {
            try
            {
  
                byte[] packetBody;
                byte[] packetHeaderSize = new byte[2];
                byte[] packetHeaderType = new byte[2];
                UInt16 bodySize;
//여기부터 새코드 시작
                string[] commandLine = msgLine.Split('\r');
                foreach (string command in commandLine)
                {
                    UInt16 dTime = 0;
                    try
                    {
                        dTime = UInt16.Parse(command); //정수만 입력된 행은 해당 수치만큼 쓰레드 슬립 
                        this.Enabled = false; //쓰레드가 슬립중일 때, 폼 비활성화
                        Thread.Sleep(dTime);
                        this.Enabled = true;
                    }
                    catch //정수로 파싱할 수 없는 행은 명령어로 간주하여 바로 명령 전송
                    {
                        packetBody = Encoding.UTF8.GetBytes(command);
                        bodySize = (UInt16)(packetBody.Length + 2);

                        packetHeaderSize = BitConverter.GetBytes(bodySize);
                        packetHeaderType = BitConverter.GetBytes(operType); //명령어타입이 0일경우 무시, 1일경우 예외처리함

                        jcStream.Write(packetHeaderSize, 0, 2);
                        jcStream.Write(packetHeaderType, 0, 2);
                        jcStream.Write(packetBody, 0, bodySize - 2); //명령어 타입 2바이트를 제외
                        jcStream.Flush();
                    }

                }

//여기까지 새코드

/*
                packetBody = Encoding.UTF8.GetBytes(msgLine);
                bodySize = (UInt16)(packetBody.Length + 2);
                
                packetHeaderSize = BitConverter.GetBytes(bodySize);
                packetHeaderType = BitConverter.GetBytes(operType); //명령어타입이 0일경우 무시, 1일경우 예외처리함

                jcStream.Write(packetHeaderSize, 0, 2);
                jcStream.Write(packetHeaderType, 0, 2);
                jcStream.Write(packetBody, 0, bodySize - 2); //명령어 타입 2바이트를 제외
                jcStream.Flush();
*/

                //디버그용
                //Invoke(AddText, BitConverter.ToString(packetHeaderSize) + "-" + BitConverter.ToString(packetHeaderType) + "-" + BitConverter.ToString(packetBody)); 
                
                //var dt = Convert.ToString(DateTime.Now);
                //jcWrite.WriteLine(msgLine);
                //jcWrite.Flush();

                //Invoke(AddText, msgLine + ".." + dt);
            }
            catch
            {
                //Invoke(AddText, "클라이언트에 명령을 전달할 수 없습니다.");
                disconnect();
            }
        }

        private void msgReceive() //메시지 수신
        {
            
            try
            {
                byte msg = new byte();
                byte[] packetBody = new byte[2000];
                byte[] packetHeaderSize = new byte[2];
                byte[] packetHeaderType = new byte[2];
                UInt16 bodySize, cmdType;
                string receiveCommand = null;

                while (this.ClientCon)
                {
                    //Invoke(AddText, jcStream.ReadByte().ToString());
                    

                    for (int index = 0; index < 2; index++)
                    {
                        msg = (byte)jcStream.ReadByte();
                        
                        if (msg < 0)
                        {
                            this.ClientCon = false;
                            //Invoke(AddText, "클라이언트와 연결이 종료되어 메세지를 수신할 수 없습니다.");
                            disconnect();
                        }
                        else
                        {
                            packetHeaderSize[index] = msg;
                        }
                    }
                    bodySize = BitConverter.ToUInt16(packetHeaderSize, 0);

                    for (int index = 0; index < 2; index++)
                    {
                        msg = (byte)jcStream.ReadByte();
                        packetHeaderType[index] = msg;
                    }
                    cmdType = BitConverter.ToUInt16(packetHeaderType, 0);

                    for (int index = 0; index < bodySize - 2; index++)
                    {
                        msg = (byte)jcStream.ReadByte();
                        
                        if (msg <= 0)
                            packetBody[index] = (byte)' ';
                        
                        else
                            packetBody[index] = msg;
                        
                    }

                    receiveCommand = Encoding.UTF8.GetString(packetBody, 0, bodySize - 2);

                    switch (cmdType)
                    {
                        case 0: //명령어타입이 0일경우 수신결과를 로그출력
                            Invoke(AddText, receiveCommand);
                            receiveCommand = null;
                            break;
                            
                        case 1: //0 외는 수신한 데이터를 별도로 처리한다. 추후 많은 확장을 할 수 있을 것으로 기대한다.
                            exceptedCommand(receiveCommand);
                            receiveCommand = null;
                            break;

                    }

                }
                disconnect();
            }
            catch
            {
                //Invoke(AddText, "메시지를 수신하는 과정에서 오류가 발생하였습니다.");
                disconnect();
            }
        }

        private void exceptedCommand(string commandLine) //예외처리할 명령어 실행
        {
            string[] parseCommand = commandLine.Split('|');
            switch (parseCommand[0].ToLower())
            {
                case "targetname": //타겟네임을 수신하여 사용
                    Invoke(AddTargetName, parseCommand[1]);
                    //Invoke(AddText, parseCommand[1]);
                    break;
            }

        }

        private void cbAlwaysOnTop_CheckedChanged(object sender, EventArgs e) //항상 위 동작여부 체크
        {
            if (this.cbAlwaysOnTop.Checked)
                this.TopMost = true;
            else
                this.TopMost = false;
        }

        private void formJCommand_FormClosed(object sender, FormClosedEventArgs e) //메인 폼 닫힐 때, 사용중인 자원 정리 
        {
            disconnect();
            KeyboardHooker.UnHook();
        }

        private void btConn_Click(object sender, EventArgs e) //연결버튼 누를 때마다 서버 연결 또는 해제
        {
            this.btConn.Enabled = false; //이벤트 처리 동안 버튼 비활성화

            if (!(this.ClientCon))
            {
                this.Port_slot1 = Convert.ToInt32(tbPort.Text);
                ClientConnection(); //클라이언트 연결 함수 호출
/*
                if (this.Port_slot1 < 9111 || this.Port_slot1 > 9112) //포트 대역 검증
                {
                    MessageBox.Show("9111 ~ 9112 사이의 포트를 사용할 수 있습니다.");
                    this.tbPort.Text = "9111";
                }
                else
                {

                    ClientConnection();
                }
 */ 
                
            }
            else
            {
                disconnect(); //클라이언트 종료 함수 호출
                //btConn.Enabled = true;
            }
            this.btConn.Enabled = true;
        }

        private void ClientConnection() //JC와 클라이언트 연결
        {

            try
            {
                Invoke(AddPort, "false"); //연결 중인 동안 포트 수정 불가
                tcClient = new TcpClient(addr, Port_slot1);
                jcStream = tcClient.GetStream();
                doitdoit.doitStream = jcStream;
                
                Invoke(AddText, "클라이언트에 연결되었습니다.");
                Invoke(AddConn, "해제");
                //this.btConn.Text = "해제";
                this.ClientCon = true;

                slot1_Reader = new Thread(msgReceive);
                slot1_Reader.Start();

            }
            catch
            {
                this.ClientCon = false;
                Invoke(AddText, "클라이언트와 연결하지 못했습니다.");
                Invoke(AddConn, "연결");
                Invoke(AddPort, "true"); //포트 입력창 활성화
                
/*              추후에 게임 클라이언트에서 포트 자동 변경 기능이 들어가면 사용 예정  
                if (Port_slot1 < 9112 && !(this.ClientCon))
                {
                    ++this.Port_slot1;
                    Invoke(AddPort, Port_slot1.ToString());
                    ClientConnection();
                }
                else
                {
                    this.ClientCon = false;
                    Invoke(AddText, "클라이언트와 연결하지 못했습니다.");
                    Invoke(AddConn, "연결");
                    Invoke(AddPort, Port_slot1);
                    
                }
 */ 
            }
        }


        private XmlAttributeCollection getCommandLine(string nameBt) //버튼 이름에 매핑된 애트리뷰트 가져오기
        {
            try
            {
                XmlNode getNode;
                XmlAttributeCollection selectedNode;

                bool flagChanged = false;
                //string cmdName, toggle, argument, target, confirmation, commandLine_1, commandLine_2 = null;
                structPreset setPreset;
                setPreset.nameBt = nameBt;

                if ((xmlSavePreset.SelectSingleNode("Preset/" + nameBt)) == null) //노드가 없을 경우 새로 생성함
                {
                    setPreset.cmdName = null;
                    setPreset.toggle = null;
                    setPreset.argument = null;
                    setPreset.target = null;
                    setPreset.confirmation = null;
                    setPreset.commandLine_1 = null;
                    setPreset.commandLine_2 = null;
                    setCommandLine(setPreset);
                }

                getNode = xmlSavePreset.SelectSingleNode("Preset/" + nameBt);
                selectedNode = getNode.Attributes;
                

                if (selectedNode.GetNamedItem("cmdName") == null)
                {
                    setPreset.cmdName = "";
                    flagChanged = true;
                }
                else
                {
                    setPreset.cmdName = selectedNode.GetNamedItem("cmdName").Value;
                }

                if (selectedNode.GetNamedItem("toggle") == null)
                {
                    setPreset.toggle = "false";
                    flagChanged = true;
                }
                else
                {
                    setPreset.toggle = selectedNode.GetNamedItem("toggle").Value;
                }

                if (selectedNode.GetNamedItem("argument") == null)
                {
                    setPreset.argument = "false";
                    flagChanged = true;
                }
                else
                {
                    setPreset.argument = selectedNode.GetNamedItem("argument").Value;
                }

                if (selectedNode.GetNamedItem("target") == null)
                {
                    setPreset.target = "false";
                    flagChanged = true;
                }
                else
                {
                    setPreset.target = selectedNode.GetNamedItem("target").Value;
                }


                if (selectedNode.GetNamedItem("confirmation") == null)
                {
                    setPreset.confirmation = "false";
                    flagChanged = true;
                }
                else
                {
                    setPreset.confirmation = selectedNode.GetNamedItem("confirmation").Value;
                } 
                
                if (selectedNode.GetNamedItem("commandLine_1") == null)
                {
                    setPreset.commandLine_1 = "";
                    flagChanged = true;
                }
                else
                {
                    setPreset.commandLine_1 = selectedNode.GetNamedItem("commandLine_1").Value;
                } 
                
                if (selectedNode.GetNamedItem("commandLine_2") == null)
                {
                    setPreset.commandLine_2 = "";
                    flagChanged = true;
                }
                else
                {
                    setPreset.commandLine_2 = selectedNode.GetNamedItem("commandLine_2").Value;
                }


                if (flagChanged) //설정파일이 구버전이라 속성값이 일치하지 않을 경우 해당 노드를 갱신하여 줌
                {
                    setCommandLine(setPreset);
                }


                return selectedNode;
            
            }
            catch
            {
                Invoke(AddText, "프리셋 설정 파일에 문제가 있습니다.");
                return null;
            }
        }

        private void setCommandLine(structPreset setPreset)
        //버튼의 설정으로 입력된 값을 XML파일에 쓰기
        {
            
            XmlNode getNode = xmlSavePreset.SelectSingleNode("Preset");
            
            XmlElement sourceElement = xmlSavePreset.CreateElement(setPreset.nameBt);

            sourceElement.SetAttribute("cmdName", setPreset.cmdName);
            sourceElement.SetAttribute("toggle", setPreset.toggle);
            sourceElement.SetAttribute("argument", setPreset.argument);
            sourceElement.SetAttribute("target", setPreset.target);
            sourceElement.SetAttribute("confirmation", setPreset.confirmation);
            sourceElement.SetAttribute("commandLine_1", setPreset.commandLine_1);
            sourceElement.SetAttribute("commandLine_2", setPreset.commandLine_2);
            try
            {
                getNode.ReplaceChild(sourceElement, getNode.SelectSingleNode(setPreset.nameBt)); //기존설정이 있을경우 교체 (구버전 설정파일도 자동 교체됨)
            }
            catch
            {
                getNode.AppendChild(sourceElement); //새로운 설정일 경우 자식의 맨 마지막에 추가함
            }


/*          프리셋 파일에 새로운 속성이 들어갈 경우, 기존 프리셋 파일을 못쓰게 되는 상황을 막기 위해
 *          내용을 수정할 경우에 해당 노드 전체를 새로 쓰도록 수정하였음
 *          문제가 발생할 경우 수정을 위해 아래의 구코드를 보존함
            XmlNode getNode = xmlSavePreset.SelectSingleNode("Preset" + nameBt);
            XmlAttributeCollection selectedNode = getNode.Attributes;

            if (selectedNode.GetNamedItem("cmdName") != null)
            {
                selectedNode.GetNamedItem("cmdName").Value = cmdName;
            }
            if (selectedNode.GetNamedItem("toggle") != null)
            {
                selectedNode.GetNamedItem("toggle").Value = toggle;
            }
            if (selectedNode.GetNamedItem("argument") != null)
            {
                selectedNode.GetNamedItem("argument").Value = argument;
            }
            if (selectedNode.GetNamedItem("confirmation") != null)
            {
                selectedNode.GetNamedItem("confirmation").Value = confirmation;
            }
            if (selectedNode.GetNamedItem("commandLine_1") != null)
            {
                selectedNode.GetNamedItem("commandLine_1").Value = commandLine_1;
            }
            if (selectedNode.GetNamedItem("commandLine_2") != null)
            {
                selectedNode.GetNamedItem("commandLine_2").Value = commandLine_2;
            }
 */
            savePreset(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JC\preset.xml");

        }

        private void setCommandLine(string[] nameSetTabpage)
        //탭이름이 변경되면 파일에 쓰기
        {

            XmlNode getNode = xmlSavePreset.SelectSingleNode("Preset");

            XmlElement sourceElement = xmlSavePreset.CreateElement("nameTabPage");

            for (int i = 0; i < tab_count; i++)
            {
                sourceElement.SetAttribute("tabPage_" + (i+1).ToString(), nameSetTabpage[i]);
            }
            try
            {
                getNode.ReplaceChild(sourceElement, getNode.SelectSingleNode("nameTabPage")); //기존설정이 있을경우 교체
            }
            catch
            {
                getNode.PrependChild(sourceElement); //설정파일에 탭 설정이 없을 경우 맨 앞에 추가함
            }


            savePreset(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JC\preset.xml");

        }


        private void btMinimum_Click(object sender, EventArgs e) //메인 폼 접기/펼치기 버튼 클릭 이벤트
        {
            if (!(this.setMinimum))
            {
                this.Size = new Size(this.Size.Width ,25);
                this.setMinimum = true;
                this.btMinimum.Text = "펴기";
            }
            else
            {
                this.Size = new Size(this.Size.Width, 230);
                this.setMinimum = false;
                this.btMinimum.Text = "접기";
            }
        }


        private void formJCommand_KeyDown(object sender, KeyEventArgs e) //ESC를 누르면 창 최소화 하기 
        {
            if (e.KeyCode == Keys.Escape)
            {
//                this.WindowState = FormWindowState.Minimized; //사용은 검토 후 진행
            }
        }

        private void btSet_Click(object sender, EventArgs e) //명령어가 저장된 버튼을 클릭했을 때, 저장된 명령 실행
        {
            Button thisButton = (Button)sender;

            if (Control.ModifierKeys == Keys.Control)
            {
                btSet_copyCommand(thisButton);
            }
            else
            {
                btSet_ex(thisButton);
            }
        }

        private void btSet_copyCommand(Button thisButton)
        {
            try
            {
                string cmdNameValue = null;
                string command_1Value = null;
                string nameBt = thisButton.Name;

                XmlAttributeCollection selectedNode = getCommandLine(nameBt);
                cmdNameValue = selectedNode.GetNamedItem("cmdName").Value.ToString(); //명령어 라인의 이름 가져오기
                command_1Value = selectedNode.GetNamedItem("commandLine_1").Value.ToString(); //명령어 라인 가져오기


                Clipboard.SetText(command_1Value);
                Invoke(AddText, "'" + cmdNameValue + "'" + "의 명령어 라인이 클립보드에 복사되었습니다.");
            }
            catch
            { }

        }

        private void btSet_ex(Button thisButton)
        {
            bool toggleValue = false;
            bool argumentValue = false;
            bool targetValue = false;
            bool confirmationValue = false;
            string cmdNameValue = null;
            string command_1Value = null;
            string command_2Value = null;
            string resultCommandLine = null;
            string nameBt = thisButton.Name;
            XmlAttributeCollection selectedNode = getCommandLine(nameBt);


            try
            {
                toggleValue = bool.Parse(selectedNode.GetNamedItem("toggle").Value);
                argumentValue = bool.Parse(selectedNode.GetNamedItem("argument").Value);
                targetValue = bool.Parse(selectedNode.GetNamedItem("target").Value);
                confirmationValue = bool.Parse(selectedNode.GetNamedItem("confirmation").Value);
                cmdNameValue = selectedNode.GetNamedItem("cmdName").Value.ToString();
                command_1Value = selectedNode.GetNamedItem("commandLine_1").Value.ToString();
                command_2Value = selectedNode.GetNamedItem("commandLine_2").Value.ToString();

                if (toggleValue) //토글 명령어일 경우
                {
                    if (thisButton.BackColor != System.Drawing.Color.Gold)
                    {
                        resultCommandLine = command_1Value;
                        thisButton.BackColor = System.Drawing.Color.Gold;

                    }
                    else
                    {
                        resultCommandLine = command_2Value;
                        thisButton.BackColor = Button.DefaultBackColor;
                    }

                    if (targetValue) //타겟을 요구할 때
                    {
                        resultCommandLine += " " + this.btGetTarget.Text;
                    }

                    if (argumentValue) //인자를 요구할 때
                    {
                        frmInput argInput = new frmInput();
                        argInput.Text = thisButton.Text;
                        argInput.ShowDialog();
                        resultCommandLine += argInput.addInput;
                    
                    }


                }
                else //토글 명령어가 아닐 경우
                {
                    thisButton.BackColor = Button.DefaultBackColor;
                    resultCommandLine = command_1Value; //항상 command_1Value만 실행함

                    if (targetValue) //타겟을 요구할 때
                    {
                        resultCommandLine += " " + this.btGetTarget.Text;
                    }

                    if (argumentValue) //인자를 요구할 때
                    {
                        frmInput argInput = new frmInput();
                        argInput.Text = thisButton.Text;
                        argInput.ShowDialog();
                        resultCommandLine += argInput.addInput;

                    }


                }

                if (confirmationValue) //확인 후 실행 명령어에 대한 처리
                {
                    if (MessageBox.Show
                        (
                            "[" + cmdNameValue + "]" + 
                            " 명령을 실행해도 좋습니까?" + 
                            Environment.NewLine + 
                            "실행할 명령어: " +
                            Environment.NewLine +
                            resultCommandLine , 
                            "경고", 
                            MessageBoxButtons.YesNo
                        ) == DialogResult.No)
                    {
                        cmdNameValue = null;//No선택 시, 명령어 이름을 초기화하여 실행을 회피함

                        if (toggleValue) //토글 명령어일 경우, 버튼 ON/OFF 초기화
                        {
                            if (thisButton.BackColor != System.Drawing.Color.Gold)
                                thisButton.BackColor = System.Drawing.Color.Gold;
                            else
                                thisButton.BackColor = Button.DefaultBackColor;
                        }
                    }
                }

                if (cmdNameValue != "" && cmdNameValue != null)
                {
                    msgSend(resultCommandLine, 0);
                }
            }
            catch
            {
                Invoke(AddText, "프리셋 설정 파일에 문제가 있습니다.");
            }
        }
    


        private void btSet_MouseDown(object sender, MouseEventArgs e) //각 버튼에 마우스를 우클릭을 하면 버튼 재설정 하기
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    string nameBt = ((Button)sender).Name;
                    bool tempBoolToString;
                    //bool toggleValue, argumentValue, confirmationValue, targetValue = false;
                    //string cmdNameValue, command_1Value, command_2Value;
                    frmPreset inputFrmPreset = new frmPreset();
                    XmlAttributeCollection selectedNode = getCommandLine(nameBt);
                    structPreset setPreset;
                    
                    if (selectedNode.GetNamedItem("cmdName") != null)
                    {
                        inputFrmPreset.cmdNameValue = selectedNode.GetNamedItem("cmdName").Value.ToString();
                    }
                    if (selectedNode.GetNamedItem("toggle") != null)
                    {
                        try
                        {
                            inputFrmPreset.toggleValue = bool.Parse(selectedNode.GetNamedItem("toggle").Value);
                        }
                        catch
                        {
                            inputFrmPreset.toggleValue = false;
                        }
                    }
                    if (selectedNode.GetNamedItem("argument") != null)
                    {
                        try
                        {
                            inputFrmPreset.argumentValue = bool.Parse(selectedNode.GetNamedItem("argument").Value);
                        }
                        catch
                        {
                            inputFrmPreset.argumentValue = false;
                        }
                    }
                    if (selectedNode.GetNamedItem("target") != null)
                    {
                        try
                        {
                            inputFrmPreset.targetValue = bool.Parse(selectedNode.GetNamedItem("target").Value);
                        }
                        catch
                        {
                            inputFrmPreset.targetValue = false;
                        }

                    }
                    if (selectedNode.GetNamedItem("confirmation") != null)
                    {
                        try
                        {
                            inputFrmPreset.confirmationValue = bool.Parse(selectedNode.GetNamedItem("confirmation").Value);
                        }
                        catch
                        {
                            inputFrmPreset.confirmationValue = false;
                        }

                    }
                    if (selectedNode.GetNamedItem("commandLine_1") != null)
                    {
                        inputFrmPreset.command_1Value = selectedNode.GetNamedItem("commandLine_1").Value.ToString();
                    }
                    if (selectedNode.GetNamedItem("commandLine_2") != null)
                    {
                        inputFrmPreset.command_2Value = selectedNode.GetNamedItem("commandLine_2").Value.ToString();
                    }

                    inputFrmPreset.ShowDialog();

                    setPreset.nameBt = nameBt;
                    setPreset.cmdName = inputFrmPreset.cmdNameValue;
                    tempBoolToString = inputFrmPreset.toggleValue; //참조로 마샬링하기 위해서 지역변수에 복사후 진행
                    setPreset.toggle = tempBoolToString.ToString().ToLower();
                    tempBoolToString = inputFrmPreset.argumentValue; //참조로 마샬링하기 위해서 지역변수에 복사후 진행
                    setPreset.argument = tempBoolToString.ToString().ToLower();
                    tempBoolToString = inputFrmPreset.targetValue; //참조로 마샬링하기 위해서 지역변수에 복사후 진행
                    setPreset.target = tempBoolToString.ToString().ToLower();
                    tempBoolToString = inputFrmPreset.confirmationValue; //참조로 마샬링하기 위해서 지역변수에 복사후 진행
                    setPreset.confirmation = tempBoolToString.ToString().ToLower();
                    setPreset.commandLine_1 = inputFrmPreset.command_1Value;
                    setPreset.commandLine_2 = inputFrmPreset.command_2Value;

                    setCommandLine(setPreset); //새로운 설정으로 파일에 쓰기
               
                    if (inputFrmPreset.cmdNameValue == "" || inputFrmPreset.cmdNameValue == null) //입력된 명령이 없을경우 버튼텍스트를 '+'로 바꿈
                    {
                        ((Button)sender).Text = "+";
                    }
                    else
                    {
                        ((Button)sender).Text = inputFrmPreset.cmdNameValue;
                    }
                    


                }
            }

            catch
            {
                Invoke(AddText, "저장된 명령어를 불러오거나 저장하는 과정에서 문제가 발생하였습니다.");
            }

        }


        private void btHidden_Click(object sender, EventArgs e) //숨기기 버튼 클릭
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void cbCommand_KeyDown(object sender, KeyEventArgs e)//커맨드 라인을 직접 입력받아서 실행함
        {
            if ((e.KeyCode == Keys.Enter) && (!((ComboBox)sender).Text.Equals(""))) //아무것도 입력하지 않았을 경우 명령 전송하지 않음
            {
                msgSend(this.cbCommand.Text, 0);
                if (this.cbCommand.Items.Count > 100) //100개로 히스토리 개수 제한
                {
                    this.cbCommand.Items.RemoveAt(100);
                }

                this.cbCommand.Items.Insert(0, this.cbCommand.Text); //히스토리에 항목 추가

            }
            else if (e.KeyCode == Keys.Escape)
            {
                ((ComboBox)sender).SelectAll();
            }
            else if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {

                    ((ComboBox)sender).SelectAll();
                }
            }
        }

        private void btGetItem_Click(object sender, EventArgs e)
        {
            string argument = this.cbGetItem.Text; //아이템 이름의 일부만 입력하면 검색해주는 기능 사용을 위해 
            try
            {
                argument = this.listItem[this.cbGetItem.SelectedIndex].id;

                if (this.cbGetItem.Text != "")
                {
                    msgSend("/ITEM_Give " + argument, 0);
                }
            }
            catch
            {
                if (cbGetItem.Text != "")
                    msgSend("/ITEM_Give " + argument, 0);
            }
        }
        private void btGetVehicle_Click(object sender, EventArgs e)
        {
            string argument = this.cbGetVehicle.Text;
            try
            {
                argument = this.listVehicle[this.cbGetVehicle.SelectedIndex].id;

                if (this.cbGetVehicle.Text != "")
                {
                    msgSend("/VEHICLE_Create " + argument, 0);
                }
            }
            catch
            {
                if (cbGetVehicle.Text != "")
                    msgSend("/VEHICLE_Create " + argument, 0);
            }
        }
        private void btGetNpc_Click(object sender, EventArgs e)
        {
            string argument = this.cbGetNpc.Text;
            try
            {
                argument = this.listNpc[this.cbGetNpc.SelectedIndex].id;

                if (this.cbGetNpc.Text != "")
                {
                    msgSend("/npc_spawn " + argument, 0);
                }
            }
            catch
            {
                if (cbGetNpc.Text != "")
                    msgSend("/npc_spawn " + argument, 0);
            }
        }


        private void cbContentsList_KeyDown(object sender, KeyEventArgs e) //컨텐츠 리스트가 담긴 콤보박스의 키 입력 이벤트 처리
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btContentsListClick((ComboBox)sender, e);
                    ((ComboBox)sender).Focus();
                    break;
                case Keys.Escape:
                    ((ComboBox)sender).SelectAll();
                    break;
                case Keys.A:
                    if (e.Control)
                    {
                        ((ComboBox)sender).SelectAll();
                    }
                    break;
                default:
                    break;
            }
        }

        private void btContentsListClick(ComboBox sender, KeyEventArgs e) //컨텐츠 리스트가 담긴 콤보박스에 따라 맞는 실행 버튼을 찾아 실행
        {
            switch (sender.Name)
            {
                case "cbGetItem":
                    btGetItem_Click(sender, e);
                    break;
                case "cbGetVehicle":
                    btGetVehicle_Click(sender, e);
                    break;
                case "cbGetNpc":
                    btGetNpc_Click(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void btGetTarget_Click(object sender, EventArgs e)
        {
            msgSend("/Tool_getTargetName", 1);
        }

        private void btCommandMultiLine_Click(object sender, EventArgs e)
        {
            msgSend(tbCommandMultiLine.Text, 0);
        }

        private void btDoitDoit_Click(object sender, EventArgs e)
        {
            if (doitdoit.Visible)
            {
                doitdoit.Hide();
            }
            else
            {
                doitdoit.Show();
            }
            
        }

        private void tbCommandMultiLine_KeyDown(object sender, KeyEventArgs e) //
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {

                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void tcButtonSet_DoubleClick(object sender, EventArgs e) //탭이름 변경 
        {
            string[] nameSetTabpage = new string[tab_count];
            frmInput nameTabpage = new frmInput();
            nameTabpage.Text = tcButtonSet.SelectedTab.Text + " 이름변경";
            nameTabpage.addInput = tcButtonSet.SelectedTab.Text; //현재 탭 이름을 입력창에 기본 텍스트로 보냄
            nameTabpage.ShowDialog();
            tcButtonSet.SelectedTab.Text = nameTabpage.addInput.Trim(); //리턴 문자열의 앞 공백 제거
            foreach (TabPage page in tcButtonSet.TabPages)
            {
                nameSetTabpage[(tcButtonSet.TabPages.IndexOf(page))] = page.Text;
            }
            setCommandLine(nameSetTabpage);

        }




        private void runSetButtonName() //초기 탭/버튼 텍스트 설정. 폼 디자이너를 사용하기 위해 일단 무식하게 엑셀로 작성하였음
        {
            string[] nameSetTabpage = loadNameTabpage();

            foreach (TabPage page in tcButtonSet.TabPages)
            {
                page.Text = nameSetTabpage[(tcButtonSet.TabPages.IndexOf(page))];
            }
            //tab1 
            this.btSet1_1.Text = loadNameBt(btSet1_1);
            this.btSet1_2.Text = loadNameBt(btSet1_2);
            this.btSet1_3.Text = loadNameBt(btSet1_3);
            this.btSet1_4.Text = loadNameBt(btSet1_4);
            this.btSet1_5.Text = loadNameBt(btSet1_5);
            this.btSet1_6.Text = loadNameBt(btSet1_6);
            this.btSet1_7.Text = loadNameBt(btSet1_7);
            this.btSet1_8.Text = loadNameBt(btSet1_8);
            this.btSet1_9.Text = loadNameBt(btSet1_9);
            this.btSet1_10.Text = loadNameBt(btSet1_10);
            this.btSet1_11.Text = loadNameBt(btSet1_11);
            this.btSet1_12.Text = loadNameBt(btSet1_12);
            this.btSet1_13.Text = loadNameBt(btSet1_13);
            this.btSet1_14.Text = loadNameBt(btSet1_14);
            this.btSet1_15.Text = loadNameBt(btSet1_15);
            this.btSet1_16.Text = loadNameBt(btSet1_16);
            this.btSet1_17.Text = loadNameBt(btSet1_17);
            this.btSet1_18.Text = loadNameBt(btSet1_18);
            this.btSet1_19.Text = loadNameBt(btSet1_19);
            this.btSet1_20.Text = loadNameBt(btSet1_20);
            this.btSet1_21.Text = loadNameBt(btSet1_21);
            this.btSet1_22.Text = loadNameBt(btSet1_22);
            this.btSet1_23.Text = loadNameBt(btSet1_23);
            this.btSet1_24.Text = loadNameBt(btSet1_24);
            this.btSet1_25.Text = loadNameBt(btSet1_25);
            this.btSet1_26.Text = loadNameBt(btSet1_26);
            this.btSet1_27.Text = loadNameBt(btSet1_27);
            this.btSet1_28.Text = loadNameBt(btSet1_28);
            this.btSet1_29.Text = loadNameBt(btSet1_29);
            this.btSet1_30.Text = loadNameBt(btSet1_30);

            //tab2
            this.btSet2_1.Text = loadNameBt(btSet2_1);
            this.btSet2_2.Text = loadNameBt(btSet2_2);
            this.btSet2_3.Text = loadNameBt(btSet2_3);
            this.btSet2_4.Text = loadNameBt(btSet2_4);
            this.btSet2_5.Text = loadNameBt(btSet2_5);
            this.btSet2_6.Text = loadNameBt(btSet2_6);
            this.btSet2_7.Text = loadNameBt(btSet2_7);
            this.btSet2_8.Text = loadNameBt(btSet2_8);
            this.btSet2_9.Text = loadNameBt(btSet2_9);
            this.btSet2_10.Text = loadNameBt(btSet2_10);
            this.btSet2_11.Text = loadNameBt(btSet2_11);
            this.btSet2_12.Text = loadNameBt(btSet2_12);
            this.btSet2_13.Text = loadNameBt(btSet2_13);
            this.btSet2_14.Text = loadNameBt(btSet2_14);
            this.btSet2_15.Text = loadNameBt(btSet2_15);
            this.btSet2_16.Text = loadNameBt(btSet2_16);
            this.btSet2_17.Text = loadNameBt(btSet2_17);
            this.btSet2_18.Text = loadNameBt(btSet2_18);
            this.btSet2_19.Text = loadNameBt(btSet2_19);
            this.btSet2_20.Text = loadNameBt(btSet2_20);
            this.btSet2_21.Text = loadNameBt(btSet2_21);
            this.btSet2_22.Text = loadNameBt(btSet2_22);
            this.btSet2_23.Text = loadNameBt(btSet2_23);
            this.btSet2_24.Text = loadNameBt(btSet2_24);
            this.btSet2_25.Text = loadNameBt(btSet2_25);
            this.btSet2_26.Text = loadNameBt(btSet2_26);
            this.btSet2_27.Text = loadNameBt(btSet2_27);
            this.btSet2_28.Text = loadNameBt(btSet2_28);
            this.btSet2_29.Text = loadNameBt(btSet2_29);
            this.btSet2_30.Text = loadNameBt(btSet2_30);

            //tab3
            this.btSet3_1.Text = loadNameBt(btSet3_1);
            this.btSet3_2.Text = loadNameBt(btSet3_2);
            this.btSet3_3.Text = loadNameBt(btSet3_3);
            this.btSet3_4.Text = loadNameBt(btSet3_4);
            this.btSet3_5.Text = loadNameBt(btSet3_5);
            this.btSet3_6.Text = loadNameBt(btSet3_6);
            this.btSet3_7.Text = loadNameBt(btSet3_7);
            this.btSet3_8.Text = loadNameBt(btSet3_8);
            this.btSet3_9.Text = loadNameBt(btSet3_9);
            this.btSet3_10.Text = loadNameBt(btSet3_10);
            this.btSet3_11.Text = loadNameBt(btSet3_11);
            this.btSet3_12.Text = loadNameBt(btSet3_12);
            this.btSet3_13.Text = loadNameBt(btSet3_13);
            this.btSet3_14.Text = loadNameBt(btSet3_14);
            this.btSet3_15.Text = loadNameBt(btSet3_15);
            this.btSet3_16.Text = loadNameBt(btSet3_16);
            this.btSet3_17.Text = loadNameBt(btSet3_17);
            this.btSet3_18.Text = loadNameBt(btSet3_18);
            this.btSet3_19.Text = loadNameBt(btSet3_19);
            this.btSet3_20.Text = loadNameBt(btSet3_20);
            this.btSet3_21.Text = loadNameBt(btSet3_21);
            this.btSet3_22.Text = loadNameBt(btSet3_22);
            this.btSet3_23.Text = loadNameBt(btSet3_23);
            this.btSet3_24.Text = loadNameBt(btSet3_24);
            this.btSet3_25.Text = loadNameBt(btSet3_25);
            this.btSet3_26.Text = loadNameBt(btSet3_26);
            this.btSet3_27.Text = loadNameBt(btSet3_27);
            this.btSet3_28.Text = loadNameBt(btSet3_28);
            this.btSet3_29.Text = loadNameBt(btSet3_29);
            this.btSet3_30.Text = loadNameBt(btSet3_30);

            //tab4
            this.btSet4_1.Text = loadNameBt(btSet4_1);
            this.btSet4_2.Text = loadNameBt(btSet4_2);
            this.btSet4_3.Text = loadNameBt(btSet4_3);
            this.btSet4_4.Text = loadNameBt(btSet4_4);
            this.btSet4_5.Text = loadNameBt(btSet4_5);
            this.btSet4_6.Text = loadNameBt(btSet4_6);
            this.btSet4_7.Text = loadNameBt(btSet4_7);
            this.btSet4_8.Text = loadNameBt(btSet4_8);
            this.btSet4_9.Text = loadNameBt(btSet4_9);
            this.btSet4_10.Text = loadNameBt(btSet4_10);
            this.btSet4_11.Text = loadNameBt(btSet4_11);
            this.btSet4_12.Text = loadNameBt(btSet4_12);
            this.btSet4_13.Text = loadNameBt(btSet4_13);
            this.btSet4_14.Text = loadNameBt(btSet4_14);
            this.btSet4_15.Text = loadNameBt(btSet4_15);
            this.btSet4_16.Text = loadNameBt(btSet4_16);
            this.btSet4_17.Text = loadNameBt(btSet4_17);
            this.btSet4_18.Text = loadNameBt(btSet4_18);
            this.btSet4_19.Text = loadNameBt(btSet4_19);
            this.btSet4_20.Text = loadNameBt(btSet4_20);
            this.btSet4_21.Text = loadNameBt(btSet4_21);
            this.btSet4_22.Text = loadNameBt(btSet4_22);
            this.btSet4_23.Text = loadNameBt(btSet4_23);
            this.btSet4_24.Text = loadNameBt(btSet4_24);
            this.btSet4_25.Text = loadNameBt(btSet4_25);
            this.btSet4_26.Text = loadNameBt(btSet4_26);
            this.btSet4_27.Text = loadNameBt(btSet4_27);
            this.btSet4_28.Text = loadNameBt(btSet4_28);
            this.btSet4_29.Text = loadNameBt(btSet4_29);
            this.btSet4_30.Text = loadNameBt(btSet4_30);

            //tab5
            this.btSet5_1.Text = loadNameBt(btSet5_1);
            this.btSet5_2.Text = loadNameBt(btSet5_2);
            this.btSet5_3.Text = loadNameBt(btSet5_3);
            this.btSet5_4.Text = loadNameBt(btSet5_4);
            this.btSet5_5.Text = loadNameBt(btSet5_5);
            this.btSet5_6.Text = loadNameBt(btSet5_6);
            this.btSet5_7.Text = loadNameBt(btSet5_7);
            this.btSet5_8.Text = loadNameBt(btSet5_8);
            this.btSet5_9.Text = loadNameBt(btSet5_9);
            this.btSet5_10.Text = loadNameBt(btSet5_10);
            this.btSet5_11.Text = loadNameBt(btSet5_11);
            this.btSet5_12.Text = loadNameBt(btSet5_12);
            this.btSet5_13.Text = loadNameBt(btSet5_13);
            this.btSet5_14.Text = loadNameBt(btSet5_14);
            this.btSet5_15.Text = loadNameBt(btSet5_15);
            this.btSet5_16.Text = loadNameBt(btSet5_16);
            this.btSet5_17.Text = loadNameBt(btSet5_17);
            this.btSet5_18.Text = loadNameBt(btSet5_18);
            this.btSet5_19.Text = loadNameBt(btSet5_19);
            this.btSet5_20.Text = loadNameBt(btSet5_20);
            this.btSet5_21.Text = loadNameBt(btSet5_21);
            this.btSet5_22.Text = loadNameBt(btSet5_22);
            this.btSet5_23.Text = loadNameBt(btSet5_23);
            this.btSet5_24.Text = loadNameBt(btSet5_24);
            this.btSet5_25.Text = loadNameBt(btSet5_25);
            this.btSet5_26.Text = loadNameBt(btSet5_26);
            this.btSet5_27.Text = loadNameBt(btSet5_27);
            this.btSet5_28.Text = loadNameBt(btSet5_28);
            this.btSet5_29.Text = loadNameBt(btSet5_29);
            this.btSet5_30.Text = loadNameBt(btSet5_30);

            //tab6
            this.btSet6_1.Text = loadNameBt(btSet6_1);
            this.btSet6_2.Text = loadNameBt(btSet6_2);
            this.btSet6_3.Text = loadNameBt(btSet6_3);
            this.btSet6_4.Text = loadNameBt(btSet6_4);
            this.btSet6_5.Text = loadNameBt(btSet6_5);
            this.btSet6_6.Text = loadNameBt(btSet6_6);
            this.btSet6_7.Text = loadNameBt(btSet6_7);
            this.btSet6_8.Text = loadNameBt(btSet6_8);
            this.btSet6_9.Text = loadNameBt(btSet6_9);
            this.btSet6_10.Text = loadNameBt(btSet6_10);
            this.btSet6_11.Text = loadNameBt(btSet6_11);
            this.btSet6_12.Text = loadNameBt(btSet6_12);
            this.btSet6_13.Text = loadNameBt(btSet6_13);
            this.btSet6_14.Text = loadNameBt(btSet6_14);
            this.btSet6_15.Text = loadNameBt(btSet6_15);
            this.btSet6_16.Text = loadNameBt(btSet6_16);
            this.btSet6_17.Text = loadNameBt(btSet6_17);
            this.btSet6_18.Text = loadNameBt(btSet6_18);
            this.btSet6_19.Text = loadNameBt(btSet6_19);
            this.btSet6_20.Text = loadNameBt(btSet6_20);
            this.btSet6_21.Text = loadNameBt(btSet6_21);
            this.btSet6_22.Text = loadNameBt(btSet6_22);
            this.btSet6_23.Text = loadNameBt(btSet6_23);
            this.btSet6_24.Text = loadNameBt(btSet6_24);
            this.btSet6_25.Text = loadNameBt(btSet6_25);
            this.btSet6_26.Text = loadNameBt(btSet6_26);
            this.btSet6_27.Text = loadNameBt(btSet6_27);
            this.btSet6_28.Text = loadNameBt(btSet6_28);
            this.btSet6_29.Text = loadNameBt(btSet6_29);
            this.btSet6_30.Text = loadNameBt(btSet6_30);

            //tab7
            this.btSet7_1.Text = loadNameBt(btSet7_1);
            this.btSet7_2.Text = loadNameBt(btSet7_2);
            this.btSet7_3.Text = loadNameBt(btSet7_3);
            this.btSet7_4.Text = loadNameBt(btSet7_4);
            this.btSet7_5.Text = loadNameBt(btSet7_5);
            this.btSet7_6.Text = loadNameBt(btSet7_6);
            this.btSet7_7.Text = loadNameBt(btSet7_7);
            this.btSet7_8.Text = loadNameBt(btSet7_8);
            this.btSet7_9.Text = loadNameBt(btSet7_9);
            this.btSet7_10.Text = loadNameBt(btSet7_10);
            this.btSet7_11.Text = loadNameBt(btSet7_11);
            this.btSet7_12.Text = loadNameBt(btSet7_12);
            this.btSet7_13.Text = loadNameBt(btSet7_13);
            this.btSet7_14.Text = loadNameBt(btSet7_14);
            this.btSet7_15.Text = loadNameBt(btSet7_15);
            this.btSet7_16.Text = loadNameBt(btSet7_16);
            this.btSet7_17.Text = loadNameBt(btSet7_17);
            this.btSet7_18.Text = loadNameBt(btSet7_18);
            this.btSet7_19.Text = loadNameBt(btSet7_19);
            this.btSet7_20.Text = loadNameBt(btSet7_20);
            this.btSet7_21.Text = loadNameBt(btSet7_21);
            this.btSet7_22.Text = loadNameBt(btSet7_22);
            this.btSet7_23.Text = loadNameBt(btSet7_23);
            this.btSet7_24.Text = loadNameBt(btSet7_24);
            this.btSet7_25.Text = loadNameBt(btSet7_25);
            this.btSet7_26.Text = loadNameBt(btSet7_26);
            this.btSet7_27.Text = loadNameBt(btSet7_27);
            this.btSet7_28.Text = loadNameBt(btSet7_28);
            this.btSet7_29.Text = loadNameBt(btSet7_29);
            this.btSet7_30.Text = loadNameBt(btSet7_30);

            //tab8
            this.btSet8_1.Text = loadNameBt(btSet8_1);
            this.btSet8_2.Text = loadNameBt(btSet8_2);
            this.btSet8_3.Text = loadNameBt(btSet8_3);
            this.btSet8_4.Text = loadNameBt(btSet8_4);
            this.btSet8_5.Text = loadNameBt(btSet8_5);
            this.btSet8_6.Text = loadNameBt(btSet8_6);
            this.btSet8_7.Text = loadNameBt(btSet8_7);
            this.btSet8_8.Text = loadNameBt(btSet8_8);
            this.btSet8_9.Text = loadNameBt(btSet8_9);
            this.btSet8_10.Text = loadNameBt(btSet8_10);
            this.btSet8_11.Text = loadNameBt(btSet8_11);
            this.btSet8_12.Text = loadNameBt(btSet8_12);
            this.btSet8_13.Text = loadNameBt(btSet8_13);
            this.btSet8_14.Text = loadNameBt(btSet8_14);
            this.btSet8_15.Text = loadNameBt(btSet8_15);
            this.btSet8_16.Text = loadNameBt(btSet8_16);
            this.btSet8_17.Text = loadNameBt(btSet8_17);
            this.btSet8_18.Text = loadNameBt(btSet8_18);
            this.btSet8_19.Text = loadNameBt(btSet8_19);
            this.btSet8_20.Text = loadNameBt(btSet8_20);
            this.btSet8_21.Text = loadNameBt(btSet8_21);
            this.btSet8_22.Text = loadNameBt(btSet8_22);
            this.btSet8_23.Text = loadNameBt(btSet8_23);
            this.btSet8_24.Text = loadNameBt(btSet8_24);
            this.btSet8_25.Text = loadNameBt(btSet8_25);
            this.btSet8_26.Text = loadNameBt(btSet8_26);
            this.btSet8_27.Text = loadNameBt(btSet8_27);
            this.btSet8_28.Text = loadNameBt(btSet8_28);
            this.btSet8_29.Text = loadNameBt(btSet8_29);
            this.btSet8_30.Text = loadNameBt(btSet8_30);

            //tab9
            this.btSet9_1.Text = loadNameBt(btSet9_1);
            this.btSet9_2.Text = loadNameBt(btSet9_2);
            this.btSet9_3.Text = loadNameBt(btSet9_3);
            this.btSet9_4.Text = loadNameBt(btSet9_4);
            this.btSet9_5.Text = loadNameBt(btSet9_5);
            this.btSet9_6.Text = loadNameBt(btSet9_6);
            this.btSet9_7.Text = loadNameBt(btSet9_7);
            this.btSet9_8.Text = loadNameBt(btSet9_8);
            this.btSet9_9.Text = loadNameBt(btSet9_9);
            this.btSet9_10.Text = loadNameBt(btSet9_10);
            this.btSet9_11.Text = loadNameBt(btSet9_11);
            this.btSet9_12.Text = loadNameBt(btSet9_12);
            this.btSet9_13.Text = loadNameBt(btSet9_13);
            this.btSet9_14.Text = loadNameBt(btSet9_14);
            this.btSet9_15.Text = loadNameBt(btSet9_15);
            this.btSet9_16.Text = loadNameBt(btSet9_16);
            this.btSet9_17.Text = loadNameBt(btSet9_17);
            this.btSet9_18.Text = loadNameBt(btSet9_18);
            this.btSet9_19.Text = loadNameBt(btSet9_19);
            this.btSet9_20.Text = loadNameBt(btSet9_20);
            this.btSet9_21.Text = loadNameBt(btSet9_21);
            this.btSet9_22.Text = loadNameBt(btSet9_22);
            this.btSet9_23.Text = loadNameBt(btSet9_23);
            this.btSet9_24.Text = loadNameBt(btSet9_24);
            this.btSet9_25.Text = loadNameBt(btSet9_25);
            this.btSet9_26.Text = loadNameBt(btSet9_26);
            this.btSet9_27.Text = loadNameBt(btSet9_27);
            this.btSet9_28.Text = loadNameBt(btSet9_28);
            this.btSet9_29.Text = loadNameBt(btSet9_29);
            this.btSet9_30.Text = loadNameBt(btSet9_30);

            //tab10
            this.btSet10_1.Text = loadNameBt(btSet10_1);
            this.btSet10_2.Text = loadNameBt(btSet10_2);
            this.btSet10_3.Text = loadNameBt(btSet10_3);
            this.btSet10_4.Text = loadNameBt(btSet10_4);
            this.btSet10_5.Text = loadNameBt(btSet10_5);
            this.btSet10_6.Text = loadNameBt(btSet10_6);
            this.btSet10_7.Text = loadNameBt(btSet10_7);
            this.btSet10_8.Text = loadNameBt(btSet10_8);
            this.btSet10_9.Text = loadNameBt(btSet10_9);
            this.btSet10_10.Text = loadNameBt(btSet10_10);
            this.btSet10_11.Text = loadNameBt(btSet10_11);
            this.btSet10_12.Text = loadNameBt(btSet10_12);
            this.btSet10_13.Text = loadNameBt(btSet10_13);
            this.btSet10_14.Text = loadNameBt(btSet10_14);
            this.btSet10_15.Text = loadNameBt(btSet10_15);
            this.btSet10_16.Text = loadNameBt(btSet10_16);
            this.btSet10_17.Text = loadNameBt(btSet10_17);
            this.btSet10_18.Text = loadNameBt(btSet10_18);
            this.btSet10_19.Text = loadNameBt(btSet10_19);
            this.btSet10_20.Text = loadNameBt(btSet10_20);
            this.btSet10_21.Text = loadNameBt(btSet10_21);
            this.btSet10_22.Text = loadNameBt(btSet10_22);
            this.btSet10_23.Text = loadNameBt(btSet10_23);
            this.btSet10_24.Text = loadNameBt(btSet10_24);
            this.btSet10_25.Text = loadNameBt(btSet10_25);
            this.btSet10_26.Text = loadNameBt(btSet10_26);
            this.btSet10_27.Text = loadNameBt(btSet10_27);
            this.btSet10_28.Text = loadNameBt(btSet10_28);
            this.btSet10_29.Text = loadNameBt(btSet10_29);
            this.btSet10_30.Text = loadNameBt(btSet10_30);
        }


        private int JC_HookedKeyboardNofity(int iKeyWhatHappened, int vkCode)
        {

            //일단은 기본적으로 키 이벤트를 흘려보내기 위해서 0으로 세팅
            int lResult = 0;

            if (iKeyWhatHappened == 32)
            {
                switch (vkCode)
                {
                    case 49:
                        btSet_ex(this.btSet1_1);
                        lResult = 1; //핫키로 사용된 키를 윈도우로 반환하지 않음(클라이언트에서 의도치 않은 스킬 사용을 막기 위해서)
                        break;
                    case 50:
                        btSet_ex(this.btSet1_2);
                        lResult = 1;
                        break;
                    case 51:
                        btSet_ex(this.btSet1_3);
                        lResult = 1;
                        break;
                    case 52:
                        btSet_ex(this.btSet1_4);
                        lResult = 1;
                        break;
                    case 53:
                        btSet_ex(this.btSet1_5);
                        lResult = 1;
                        break;
                    case 54:
                        btSet_ex(this.btSet1_6);
                        lResult = 1;
                        break;
                    case 55:
                        btSet_ex(this.btSet1_7);
                        lResult = 1;
                        break;
                    case 56:
                        btSet_ex(this.btSet1_8);
                        lResult = 1;
                        break;
                    case 57:
                        btSet_ex(this.btSet1_9);
                        lResult = 1;
                        break;
                    case 48:
                        btSet_ex(this.btSet1_10);
                        lResult = 1;
                        break;
                    case 0x5A: //Alt+Z
                        btHidden_Click(null, null);
                        lResult = 1;
                        break;
                    case 0xC0: //Alt+`
                        this.WindowState = FormWindowState.Normal; //툴 활성화
                        this.cbCommand.Focus(); //로드시마다 명령어 입력창에 포커스
                        lResult = 1;
                        break;
                    case 0x54: //Alt+T
                        btGetTarget_Click(null, null); //현재 타겟팅중인 대상의 이름을 불러옴
                        lResult = 1;
                        break;
                    case 0x43: //Alt+C
                        btConn_Click(null, null); //클라이언트와 연결/해제
                        lResult = 1;
                        break;
                    case 0x49: //Alt+I
                        btDoitDoit_Click(null, null); //또해또해 호출 및 해제
                        lResult = 1;
                        break;
                    case 0x50: //Alt+P 
                        doitdoit.btStart_Click(null, null); //또해또해 한번에 실행
                        lResult = 1;
                        break;
                    case 0x58: //Alt+X
                        if (doitdoit.btNext.Enabled == true) //또해또해 하나씩 실행
                            doitdoit.btNext_Click(null, null);
                        lResult = 1;
                        break;
                }
            }
            else
            {
                //나머지 키들은 얌전히 보내준다.
                lResult = 0;
            }
            return lResult;
        }

        // 여기서 부터 툴팁으로 단축키 안내
        private void btHidden_MouseHover(object sender, EventArgs e)
        {
            ttTooltip.Show("Alt + Z", (Button)sender);
        }

        private void cbCommand_MouseHover(object sender, EventArgs e)
        {
            ttTooltip.Show("Alt + `", (ComboBox)sender);
        }

        private void btGetTarget_MouseHover(object sender, EventArgs e)
        {
            ttTooltip.Show("Alt + T", (Button)sender);
        }
        
        private void btConn_MouseHover(object sender, EventArgs e)
        {
            ttTooltip.Show("Alt + C", (Button)sender);
        }

        private void cbDevNameItems_MouseHover(object sender, EventArgs e)
        {
            ttTooltip.Show("체크 시, 콤보박스의 리스트를 개발명으로 교체합니다.", cbDevNameItems);
        }

        private void btDoitDoit_MouseHover(object sender, EventArgs e)
        {
            ttTooltip.Show("Alt + I", (Button)sender);
        }


        //


    }
}
