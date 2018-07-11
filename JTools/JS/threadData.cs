using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;


namespace JS
{
    class threadData
    {
        public string tableName;
        public string path;
        public string fileName;
        public string keyword;
        public DataTable dt_json;
        public int rtValue = new int();
        public int stringCheck = new int();
        //public Thread killThread;
        //public string[] resultTable = new string[500];

        
        public void threadSearchTable()
        {
            //int[] searchFlag = new int[100000];
            try
            {
                string sourceTable = File.ReadAllText(fileName);
/*
                JArray stringMap = JArray.Parse(File.ReadAllText(@"stringmap.json"));
                JArray stringFile = null;
                foreach (JObject stringMapRecord in stringMap)
                {
                    if (stringMapRecord["table_name"].ToString().Equals(tableName + ".json"))
                    {
                        if (stringMapRecord["string_file_name"].ToString() != "")
                        {
                            switch (stringCheck)
                            {
                                case 1:
                                    stringFile = JArray.Parse(File.ReadAllText(@path + @"\strings_korean\" + stringMapRecord["string_file_name"].ToString()));
                                    break;
                                case 2:
                                    stringFile = JArray.Parse(File.ReadAllText(@path + @"\strings_english\" + stringMapRecord["string_file_name"].ToString()));
                                    break;
                            }
                            if (stringFile != null)
                            {
                                foreach (JObject stringRecord in stringFile)
                                    sourceTable = sourceTable.Replace("\"" + stringRecord["key"].ToString() + "\"", "\"" + stringRecord["value"].ToString() + "\"");
                            }
                        }
                    }
                }
*/


                JArray jReader = JArray.Parse(sourceTable);
                JArray jTable = new JArray();
                //int i = 0;
                jReader.RemoveAt(0);
                foreach (JObject record in jReader)
                {
                    
                    if (record.ToString().ToLower().Contains(keyword))
                    {
                        //searchFlag[i] = 1;
                        jTable.Add(record);
                        rtValue++;
                    }
                    //i++;
                    if (rtValue > 100)
                        break;
                }
/*
                for (int j = 0; j < i; j++)
                {
                    if (searchFlag[j] == 1)
                    {
                        jTable.Add(jReader[j]);
                    }

                }
*/
                dt_json = JsonConvert.DeserializeObject<DataTable>(jTable.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        public void threadSearchFull()
        {
//            int[] searchFlag = new int[100000];
//            Thread.Yield();
            try
            {
                string sourceTable = File.ReadAllText(fileName);
/*
                if (stringCheck == 1)
                {
                    JArray stringMap = JArray.Parse(File.ReadAllText(@"stringmap.json"));
                    foreach (JObject stringMapRecord in stringMap)
                    {
                        if (stringMapRecord["table_name"].ToString().Equals(tableName + ".json"))
                        {
                            if (stringMapRecord["string_file_name"].ToString() != "")
                            {
                                JArray stringFile = JArray.Parse(File.ReadAllText(@path + @"\strings_korean\" + stringMapRecord["string_file_name"].ToString()));
                                foreach (JObject stringRecord in stringFile)
                                    sourceTable = sourceTable.Replace("\"" + stringRecord["key"].ToString() + "\"", "\"" + stringRecord["value"].ToString() + "\"");
                            }
                        }
                    }
                }


                else if (stringCheck == 2)
                {
                    JArray stringMap = JArray.Parse(File.ReadAllText(@"stringmap.json"));
                    foreach (JObject stringMapRecord in stringMap)
                    {
                        if (stringMapRecord["table_name"].ToString().Equals(tableName + ".json"))
                        {
                            if (stringMapRecord["string_file_name"].ToString() != "")
                            {
                                JArray stringFile = JArray.Parse(File.ReadAllText(@path + @"\strings_english\" + stringMapRecord["string_file_name"].ToString()));
                                foreach (JObject stringRecord in stringFile)
                                    sourceTable = sourceTable.Replace("\"" + stringRecord["key"].ToString() + "\"", "\"" + stringRecord["value"].ToString() + "\"");
                            }
                        }
                    }
                }
*/
                JArray jReader = JArray.Parse(sourceTable);
                JArray jTable = new JArray();
                //                int i = 0;
                jReader.RemoveAt(0);
                foreach (JObject record in jReader)
                {
                    if (record.ToString().ToLower().Contains(keyword))
                    {
                        //searchFlag[i] = 1;
                        jTable.Add(record);
                    }
                    //i++;
                }
/*
                for (int j = 0; j < i; j++)
                {

                    if (searchFlag[j] == 1)
                    {
                        jTable.Add(jReader[j]);
                    }

                }
*/
                dt_json = JsonConvert.DeserializeObject<DataTable>(jTable.ToString());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


        }

    }
}
