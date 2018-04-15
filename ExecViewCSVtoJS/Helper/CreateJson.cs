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
                    calculateaverageHeight(outputpath, ply);

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
           
            calculateGold(outputpath, ply);
          
            WriteAdditionalprefixtoJson(outputpath, "}," + "\r\n" + "{");
            calculatesilver(outputpath, ply);
            WriteAdditionalprefixtoJson(outputpath, "}," + "\r\n" + "{");
            calculatebronze(outputpath, ply);
            WriteAdditionalprefixtoJson(outputpath, "}]," + "\r\n");
            WriteAdditionalprefixtoJson(outputpath, "\"\": {");
            playersbypositions(outputpath, ply);
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

            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"Gold\": " + jsongoldname + "," + "\r\n" + "\"GoldPPG\": " + jsongoldPPG);
           
        }


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

        //Question4: find players by position.
        public void playersbypositions(string outputpath, List<Player> ply)
        {

            int totalPG = 0;
            int totalC = 0;
            int totalPF = 0;
            int totalSG = 0;
            int totalSF = 0;

            if (ply.Count > 0)
            {
                totalPG = ply.Count(p => p.Position == "PG");
                totalC = ply.Count(p => p.Position == "C");
                totalPF = ply.Count(p => p.Position == "PF");
                totalSG = ply.Count(p => p.Position == "SG");
                totalSF = ply.Count(p => p.Position == "SF");
            }

            //  string presentemedals = JsonConvert.SerializeObject("" +": {", Formatting.Indented);// WriteAdditionalprefixtoJson("\"\": {");
            string jsontotalPG = JsonConvert.SerializeObject(totalPG, Formatting.Indented);
            string jsontotalC = JsonConvert.SerializeObject(totalC, Formatting.Indented);
            string jsontotalPF = JsonConvert.SerializeObject(totalPF, Formatting.Indented);
            string jsontotalSG = JsonConvert.SerializeObject(totalSG, Formatting.Indented);
            string jsontotalSF = JsonConvert.SerializeObject(totalSF, Formatting.Indented);

            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"PG\": " + jsontotalPG + ", " + "\r\n" + "\"C\": " + jsontotalC + ",");
            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"PF\": " + jsontotalPF + ", " + "\r\n" + "\"SG\": " + jsontotalSG + ",");
            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"SF\": " + jsontotalSF);
        }

        //Question5: calculate average height
        public double convertTocentimeter(string height)
        {
            double cm1;
            double cm2;

            string feetpart = height.Split('f')[0];
            string inchpart0 = height.Split('t')[1].Trim();
            string inchpart = inchpart0.Trim().Substring(0, inchpart0.Trim().Length - 2);
            double inc = Convert.ToDouble(inchpart);
            cm1 = inc * 2.54;
            cm2 = Convert.ToDouble(feetpart) * 12 * 2.54;

            return cm1 + cm2;
        }
        public void calculateaverageHeight(string outputpath, List<Player> plylist)
        {
            Double average = 0;
            Double totalHeight = 0;

            if (plylist.Count > 0)
            {
                totalHeight = plylist.Sum(x => convertTocentimeter(x.Height));
                average = Math.Round(totalHeight / plylist.Count, 2);
            }

            string jsonaverage = JsonConvert.SerializeObject(average, Formatting.Indented);
            File.AppendAllText(GetFilePath(outputpath), "\r\n" + "\"AverageHeight\": " + jsonaverage + " cm");
        }

    }
}