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
                    calculateaveragepoint(outputpath, ply);
                    Leaders(outputpath, ply);

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

        //Question1: Sort players by PPG descending order
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

        //Question2: Calculate average poimt
        public void calculateaveragepoint(string outputpath, List<Player> emplist)
        {
            Double average = 0;
            Double totalpoint = 0;

            if (emplist.Count > 0)
            {
                totalpoint = emplist.Sum(x => x.PPG);
                average = Math.Round(totalpoint / emplist.Count, 2);
            }

            string jsonaverage = JsonConvert.SerializeObject(average, Formatting.Indented);
            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"AveragePPG\": " + jsonaverage + ",");
        }

        //Question3: find leaders
        public void Leaders(string outputpath, List<Player> ply)
        {

            WriteAdditionalprefixtoJson(outputpath, "\r\n" + "\"Leaders\":" + " [{" + "\r\n");
            // WriteAdditionalprefixtoJson("Leaders: \" [{\"");
            calculateGold(outputpath, ply);
            //  WriteAdditionalprefixtoJson("\"GoldPPG\": ");
            //calculateGoldPPG(emp);
            WriteAdditionalprefixtoJson(outputpath, "}," + "\r\n" + "{");
            calculatesilver(outputpath, ply);
            WriteAdditionalprefixtoJson(outputpath, "}," + "\r\n" + "{");
            calculatebronze(outputpath, ply);
            WriteAdditionalprefixtoJson(outputpath, "}]," + "\r\n");
            WriteAdditionalprefixtoJson(outputpath, "\"\": {");        
            WriteAdditionalprefixtoJson(outputpath, "},");

        }



        public void calculateGold(string outputpath, List<Player> ply)
        {

            Player gold = null;

            if (ply.Count > 0)
            {
                gold = ply.OrderByDescending(x => x.PPG).ToList().First();
            }

            string jsongoldname = JsonConvert.SerializeObject(gold.Name, Formatting.Indented);
            string jsongoldPPG = JsonConvert.SerializeObject(gold.PPG, Formatting.Indented);

            //string leaders = JsonConvert.SerializeObject("Leaders:", Formatting.Indented);
            //File.AppendAllText(@"C:\Users\User\InvestigationApps\jsonoutputSol\csvtojson\output\output2.js", leaders);

            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"Gold\": " + jsongoldname + "," + "\r\n" + "\"GoldPPG\": " + jsongoldPPG);
            //   
        }

        //public void AppendAllTextToJsonFile(string path, string prefix)
        //{
        //    File.AppendAllText(GetFilePath(Directory.CreateDirectory(Path.GetDirectoryName(path))), prefix);
        //}
        //Directory.CreateDirectory(Path.GetDirectoryName(filename))


        public void calculatesilver(string outputpath, List<Player> ply)
        {
            Player silver = null;
            if (ply.Count > 1)
            {
                silver = ply.OrderByDescending(x => x.PPG).ToList().Skip(1).First();
            }
            string jsonsilvername = JsonConvert.SerializeObject(silver.Name, Formatting.Indented);
            string jsonsilverPPG = JsonConvert.SerializeObject(silver.PPG, Formatting.Indented);
            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"silver\": " + jsonsilvername + "," + "\r\n" + "\"silverPPG\": " + jsonsilverPPG);
            //   File.AppendAllText(@"C:\Users\User\InvestigationApps\jsonoutputSol\csvtojson\output\output2.js", "\"silverPPG\": " + jsonsilverPPG);
        }




        public void calculatebronze(string outputpath, List<Player> ply)
        {
            Player bronze = null;
            if (ply.Count > 2)
            {
                bronze = ply.OrderByDescending(x => x.PPG).ToList().Skip(2).First();
                string jsonbronzename = JsonConvert.SerializeObject(bronze.Name, Formatting.Indented);
                string jsonbronzePPG = JsonConvert.SerializeObject(bronze.PPG, Formatting.Indented);
                File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"bronze\": " + jsonbronzename + "," + "\r\n" + "\"bronzePPG\": " + jsonbronzePPG);
                // File.AppendAllText(@"C:\Users\User\InvestigationApps\jsonoutputSol\csvtojson\output\output2.js", "\"bronzePPG\": " + jsonbronzePPG);
            }
        }


    }
}