using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.Drawing;

namespace Converter
{
    class In_Excel
    {

        public static void Get_to_Excel(int CheckedListBox1_Count, CheckedListBox ter, List<Sencors> MyAllSensors, string Save_Path, ProgressBar MyProgressBar)
        {


            Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
            exApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)exApp.ActiveSheet;

            int l = 0;
            int t = 1;
            int r = 2;
        
            for (int i = 0; i < CheckedListBox1_Count; i++) //По всем отвеченным
            {
                MyProgressBar.CreateGraphics().DrawString("Идет процесс извлечения данных...", new System.Drawing.Font("Arial", (float)7.15, FontStyle.Regular), Brushes.Black, new PointF(MyProgressBar.Width / 3 - 20, MyProgressBar.Height / 2 - 7));
                MyProgressBar.Value = i + 1;

                for (int ii = 0; ii < MyAllSensors.Count; ii++)
                {
                    if ((string)ter.CheckedItems[i] == MyAllSensors[ii].KKS_Name)
                    {
                        workSheet.Cells[1, t + l] = "Время";
                        workSheet.Cells[1, r + l] = MyAllSensors[ii].KKS_Name;
                       
                        int b = MyAllSensors[ii].MyListRecordsForOneKKS.Count;
                        //формула экселя на второй строке с 3 ей по каунт
                        workSheet.Cells[2, r + l].FormulaR1C1 = "=AVERAGE(R[1]C:R[" + b + "]C)";

                        int rowExcel = 3; //начать с 3 ей строки.

                        for (int j = 0; j <MyAllSensors[ii].MyListRecordsForOneKKS.Count; j++)
                        {
                            //заполняем строку
                            workSheet.Cells[rowExcel, t + l] = MyAllSensors[ii].MyListRecordsForOneKKS[j].DateTime;
                            workSheet.Cells[rowExcel, r + l] = MyAllSensors[ii].MyListRecordsForOneKKS[j].Value;

                            //переход на строчку вниз
                            ++rowExcel;
                        }

                        //растягивает колонки ЭКСЕЛЯ по тексту
                        workSheet.Columns.AutoFit();

                        Microsoft.Office.Interop.Excel.Application xl = new Microsoft.Office.Interop.Excel.Application();
                        Microsoft.Office.Interop.Excel.WorksheetFunction wsf = xl.WorksheetFunction;

                        l = l + 2;
                                              
                    }
                }
            }

            workSheet.SaveAs(Save_Path);
            

            string message = "Открыть файл " + Save_Path + " в Excel?";
            string caption = "Обработка в Excel";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {

                exApp.Quit();
            }
            if (result == DialogResult.Yes)
            {
                exApp.Quit();
                Microsoft.Office.Interop.Excel.Workbook fileExcel;
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                fileExcel = excel.Workbooks.Open(Save_Path + ".xlsx");
                excel.Visible = true;
            }
        }//FOR        
    }
}

