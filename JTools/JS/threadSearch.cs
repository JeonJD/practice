using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace JSearch
{
    class threadSearch
    {
        static void searchFullTable(string tableName)
        {
            
            int[] searchFlag = new int[100000];
            string fileName = Form_JSearch.tb_JsonPath.Text + "\\cms_" + tableName + ".json";
            JArray jReader = JArray.Parse(File.ReadAllText(fileName));
            JArray jTable = new JArray();
            int i = 0;
            foreach (JObject record in jReader)
            {
                if (record.ToString().ToLower().Contains(this.tb_search.Text.ToLower()))
                {
                    searchFlag[i] = 1;
                }
                i++;
            }

            for (int j = 0; j < i; j++)
            {

                if (searchFlag[j] == 1)
                {
                    jTable.Add(jReader[j]);
                }

            }

            DataTable dt_json = JsonConvert.DeserializeObject<DataTable>(jTable.ToString());
            dgv_result.DataSource = dt_json;
            dgv_result.AutoResizeColumns();
            bt_searchAll.Hide();
            this.dgv_result.Focus();

        }

        }
    }
}
