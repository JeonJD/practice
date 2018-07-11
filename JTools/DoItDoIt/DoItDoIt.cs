using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; //DllImport
using System.Net.Sockets; //TcpListener 클래스사용
using System.Threading; //스레드 클래스 사용

namespace DoItDoIt
{
    public partial class frmDoItDoIt : Form
    {
        Int16 Port_slot1 = 9111; //기본 연결 포트;
        bool ClientCon = false; //연결&해제 플래그
        bool flagPlayStop = false; //수행&정지 플래그
        bool flagTextChanged = true; //한글 입력 제한을 위한 플래그
        private TcpClient tcClient; //TCP 네트워크 서비스에 대한 클라이언트 연결 제공
        private NetworkStream jcStream; //네트워크 스트림
        private Thread slot1_Reader; //TCP 연결 스레드
        private Thread doItExec; //명령 실행 쓰레드
        private const string addr = "127.0.0.1"; //루프백

        event KeyboardHooker.HookedKeyboardUserEventHandler HookedKeyboardNofity; //후킹이벤트
                
       
        //아무곳이나 클릭하여 이동을 위해 DLL Import
        [DllImport("user32.dll")]
        public static extern int  SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public readonly int WM_NLBUTTONDOWN = 0xA1;
        public readonly int HT_CAPTION = 0x2;
        //
        protected override void OnMouseDown(MouseEventArgs e) // 메인폼의 아무곳이나 클릭하여 이동
        {

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();// 다른 컨트롤에 묶여있을 수 있을 수 있으므로 마우스캡쳐 해제
                SendMessage(this.Handle, WM_NLBUTTONDOWN, HT_CAPTION, 0);// 타이틀 바의 다운 이벤트처럼 보냄
            }
            base.OnMouseDown(e);


        }
        public frmDoItDoIt()
        {
            InitializeComponent();
        }

        private void tbOpi_Scroll(object sender, EventArgs e) //폼투명도 조절
        {
            this.Opacity = (double)tbOpi.Value / 10;
        }

        private void btExit_Click(object sender, EventArgs e) //종료버튼
        {
            this.Close();
        }

        private void btHidden_Click(object sender, EventArgs e) //최소화 버튼
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

        private void frmDoItDoIt_Load(object sender, EventArgs e)
        {
            btConn_Click(sender, e); //연결 버튼 자동실행
            CheckForIllegalCrossThreadCalls = false; //크로스 스레드 무시
            //후크 이벤트를 연결
            HookedKeyboardNofity += new KeyboardHooker.HookedKeyboardUserEventHandler(JC_HookedKeyboardNofity);
            //자동으로 훅을 시작, 훅에 대한 이벤트 연결
            KeyboardHooker.Hook(HookedKeyboardNofity);

        }

        private int JC_HookedKeyboardNofity(int iKeyWhatHappened, int vkCode)
        {

            //일단은 기본적으로 키 이벤트를 흘려보내기 위해서 0으로 세팅
            int lResult = 0;

            if (iKeyWhatHappened == 32)
            {
                switch (vkCode)
                {
                    
                    case 0x5A: //Alt+Z
                        btHidden_Click(null, null);
                        lResult = 1;
                        break;
                    case 0x50: //Alt+P
                        btStart_Click(null, null);
                        lResult = 1;
                        break;
                    case 0x58: //Alt+X
                        if (btNext.Enabled == true)
                            btNext_Click(null, null);
                        lResult = 1;
                        break;
                }
            }
            
            return lResult;
        }

        private void lstItemsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        for (int selectedIndex = 0; selectedIndex < ((ListBox)sender).Items.Count; selectedIndex++)
                        {
                            ((ListBox)sender).SetSelected(selectedIndex, true);
                        }
                        break;
                    case Keys.C:
                        object[] selectedItemList = new string [((ListBox)sender).SelectedItems.Count];
                        string resultItemList = "";
                        ((ListBox)sender).SelectedItems.CopyTo(selectedItemList, 0);
                        foreach (object selectedItem in selectedItemList)
                        {
                            resultItemList += selectedItem.ToString().Trim() +Environment.NewLine;
                        }
                        Clipboard.SetText(resultItemList.Trim());
                        break;
                }


            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        ((ListBox)sender).ClearSelected();
                        break;
                    case Keys.Delete:
                        btDeleteITems_Click(null, null);
                        break;
                }
                
            }
        }

        private void btAddITems_Click(object sender, EventArgs e)
        {
            string[] itemList = (tbInputItems.Text).Split('\r');
            lstItemsList.Items.AddRange(itemList);
            this.tbInputItems.Clear();

        }

        private void btConn_Click(object sender, EventArgs e)
        {
            
            this.btConn.Enabled = false; //이벤트 처리 동안 버튼 비활성화

            if (!(this.ClientCon))
            {
                this.Port_slot1 = Convert.ToInt16(tbPort.Text);
                ClientConnection(); //클라이언트 연결 함수 호출

            }
            else
            {
                disconnect(); //클라이언트 종료 함수 호출
            }
            this.btConn.Enabled = true;
        }

        private void ClientConnection() //클라이언트 연결
        {

            try
            {
                tbPort.Enabled = false; //연결 중인 동안 포트 수정 불가
                tcClient = new TcpClient(addr, Port_slot1);
                jcStream = tcClient.GetStream();

                //Invoke(AddText, "클라이언트에 연결되었습니다.");
                //Invoke(AddConn, "해제");
                buttonView("해제");
                this.ClientCon = true;

                slot1_Reader = new Thread(msgReceive);
                slot1_Reader.Start();

            }
            catch
            {
                this.ClientCon = false;
                //Invoke(AddText, "클라이언트와 연결하지 못했습니다.");
                //Invoke(AddConn, "연결");
                //Invoke(AddPort, "true"); //포트 입력창 활성화
                buttonView("연결");
                tbPort.Enabled = true;


            }
        }

        private void getItemfromListBox(string operateType, string selectedItem, int selectedIndex)
        {
            selectedItem = lstItemsList.SelectedItem.ToString();
            selectedIndex = lstItemsList.SelectedIndex++;
            
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            
            if (flagPlayStop || this.btReset.Enabled == false)
            {
                flagPlayStop = false;
                changeControlStoped();
                doItExec.Abort();
                
            }
            else
            {
                int selectedIndex;
                if (lstItemsList.Items.Count > 0)
                {
                    flagPlayStop = true;
                    changeControlPlayed();

                    if (lstItemsList.SelectedItem == null) //선택이 없을 경우 첫번째 아이템 선택
                    {
                        lstItemsList.SelectedIndex = 0;
                    }

                    if (lstItemsList.SelectedItems.Count > 0) //여러개가 선택되어 있을 경우 하나만 남기고 모두 선택 해제
                    {
                        selectedIndex = lstItemsList.SelectedIndex;
                        lstItemsList.ClearSelected();
                        lstItemsList.SetSelected(selectedIndex, true);

                    }

                    doItExec = new Thread(execCommand);
                    doItExec.Start();
                }
                else
                {
                    MessageBox.Show("목록이 비어있습니다.");
                }
            }
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            int selectedIndex;
            if (lstItemsList.Items.Count > 0)
            {
                changeControlPlayed();
                if (lstItemsList.SelectedItem == null) //선택이 없을 경우 첫번째 아이템 선택
                {
                    lstItemsList.SelectedIndex = 0;
                }

                if (lstItemsList.SelectedItems.Count > 0) //여러개가 선택되어 있을 경우 하나만 남기고 모두 선택 해제
                {
                    selectedIndex = lstItemsList.SelectedIndex;
                    lstItemsList.ClearSelected();
                    lstItemsList.SetSelected(selectedIndex, true);

                }

                doItExec = new Thread(execCommand);
                doItExec.Start();
            }
            else
            {
                MessageBox.Show("목록이 비어있습니다.");
            }

        }

        private void execCommand()
        {

            string[] commandLine = tbExCommand.Text.Split('\r');
            string[] subCommandLine = tbExSubCommand.Text.Split('\r');
            string selectedItem;
            int selectedIndex;
            int dTime;

            
            do
            {
                
                selectedItem = lstItemsList.SelectedItem.ToString();
                selectedIndex = lstItemsList.SelectedIndex;
                
                foreach (string command in commandLine)
                {
                    if (selectedItem.Trim() != "" && command.Trim() != "") //빈칸일때는 실행하지 않음
                    {
                        try
                        {
                            dTime = int.Parse(command);// 정수만 입력된 행은 해당 수치만큼 쓰레드 슬립 
                            Thread.Sleep(dTime);
                        }
                        catch //정수로 파싱할 수 없는 행은 명령어로 간주하여 바로 명령 전송
                        {
                            msgSend(command.Replace("?", selectedItem.Trim()), 0);
                        }
                    }
                }
                if (cbSubTurn.Checked)
                {
                    if (tbSubTurn.Text.Equals(""))
                    {
                        tbSubTurn.Text = "1";//체크되었지만 비어있을 경우 1으로 수정
                    }

                    if (((selectedIndex + 1) % (int.Parse(tbSubTurn.Text))) == 0)
                    {

                        foreach (string command in subCommandLine)
                        {
                            if (command.Trim() != "") //빈칸일때는 실행하지 않음
                            {

                                try
                                {
                                    dTime = int.Parse(command);// 정수만 입력된 행은 해당 수치만큼 쓰레드 슬립 
                                    Thread.Sleep(dTime);
                                }
                                catch //정수로 파싱할 수 없는 행은 명령어로 간주하여 바로 명령 전송
                                {
                                    msgSend(command.Replace("?", selectedItem.Trim()), 0);
                                }
                            }
                        }


                    }
                }

                lstItemsList.SetSelected(selectedIndex, false);
                if (++selectedIndex >= lstItemsList.Items.Count)
                {
                    flagPlayStop = false;
                }
                else
                {
                    lstItemsList.SetSelected(selectedIndex, true);
                }

            } while(flagPlayStop);
            changeControlStoped();
        }

        private void changeControlPlayed() //수행중일 때, 컨트롤 변경
        {
            this.btStart.Text = "그만 또 해" + Environment.NewLine + "[Alt + P]";
            btNext.Enabled = false;
            lstItemsList.Enabled = false;
            btAddITems.Enabled = false;
            btDeleteITems.Enabled = false;
            btReset.Enabled = false;
            tbInputItems.Enabled = false;
            tbExCommand.Enabled = false;
            tbExSubCommand.Enabled = false;
            tbSubTurn.Enabled = false;
            cbSubTurn.Enabled = false;

            
        }
        private void changeControlStoped() //수행중이지 않을 때, 컨트롤 변경
        {
            this.btStart.Text = "끝까지 또 해" + Environment.NewLine + "[Alt + P]";
            btNext.Enabled = true;
            lstItemsList.Enabled = true;
            btAddITems.Enabled = true;
            btDeleteITems.Enabled = true;
            btReset.Enabled = true;
            tbInputItems.Enabled = true;
            tbExCommand.Enabled = true;
            tbExSubCommand.Enabled = true;
            tbSubTurn.Enabled = true;
            cbSubTurn.Enabled = true;

        }
        private void msgSend(String msgLine, UInt16 msgLineType) //메시지 전송
        {
            try
            {

                byte[] packetBody;
                byte[] packetHeaderSize = new byte[2];
                byte[] packetHeaderType = new byte[2];
                UInt16 bodySize, operType;

                packetBody = Encoding.UTF8.GetBytes(msgLine);
                bodySize = (UInt16)(packetBody.Length + 2);
                operType = msgLineType; //명령어타입이 0일경우 무시, 1일경우 예외처리함
                packetHeaderSize = BitConverter.GetBytes(bodySize);
                packetHeaderType = BitConverter.GetBytes(operType);
                //Invoke(AddText, bodySize.ToString());
                //Invoke(AddText, msgLine.Length.ToString());

                jcStream.Write(packetHeaderSize, 0, 2);
                jcStream.Write(packetHeaderType, 0, 2);
                jcStream.Write(packetBody, 0, bodySize - 2); //명령어 타입 2바이트를 제외
                jcStream.Flush();

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
                        msg = (byte)jcStream.ReadByte(); ////명령어타입이 0일경우 무시, 1일경우 예외처리함
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
/*
                    switch (cmdType)
                    {
                        case 0: //명령어타입이 0일경우 수신결과를 로그출력
                            Invoke(AddText, receiveCommand);
                            receiveCommand = null;
                            break;

                        case 1: //1일경우 예외처리
                            exceptedCommand(receiveCommand);
                            receiveCommand = null;
                            break;

                    }
*/
                }
                disconnect();
            }
            catch
            {
                //Invoke(AddText, "메시지를 수신하는 과정에서 오류가 발생하였습니다.");
                disconnect();
            }
        }

        private void disconnect() //연결 종료시마다 호출
        {
            if (this.ClientCon != false)
            {
                this.ClientCon = false;
                //Invoke(AddConn, "연결");
                //Invoke(AddText, "클라이언트와 연결이 종료되었습니다.");
                //Invoke(AddPort, "true"); //포트 입력창 활성화
                buttonView("연결");
                tbPort.Enabled = true;


            }


            try
            {

                if (!(jcStream == null))
                {
                    jcStream.Close(); //NetworkStream 클래스 개체 리소스 해제
                }
                if (!(tcClient == null))
                {
                    tcClient.Close(); //TcpClient 클래스 개체 리소스 해제
                }
                if (!(slot1_Reader == null))
                {
                    this.slot1_Reader.Abort(); //통신 스레드 종료
                }
                if (!(doItExec == null))
                {
                    this.doItExec.Abort(); //명령 스레드 종료
                }
            }
            catch { }
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

        private void btDeleteITems_Click(object sender, EventArgs e)
        {
            if (lstItemsList.SelectedItems.Count > 0)
            {
                for (int i = lstItemsList.SelectedItems.Count; i > 0; i--)
                {
                    lstItemsList.Items.RemoveAt(lstItemsList.SelectedIndices[i - 1]);
                }
            }
        }

        private void frmDoItDoIt_FormClosing(object sender, FormClosingEventArgs e)
        {
            disconnect();
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            lstItemsList.Items.Clear();
            tbInputItems.Clear();

        }

        private void tbInputItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void tbPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void tbSubTurn_TextChanged(object sender, EventArgs e)
        {
            
            if (flagTextChanged)
            {
                flagTextChanged = false;
                try
                {
                    if (((TextBox)sender).Text != "")
                    {
                        int.Parse(((TextBox)sender).Text);
                    }

                }
                catch
                {
                    MessageBox.Show("정수만 입력 가능합니다.");
                    ((TextBox)sender).Clear();
                }
                flagTextChanged = true;
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

        private void cbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbAlwaysOnTop.Checked)
                this.TopMost = true;
            else
                this.TopMost = false;
        }

        private void tbExCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        private void tbExSubCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    ((TextBox)sender).SelectAll();
                }
            }
        }

        





    
    }
}
