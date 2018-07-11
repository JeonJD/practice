using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace JA
{
    public partial class frmJA : Form
    {
        public frmJA()
        {
            InitializeComponent();

        }

        private void btExecute_Click(object sender, EventArgs e)
        {
            try
            {
                string dbName = null;
                string dbServerIP = null;
                string accountName = tbAccount.Text;

                
                switch (lbServer.SelectedIndex)
                {
                    case 0: //QA1
                        dbName = "x3main";
                        dbServerIP = "192.168.13.178,1433";
                        break;
                    case 1: //QA1_Branch
                        dbName = "x3branch_main";
                        dbServerIP = "192.168.13.178,1433";
                        break;
                    case 2: //QA2
                        dbName = "x3main_trunk";
                        dbServerIP = "192.168.13.188,1433";
                        break;
                    case 3: //QA2_QA_GrandOpen
                        dbName = "x3main_qa_grandopen";
                        dbServerIP = "192.168.13.188,1433";
                        break;
                    case 4: //QA2_QA_OBT
                        dbName = "x3main_qa_obt";
                        dbServerIP = "192.168.13.188,1433";
                        break;
                    default:
                        break;                     
                }


                
                if (tbAccount.Text == "")
                {
                    MessageBox.Show("계정명을 입력해주세요.");
                }
                else if(lbServer.SelectedIndex < 0)
                {
                    MessageBox.Show("서버를 선택해주세요.");
                }
                    else
                {
                    string connectionString =
                        "SERVER=" + dbServerIP + ";" +
                        "UID=x3db;PWD=x3db123.";
                    string commandString =
                        "delete " +
                        "from " + dbName + ".dbo.account_achievements " +
                        "Where account_id = " +
                        "(" +
                        "select id " +
                        "from " + dbName + ".dbo.accounts " +
                        "where account_name = '" + accountName + "' " +
                        ")";



                    SqlConnection sconDbms = new SqlConnection(connectionString);
                    SqlCommand scomSQL = new SqlCommand();
                    scomSQL.Connection = sconDbms;
                    scomSQL.CommandText = commandString;

                    sconDbms.Open();
                    SqlDataReader sdr = scomSQL.ExecuteReader();

                    sdr.Close();
                    sconDbms.Close();
                    MessageBox.Show(tbAccount.Text + "의 업적이 모두 파괴되었습니다.");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
