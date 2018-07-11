using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Newtonsoft.Json.Converters;

namespace JS
{
    public partial class Form_JSearch : Form
    {
        
        

        Thread tdSearchKeyword;
        private delegate void changeContorl(object control, string controlType, string text);
        private changeContorl ccChangeText, ccChangeEnable, ccChangePBStep, ccAddItem, ccViewDataGrid, ccMessageBoxShow;
        struct structControlValue
        {
            public string tableName;
            public string keyword;
            public bool includeString;
            public int language;
            public string[] tableList;
        }
        structControlValue controlValue;


        public Form_JSearch()
        {
            InitializeComponent();
        }

        private void Form_JSearch_Load(object sender, EventArgs e)
        {
            ccChangeText = new changeContorl(changeText);
            ccChangeEnable = new changeContorl(changeEnable);
            ccChangePBStep = new changeContorl(changePBStep);
            ccAddItem = new changeContorl(addItem);
            ccViewDataGrid = new changeContorl(viewDataGrid);
            ccMessageBoxShow = new changeContorl(messageBoxShow);
            
            try
            {
                this.tb_JsonPath.Text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JS\path.cfg");
            }

            catch (Exception ex)
            {
                //File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JS\path.cfg", Directory.GetCurrentDirectory());
                DirectoryInfo pathPreset = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JS");

                if (!(pathPreset.Exists))
                    pathPreset.Create();

                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JS\path.cfg", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                this.tb_JsonPath.Text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JS\path.cfg");

                MessageBox.Show("JSON이 위치한 폴더의 경로를 설정해주세요.");
            }
            getFileNamelist();

        }

        private void changeText(object control, string controlType, string text)
        {
            switch(controlType)
            {
                case "Button":
                    ((Button)control).Text = text;
                    break;
                case "ComboBox":
                    ((ComboBox)control).Text = text;
                    break;

            }

        }

        private void changeEnable(object control, string controlType, string text)
        {
            switch (controlType)
            {
                case "Button":
                    ((Button)control).Enabled = bool.Parse(text);
                    ((Button)control).Visible = bool.Parse(text);
                    break;
                case "ComboBox":
                    ((ComboBox)control).Enabled = bool.Parse(text);
                    ((ComboBox)control).Visible = bool.Parse(text);
                    break;

            }

        }

        private void changePBStep(object control, string controlType, string text)
        {
            switch (text)
            {
                case null:
                    ((ProgressBar)control).PerformStep();
                    break;
                default:
                    ((ProgressBar)control).Value = int.Parse(text);
                    break;
            }
                   
        }
        
        private void addItem(object control, string controlType, string text)
        {
            ((ListBox)control).Items.Add(text);
        }

        private void viewDataGrid(object control, string controlType, string text)
        {
            dgv_result.DataSource = (DataTable)control;
            dgv_result.AutoResizeColumns();
        }

        private void messageBoxShow(object control, string controlType, string text)
        {

            MessageBox.Show(this.bt_search, text);
            
        }




        private void bt_setPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.SelectedPath = tb_JsonPath.Text;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.tb_JsonPath.Text = ofd.SelectedPath;
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\JS\path.cfg", ofd.SelectedPath.ToString());
                    this.cbSession.Items.Clear();
                    getFileNamelist();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folder Open Error :\n" + ex.Message);
            }
        }

        private void getFileNamelist()
        {
            //cbSession.Items.Clear();
            cb_Tablelist.Items.Clear();
            cb_Tablelist.Items.Add("ALL");

            try
            {
                

                DirectoryInfo di = new DirectoryInfo(this.tb_JsonPath.Text);
                DirectoryInfo di_strng_K = new DirectoryInfo(this.tb_JsonPath.Text + @"\strings_korean");
                DirectoryInfo di_strng_E = new DirectoryInfo(this.tb_JsonPath.Text + @"\strings_english");

                if (di.Exists)
                {
                    DirectoryInfo[] diSessionList = di.GetDirectories();

                    foreach (DirectoryInfo diSession in diSessionList)
                    {
                        if (!diSession.Name.Contains("strings") && !diSession.Name.Equals("csv") && !diSession.Name.Equals("xml"))
                        {
                            if (!cbSession.Items.Contains(diSession.Name))
                            {
                                cbSession.Items.Add(diSession.Name);
                            }

                        }
                    }

                    foreach (FileInfo fi in di.GetFiles())
                    {

                        if (fi.Extension.ToLower().Contains("json"))
                        {
                            cb_Tablelist.Items.Add((String)fi.Name.Substring(0, fi.Name.Length - 5));
                        }
                    }

                    if (this.cbSession.SelectedIndex < 0)
                    {
                        this.cbSession.SelectedIndex = 0;
                    }

                    DirectoryInfo diSelectedSession = new DirectoryInfo(this.tb_JsonPath.Text + @"\" + this.cbSession.SelectedItem.ToString());

                    foreach (FileInfo fi in diSelectedSession.GetFiles())
                    {
                        if (fi.Extension.ToLower().Contains("json"))
                            cb_Tablelist.Items.Add(@"\" + this.cbSession.SelectedItem.ToString() + @"\" + (String)fi.Name.Substring(0, fi.Name.Length - 5));
                    }

                    if (this.cb_string.Checked == true)
                    {
                        foreach (FileInfo fi in di_strng_K.GetFiles())
                        {
                            if (fi.Extension.ToLower().Contains("json"))
                                //cb_Tablelist.Items.Add((String)fi.Name.Substring(4, fi.Name.Length - (4 + 5)));
                                cb_Tablelist.Items.Add(@"\strings_korean\" + (String)fi.Name.Substring(0, fi.Name.Length - 5));
                        }
                        foreach (FileInfo fi in di_strng_E.GetFiles())
                        {
                            if (fi.Extension.ToLower().Contains("json"))
                                //cb_Tablelist.Items.Add((String)fi.Name.Substring(4, fi.Name.Length - (4 + 5)));
                                cb_Tablelist.Items.Add(@"\strings_english\" + (String)fi.Name.Substring(0, fi.Name.Length - 5));
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("DB폴더의 경로를 다시 입력해 주세요\n" + ex.Message);
            }
        }



        private void bt_search_Click(object sender, EventArgs e)
        {            
            if (bt_search.Text.Equals("Search"))
            {
                bt_search.Text = "Stop";
                pb_Searching.Value = 0;
                if (tdSearchKeyword != null)
                    tdSearchKeyword.Abort();
                tdSearchKeyword = new Thread(new ThreadStart(searchKeyword));
                this.lb_result.Items.Clear();
                this.dgv_result.DataSource = null;
                this.bt_searchAll.Hide();
                controlValue.tableName = cb_Tablelist.Text;
                controlValue.keyword = tb_search.Text;
/*
                if (this.rb_Korean.Checked)
                    controlValue.language = 1;
                else if (this.rb_English.Checked)
                    controlValue.language = 2;
                else
*/
                    controlValue.language = 0;

                controlValue.includeString = cb_string.Checked;
                controlValue.tableList = new string[cb_Tablelist.Items.Count];
                cb_Tablelist.Items.CopyTo(controlValue.tableList, 0);
                this.pb_Searching.Step = 1000 / this.cb_Tablelist.Items.Count;
                
                
                tdSearchKeyword.Start();
            }
            else
            {
                bt_search.Text = "Search";
                pb_Searching.Value = 0;
                tdSearchKeyword.Abort();
                
            }


        }
        private void searchKeyword()
        {
//            Searching searching = new Searching();
//            searching.Show();
            
            
            //this.UseWaitCursor = true;
            int stringCheck = controlValue.language;
            int[] resultCount = new int[500];
            int i = 0;
            int resultTemp = 0;
            

            if (this.controlValue.tableName.Equals("ALL") || this.controlValue.tableName.Equals(""))
            {
                //this.cb_Tablelist.Text = "ALL";
                Invoke(ccChangeText, this.cb_Tablelist, "ComboBox", "ALL");
                
                foreach (string tb in controlValue.tableList)
                {
                    //this.pb_Searching.PerformStep();
                    Invoke(ccChangePBStep, pb_Searching, null, null);

                    if (!(tb.ToLower().Equals("all")))
                    {

                        resultTemp = search_table(tb, false, stringCheck);

                        if (resultTemp > 0 && resultTemp < 100)
                        {
                            //resultTable[i++] = tb;
                            resultCount[i++] = resultTemp;
                            //lb_result.Items.Add(tb + "  [" + resultTemp + "]");
                            Invoke(ccAddItem, lb_result, "ListBox", tb + "  [" + resultTemp + "]");

                        }
                        else if (resultTemp > 100)
                        {
                            //resultTable[i++] = tb;
                            resultCount[i++] = resultTemp;
                            //lb_result.Items.Add(tb + "  [100+]");
                            Invoke(ccAddItem, lb_result, "ListBox", tb + "  [100+]");
                        }
                    }
                }

                Invoke(ccChangePBStep, pb_Searching, null, "1000");

                if (lb_result.Items.Count == 0)
                {
                    //searching.Hide();
                    
                    //this.UseWaitCursor = false;
                    
                    //MessageBox.Show("검색 결과가 없습니다");
                    
                    Invoke(ccAddItem, lb_result, "ListBox", "검색 결과가 없습니다");
                    Invoke(ccMessageBoxShow, null, null, "검색 결과가 없습니다");
                    
                }
                //searching.Hide();
                
                
            }
            else
            {
                //this.pb_Searching.Step = 500;
                //this.pb_Searching.PerformStep();
                Invoke(ccChangePBStep, pb_Searching, null, "500");
                //search_table(this.cb_Tablelist.Text, true, stringCheck);
                
                search_table(controlValue.tableName, true, stringCheck);
                //this.pb_Searching.PerformStep();
                Invoke(ccChangePBStep, pb_Searching, null, "1000");
                //searching.Hide();
            }

            
            
            Invoke(ccChangeText, this.bt_search, "Button", "Search");
 
        }

        private int search_table(string tableName, bool viewTable, int stringCheck)
        {
            //this.bt_TableCommit.Text = "";
            try
            {
                threadData tData = new threadData();
                int rtValue = new int();
                //int[] searchFlag = new int[100000];
                
                string fileName = this.@tb_JsonPath.Text + @"\" + @tableName + ".json";
                tData.tableName = @tableName;
                tData.fileName = @fileName;
                //tData.keyword = this.@tb_search.Text.ToLower();
                tData.keyword = this.controlValue.keyword.ToLower();
                tData.stringCheck = stringCheck;
                tData.path = this.@tb_JsonPath.Text;

                Thread searchThread = new Thread(new ThreadStart(tData.threadSearchTable));

                searchThread.Start();
                searchThread.Join();
/*
                JArray jReader = JArray.Parse(File.ReadAllText(fileName));
                JArray jTable = new JArray();

                int i = 0;
                foreach (JObject record in jReader)
                {
                    if (record.ToString().ToLower().Contains(this.tb_search.Text.ToLower()))
                    {
                        searchFlag[i] = 1;
                        rtValue++;
                    }
                    i++;
                    if (rtValue > 100)
                        break;
                }

                for (int j = 0; j < i; j++)
                {
                    if (searchFlag[j] == 1)
                    {
                        jTable.Add(jReader[j]);
                    }

                }
*/
                rtValue = tData.rtValue;


                if (viewTable == true)
                {
                    //DataTable dt_json = tData.dt_json;
                    Invoke(ccViewDataGrid, tData.dt_json, null, null);
                    //dgv_result.DataSource = dt_json;
                    //dgv_result.AutoResizeColumns();
                    //this.bt_TableCommit.Text = tableName;
                    if (rtValue >= 100)
                    {
                        Invoke(ccChangeText, bt_searchAll, "Button", tableName + " 끝까지 찾기");
                        Invoke(ccChangeEnable, bt_searchAll, "Button", "true");
                        //bt_searchAll.Text = tableName + " 끝까지 찾기";
                        //bt_searchAll.Show();

                    }
                    else
                    {
                        Invoke(ccChangeText, bt_searchAll, "Button", "");
                        Invoke(ccChangeEnable, bt_searchAll, "Button", "false");
                    }
                    //this.dgv_result.Focus();
                    if (rtValue == 0)
                    {
                        //MessageBox.Show("검색 결과가 없습니다");
                        Invoke(ccChangePBStep, pb_Searching, null, "1000");
                        Invoke(ccMessageBoxShow, null, null, "검색 결과가 없습니다");
                        
                    }
                }

                return rtValue;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("테이블이 없거나 파일을 열 수 없습니다"+ Environment.NewLine + ex.Message);
                return 0;
            }



        }

        
        private void tb_search_KeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
            if (e.KeyCode == Keys.Enter)
            {
                bt_search_Click(sender, e);
            }
        }

        private void lb_result_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tbName = null;
            int stringCheck = new int();

            if (this.rb_Korean.Checked == true)
                stringCheck = 1;
            else if (this.rb_English.Checked == true)
                stringCheck = 2;

            if (lb_result.SelectedItem != null)
            {
                tbName = lb_result.SelectedItem.ToString().Remove(lb_result.SelectedItem.ToString().IndexOf(" ")).Trim();
                search_table(tbName, true, stringCheck);
            }
            //this.cb_Tablelist.Text = tbName;
            //tb_search.Text = lb_result.SelectedValue.ToString();


        }

        private void lb_result_DoubleClick(object sender, EventArgs e)
        {
            string tbName = null;

            if (lb_result.SelectedItem != null)
            {
                tbName = lb_result.SelectedItem.ToString().Remove(lb_result.SelectedItem.ToString().IndexOf(" ")).Trim();
            }
            this.cb_Tablelist.Text = tbName;

        }

        public void bt_searchAll_Click(object sender, EventArgs e)
        {

            threadData tData = new threadData();
            string tableName;
            tableName = bt_searchAll.Text.ToString();
            tableName = tableName.Remove(tableName.IndexOf(" 끝까지 찾기"));
            string fileName = this.tb_JsonPath.Text + @"\" + tableName + ".json";
            //tData.tableName = tableName;
            //tData.fileName = this.tb_JsonPath.Text + @"\cms_" + tableName + ".json";
/*
            int stringCheck = new int();

            if (this.rb_Korean.Checked == true)
                stringCheck = 1;
            else if (this.rb_English.Checked == true)
                stringCheck = 2;
*/
            tData.tableName = @tableName;
            tData.fileName = @fileName;
            //tData.keyword = this.@tb_search.Text.ToLower();
            tData.keyword = controlValue.keyword.ToLower();
            //tData.stringCheck = stringCheck;
            tData.path = this.@tb_JsonPath.Text;

            

            
            //Thread searchThread = new Thread(new ThreadStart(tData.threadSearchFull));
            if (tdSearchKeyword != null)
            {
                bt_search.Text = "Search";
                tdSearchKeyword.Abort();
            }

            tdSearchKeyword = new Thread(new ThreadStart(tData.threadSearchFull));


            bt_searchAll.Text = "검색중..";
            
            //searching.Show();
            this.pb_Searching.Value = 0;
            this.pb_Searching.Step = 300;
            this.pb_Searching.PerformStep();
            tdSearchKeyword.Start();
            tdSearchKeyword.Join();
            this.pb_Searching.PerformStep();

            //this.dgv_result.DataSource = tData.dt_json;
            Invoke(ccViewDataGrid, tData.dt_json, null, null);
            this.pb_Searching.PerformStep();
            this.dgv_result.AutoResizeColumns();
            //this.bt_TableCommit.Text = tableName;
            this.bt_searchAll.Hide();
            this.dgv_result.Focus();
            this.pb_Searching.Value = 1000;
            //searching.Hide();
        }

        private void dgv_result_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //this.bt_TableCommit.Text = this.bt_TableCommit.Text + " 파일에 쓰기";
            //this.bt_TableCommit.Show();
        }

        private void bt_TableCommit_Click(object sender, EventArgs e)
        {
            //JArray jsonTableName = new JArray();
            //JObject jsonWriter = new JObject();
            //string fixedTableName = this.bt_TableCommit.Text.Remove(this.bt_TableCommit.Text.IndexOf(" 파일에 쓰기")) + ".json";


        }

        private void dgv_result_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                this.cb_Tablelist.SelectedIndex = 0;
                this.tb_search.Text = this.dgv_result[e.ColumnIndex, e.RowIndex].Value.ToString();
                lb_result.Items.Clear();
            }
          
        }




        private void rb_Korean_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Korean.Checked == true) 
                cb_string.Checked = false;
        }

        private void rb_English_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_English.Checked == true)
                cb_string.Checked = false;
        }

        private void cbSession_TextUpdate(object sender, EventArgs e)
        {
            getFileNamelist();
        }

        private void cb_string_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_string.Checked == true)
            {
                this.rb_English.Checked = false;
                this.rb_Korean.Checked = false;
            }
            getFileNamelist();
        }

        private void bt_stringCancel_Click(object sender, EventArgs e)
        {
            this.rb_English.Checked = false;
            this.rb_Korean.Checked = false;
        }


        private void Form_JSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (bt_search.Text.Equals("Stop"))
                {
                    bt_search_Click(sender, e);
                }
            }
            
        }

        

        private void Form_JSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (tdSearchKeyword != null)
                tdSearchKeyword.Abort();
        }

        private void tb_search_Enter(object sender, EventArgs e)
        {
            tb_search.SelectAll();
        }

    }
}
