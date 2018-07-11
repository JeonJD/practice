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

namespace JC
{
    public partial class frmDoItDoIt : Form
    {
        public NetworkStream doitStream = null; //네트워크 스트림
        bool flagPlayStop = false; //수행&정지 플래그
        bool flagTextChanged = true; //한글 입력 제한을 위한 플래그
        private Thread doItExec; //명령 실행 쓰레드
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

        public void btExit_Click(object sender, EventArgs e) //종료버튼
        {
            this.Hide();
        }

        private void frmDoItDoIt_Load(object sender, EventArgs e)
        {
            //btConn_Click(sender, e); //연결 버튼 자동실행
            //CheckForIllegalCrossThreadCalls = false; //크로스 스레드 무시
            //후크 이벤트를 연결
            
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



        private void getItemfromListBox(string operateType, string selectedItem, int selectedIndex)
        {
            selectedItem = lstItemsList.SelectedItem.ToString();
            selectedIndex = lstItemsList.SelectedIndex++;
            
        }

        public void btStart_Click(object sender, EventArgs e)
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

        public void btNext_Click(object sender, EventArgs e)
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

                doitStream.Write(packetHeaderSize, 0, 2);
                doitStream.Write(packetHeaderType, 0, 2);
                doitStream.Write(packetBody, 0, bodySize - 2); //명령어 타입 2바이트를 제외
                doitStream.Flush();

                //디버그용
                //Invoke(AddText, BitConverter.ToString(packetHeaderSize) + "-" + BitConverter.ToString(packetHeaderType) + "-" + BitConverter.ToString(packetBody)); 

                //var dt = Convert.ToString(DateTime.Now);
                //jcWrite.WriteLine(msgLine);
                //jcWrite.Flush();

                //Invoke(AddText, msgLine + ".." + dt);
            }
            catch (Exception ex)
            {
                //Invoke(AddText, "클라이언트에 명령을 전달할 수 없습니다.");
                //disconnect();
                MessageBox.Show(ex.Message);
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
