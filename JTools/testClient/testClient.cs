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
using System.Net; //IPAddress
using System.Net.Sockets; //TcpListener 클래스사용
using System.Threading; //스레드 클래스 사용
using System.IO; //파일 클래스 사용

namespace testClient
{
    public partial class testClient : Form
    {
        private TcpListener tcServerListener; //CP 네트워크 클라이언트에서 연결 수신
        private TcpClient tcClient; //TCP 네트워크 서비스에 대한 클라이언트 연결 제공
        private NetworkStream jcStream; //네트워크 스트림
        private Boolean serverStarted = false; //서버 실행 여부
        private Boolean ClientCon = false; //클라이언트 접속 여부
        private int Port_slot1 = 9111; //포트
        private Thread trdReader; //스레드
        private delegate void AddTextDelegate(string strText); //임시 콘솔 출력에 사용할 델리게이트
        private AddTextDelegate AddText = null; 


        public testClient()
        {
            InitializeComponent();
        }

        private void testClient_Load(object sender, EventArgs e)
        {
            AddText = new AddTextDelegate(MessageView);//임시 콘솔 출력을 위한 델리게이트

            if (!(this.serverStarted))
                {
                    try
                    {
                        serverStarted = true;
                        tcServerListener = new TcpListener(this.Port_slot1);
                        tcServerListener.Start();

                        trdReader = new Thread(serverStart);
                        trdReader.Start();

                    }
                    catch
                    {
                        Invoke(AddText, "클라이언트 연결대기모드 실행 실패.");
                    }
                }
                else
                {
                    disconnect(); 
                }
          
        }

        private void serverStart()
        {

            Invoke(AddText, "CH1 연결대기중..");
            
            try
            {
                while (serverStarted)
                {
                    tcClient = tcServerListener.AcceptTcpClient();
                    Invoke(AddText, "클라이언트 접속..\n");
                    jcStream = tcClient.GetStream();

                    this.ClientCon = true;
                    Receive();
                }
            }
            catch 
            {
                //Invoke(AddText, "클라이언트 접속 중 실패 발생");
            }
            
            
        }
        private void MessageView(string strText)//임시 콘솔창에 메시지 출력
        {
            this.tbDebug.AppendText(strText + "\n");
        }

        private void btConn_Click(object sender, EventArgs e) //연결 버튼 클릭 이벤트 핸들러
        {
            if (!(this.ClientCon))
            {
//                clientConnection(); //ClientConnection() 함수 호출
            }
            else
            {
                //disconnect(); //함수 호출
            }
        }

        private void btDiscon_Click(object sender, EventArgs e) //연결 해제 버튼 클릭 이벤트 핸들러
        {
            Invoke(AddText, "연결 종료");
           

            //MessageView("연결 종료");
            disconnect();
            testClient_Load(sender, e);
        }
/*
        private void clientConnection() //JC에 연결
        {
            try
            {
                tcClient = new TcpClient("127.0.0.1", Port_slot1); 
                Invoke(AddText, "연결.");
                //MessageView("연결");
                jcStream = tcClient.GetStream();

                jcRead = new StreamReader(jcStream);
                jcWrite = new StreamWriter(jcStream);
                this.ClientCon = true;
                                
                trdReader = new Thread(Receive);
                trdReader.Start();
            }
            catch
            {
                this.ClientCon = false;
                Invoke(AddText, "연결 실패.");
                //MessageView("연결 실패.");
            }
        }
*/
        private void disconnect() //JC에 연결해제
        {
            this.ClientCon = false;
            this.serverStarted = false;
            
            
            try
            {
                if (!(tcServerListener == null))
                {
                    tcServerListener.Stop(); // TCP리스너 종료
                }

                if (!(jcStream == null))
                {
                    jcStream.Close(); //NetworkStream 클래스 개체 리소스 해제
                }
                if (!(tcClient == null))
                {
                    tcClient.Close(); //TcpClient 클래스 개체 리소스 해제
                }
                if (!(trdReader == null))
                {
                    trdReader.Abort();
                }
            }
            catch
            {
                return;
            }
        }

        private void Receive() //JC로부터 메시지 수신에 대한 처리
        {
            try
            {
                byte msg = new byte();
                byte[] packetBody = new byte[2000];
                byte[] packetHeaderSize = new byte[2];
                byte[] packetHeaderType = new byte[2];
                UInt16 bodySize, operType;
                string temp = null;

                while (this.ClientCon)
                {
                    //Invoke(AddText, jcStream.ReadByte().ToString());
                    //var msg = jcRead.ReadLine();


                    for (int index = 0; index < 2; index++)
                    {
                        msg = (byte)jcStream.ReadByte();

                        if (msg < 0)
                        {
                            this.ClientCon = false;
                            Invoke(AddText, "클라이언트와 연결이 종료됨");
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
                        msg = (byte)jcStream.ReadByte(); //명령어 타입을 2바이트 스킵, 이에 대한 처리는 당분간 따로 하지 않음
                    }

                    for (int index = 0; index < bodySize - 2; index++)
                    {
                        msg = (byte)jcStream.ReadByte();

                        if (msg <= 0)
                            packetBody[index] = (byte)' ';

                        else
                            packetBody[index] = msg;

                    }
                    temp = Encoding.UTF8.GetString(packetBody, 0, bodySize - 2);

                    //for (int index = 0; index < bodySize - 2; index++)
                    //    temp = temp + (char)packetBody[index];

                    Invoke(AddText, temp);
                    Msg_send(temp);
                    temp = null;


                }
            }
            catch
            {
                Invoke(AddText, "메시지 수신 에러");
            }
                                                                                            
        }



        private void Msg_send(String msgLine) // JC로 메세지 보내기
        {
            try
            {

                byte[] packetBody;
                byte[] packetHeaderSize = new byte[2];
                byte[] packetHeaderType = new byte[2];
                UInt16 bodySize, operType;

                packetBody = Encoding.UTF8.GetBytes(msgLine);
                bodySize = (UInt16)(packetBody.Length + 2);
                operType = 0;
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
                Invoke(AddText, "클라이언트에 명령을 전달할 수 없음");
            }
        }

        private void testClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.serverStarted = false;
            disconnect(); 
        }



    }
}
