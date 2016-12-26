using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Converter
{
    class Read_RSA
    {
        public static string ReadLine_NameKKS(string MyFile, int Number_line, int What_Remove)
        {
            return File.ReadLines(MyFile).Skip(Number_line).First().Remove(0, What_Remove);
        }

        public static void Get_KKSnames(StreamReader Filename, List<Sencors> MyAllSensors)
        {
            for (int i = 0; i < 18; i++)
            {
                Filename.ReadLine();
            }

            List<string> MyListKKS = new List<string>();

            string Line;
            string All_Line = null;
            while ((Line = Filename.ReadLine()) != "MaxRowCnt=1000")
            {
                Line = Line.Remove(0, 13);
                All_Line = All_Line + Line;

                MyListKKS = All_Line.Split(';').ToList();          
            }
            MyListKKS.RemoveAt(MyListKKS.Count-1);

            for (int i = 0; i < MyListKKS.Count; i++)
            {
                Sencors MyOneSensors = new Sencors();
                MyOneSensors.KKS_Name = MyListKKS[i];;
                MyAllSensors.Add(MyOneSensors);
            }
        }
        
        public static void Get_MyListRecord(StreamReader Filename, List<Sencors> MyAllSensors, System.Windows.Forms.ProgressBar MyProgressBar, string MyFilename)
        {

             int Length_MyFile = System.IO.File.ReadAllLines(MyFilename).Length;

            string Line = null;

            while ((Line != "RsaData"))
            {
                Line = Filename.ReadLine();
                string Identificator_Start_Read = Line.Split('=')[0].Trim();
                if (Identificator_Start_Read == "RsaData")
                {
                    break;
                }
            }

               MyProgressBar.Maximum = Length_MyFile - 26;
               MyProgressBar.Visible = true;
               MyProgressBar.Minimum = 1;
          
            do
            {
                List<string> Values = new List<string>();
                Values.Clear();

                Line = Line.Remove(0, 8);

                Record MyRecord = new Record();
                MyRecord.DateTime = Convert.ToDateTime(Line.Split(';')[0]);                
                
                string With_Delete_Time = Line.Remove(0, 26);
                List<string> ValuesAndIndicator = new List<string>();

                ValuesAndIndicator = With_Delete_Time.Split(';').ToList();

                for (int i = 0; i < ValuesAndIndicator.Count - 1; i++)
                {
                    Values.Add(ValuesAndIndicator[i].Split(' ')[1]);

                }
                for (int i = 0; i < MyAllSensors.Count - 1; i++)
                {
                    MyRecord.Value = double.Parse(Values[i].Replace('.', ',').Trim());
                    MyAllSensors[i].MyListRecordsForOneKKS.Add(MyRecord);               
                }
                Line = Filename.ReadLine();
                MyProgressBar.Value++;
            }
            while (Line!= null);
           
            //for (int i = MyAllSensors[0].MyListRecordsForOneKKS.Count+1; i <= MyProgressBar.Maximum; i++)
            //{
            MyProgressBar.Value = MyProgressBar.Maximum;
            //}          
        }
    }
}
