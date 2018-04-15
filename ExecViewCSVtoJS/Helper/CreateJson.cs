using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExecViewCSVtoJS.Models;

namespace ExecViewCSVtoJS.Helper
{
    public class CreateJson
    {

        public void Executecsv(string inputpath, string outputpath)
        {
            //Upload and save the file  
            //string csvPath = @"C:\Users\User\InvestigationApps\jsonoutputSol\csvtojson\input\input2.csv";
            //File.WriteAllText(@"C:\Users\User\InvestigationApps\jsonoutputSol\csvtojson\output\output2.js", string.Empty);
            try
            {
                string csvPath = GetFilePath(inputpath);
                File.WriteAllText(GetFilePath(outputpath), string.Empty);


                //Read the contents of CSV file.           
                string[] col;
                char[] seperators = { '\t', ',', '\\' };
                Player eObj = null;


                WriteprefixtoJson(outputpath, "{" + "\r\n" + "\"Players\": ");
                List<Player> ply = new List<Player>();
                using (StreamReader sr = new StreamReader(GetFilePath(inputpath)))
                {
                    string data = sr.ReadLine();

                    while ((data = sr.ReadLine()) != null)
                    {
                        col = data.Split(seperators, StringSplitOptions.None);

                        if (col.Length >= 1)
                        {
                            eObj = new Player();
                            eObj.Id = col[0].ToString();
                            eObj.Position = col[1].ToString();
                            eObj.Number = col[2].ToString();
                            eObj.Country = col[3].ToString();
                            eObj.Name = col[4].ToString().Substring(1).Trim() + "," + col[5].ToString().Substring(0, col[5].ToString().Length - 1);
                            eObj.Height = col[6].ToString();
                            eObj.Weight = col[7].ToString();
                            eObj.University = col[8].ToString();
                            eObj.PPG = Double.Parse(col[9]);
                            ply.Add(eObj);
                        }

                    }

                    WritetoJsonbylist(ply, GetFilePath(outputpath));                

                }
            }
            catch (Exception exc)
            {
                //exc.Message
            }

        }


        public string GetFilePath(string path)
        {
            return path;
        }

        public void WritetoJsonbylist(List<Player> ply, string path)
        {
            List<Player> orderedEmp = ply.OrderByDescending(x => x.PPG).ToList();

            string json = JsonConvert.SerializeObject(orderedEmp, Formatting.Indented);

            File.AppendAllText(GetFilePath(path), json + ",");
        }

        public void WriteAdditionalprefixtoJson(string path, string prefix)
        {
            string jsonprefix = JsonConvert.SerializeObject(prefix, Formatting.Indented);
            //write string to file
            File.AppendAllText(GetFilePath(path), "\r\n" + prefix);
        }

        public void WriteprefixtoJson(string outputpath, string prefix)
        {
            string jsonprefix = JsonConvert.SerializeObject(prefix, Formatting.Indented);
            //write string to file
            File.WriteAllText(GetFilePath(outputpath), prefix);
        }

    }
}