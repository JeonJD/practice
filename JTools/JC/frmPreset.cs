using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC
{
    public partial class frmPreset : Form
    {
        public string cmdNameValue = null;
        public bool toggleValue = false;
        public bool argumentValue = false;
        public bool targetValue = false;
        public bool confirmationValue = false;
        public string command_1Value = null;
        public string command_2Value = null;

        public frmPreset()
        {
            InitializeComponent();
        }

        private void frmPreset_Load(object sender, EventArgs e)
        {
            this.tbCommandName.Text = cmdNameValue;
            this.cbToggle.Checked = toggleValue;
            this.cbArgument.Checked = argumentValue;
            this.cbTarget.Checked = targetValue;
            this.cbConfirmation.Checked = confirmationValue;
            this.tbCommand_1.Text = command_1Value;
            this.tbCommand_2.Text = command_2Value;


        }

        private void cbToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (cbToggle.Checked)
            {
                lbCommand_2.Visible = true;
                tbCommand_2.Visible = true;
            }
            else
            {
                lbCommand_2.Visible = false;
                tbCommand_2.Visible = false;
                this.Size = new Size(this.Size.Width, 300);
                this.Size = new Size(this.Size.Height, 450);
            }
        }



        private void btSubmit_Click(object sender, EventArgs e)
        {

            if (
                    tbCommandName.Text == "" &&
                    cbToggle.Checked == false &&
                    cbArgument.Checked == false &&
                    cbTarget.Checked == false &&
                    cbConfirmation.Checked == false &&
                    tbCommand_1.Text == ""
                ) //컨트롤에 값이 없을 경우 프리셋 초기화 진행
            {
                btReset_Click(sender, e); //모든 컨트롤 및 값 초기화
                cmdNameValue = null;
                toggleValue = false;
                argumentValue = false;
                targetValue = false;
                confirmationValue = false;
                command_1Value = null;
                command_2Value = null;


                this.Close();
            }
/*
            else if (tbCommandName.Text == "")
                MessageBox.Show("'명령 이름'창은 비워둘 수 없습니다.");
*/
            else if (tbCommand_1.Text == "")
                MessageBox.Show("'실행 명령어'창은 비워둘 수 없습니다.");
            else if (cbToggle.Checked && tbCommand_2.Text == "")
                MessageBox.Show("'토글 해제 명령어'창은 비워둘 수 없습니다.");
            else
            {
                if (tbCommandName.Text != "")
                    cmdNameValue = tbCommandName.Text;
                else
                {
                    //명령 이름을 입력하지 않으면 명령어 라인을 그대로 사용
                    if (tbCommand_1.Text.Length > 100)
                        cmdNameValue = tbCommand_1.Text.Substring(0, 100);
                    else
                        cmdNameValue = tbCommand_1.Text;
                }

                toggleValue = cbToggle.Checked;
                argumentValue = cbArgument.Checked;
                confirmationValue = cbConfirmation.Checked;
                targetValue = cbTarget.Checked;

                if (tbCommand_1.Text != "")
                    command_1Value = tbCommand_1.Text;
                else
                    command_1Value = null;

                if (tbCommand_2.Text != "" && toggleValue) //토글이 아닐경우 command2는 무조건 널로 초기화
                    command_2Value = tbCommand_2.Text;
                else
                    command_2Value = null;

                this.Close();


            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btReset_Click(object sender, EventArgs e) //모든 컨트롤 초기화
        {
            this.tbCommandName.Text = null;
            this.cbToggle.Checked = false;
            this.cbArgument.Checked = false;
            this.cbTarget.Checked = false;
            this.cbConfirmation.Checked = false;
            this.tbCommand_1.Text = null;
            this.tbCommand_2.Text = null;

        }

        private void frmPreset_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                //this.Visible = false;
            }
        }

        private void tbSelectAll(object sender, KeyEventArgs e)
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
