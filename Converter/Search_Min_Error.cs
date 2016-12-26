using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Converter
{
    class Search_Min_Error
    {
    

        public static void Cursor_Value(int NumberSeries, List<Sencors> Value, double PixelPositionToValue, Chart t, DataTable u, List<DateTime> ddd)
        {
            double r = 0;
            DateTime q = new DateTime();
            List<double> er = new List<double>();
            for (int ii = 0; ii < NumberSeries; ii++)
            {
                er.Clear();
                for (int j = 0; j < Value.Count; j++)
                {
                    if (t.Series[ii].LegendText == Value[j].KKS_Name)
                    {
                        for (int jj = 0; jj < Value[j].MyListRecordsForOneKKS.Count; jj++)
                        {

                            double w = Value[j].MyListRecordsForOneKKS[jj].DateTime.ToOADate() - PixelPositionToValue;
                            w = Math.Abs(w);
                            er.Add(w);
                        }

                        er.IndexOf(er.Min());

                        r = Value[j].MyListRecordsForOneKKS[er.IndexOf(er.Min())].Value;
                        q = Value[j].MyListRecordsForOneKKS[er.IndexOf(er.Min())].DateTime;
                    }
                }            
                u.Rows.Add(t.Series[ii].LegendText, q, r);
             //   ddd.Add(q)
            }//for
             ddd.Add(q);
    
        }// Конец метода

        public static void Cursor_Value_Average(int NumberSeries, List<Sencors> Value, double PixelPositionToValue, Chart t, DataTable u, int AmountPoint)
        {
            double r = 0;
            double sum = 0;
            DateTime q = new DateTime();
            int tre = 0;
            
            List<double> er = new List<double>();
            for (int ii = 0; ii < NumberSeries; ii++)
            {
                er.Clear();
                for (int j = 0; j < Value.Count; j++)
                {
                    if (t.Series[ii].LegendText == Value[j].KKS_Name)
                    {
                        for (int jj = 0; jj < Value[j].MyListRecordsForOneKKS.Count; jj++)
                        {

                            double w = Value[j].MyListRecordsForOneKKS[jj].DateTime.ToOADate() - PixelPositionToValue;
                            w = Math.Abs(w);
                            er.Add(w);
                        }

                        tre = er.IndexOf(er.Min());

                        r = Value[j].MyListRecordsForOneKKS[tre].Value;
                        q = Value[j].MyListRecordsForOneKKS[tre].DateTime;
                        sum = 0;
                        if (tre >= AmountPoint)
                        {
                            for (int jjj = tre - AmountPoint; jjj < tre + 1; jjj++)
                            {
                                sum = (sum + Value[j].MyListRecordsForOneKKS[jjj].Value);
                            }
                        }
                        if (tre < AmountPoint)
                        {
                            for (int jjj = 0; jjj < tre + 1; jjj++)
                            {
                                sum = (sum + Value[j].MyListRecordsForOneKKS[jjj].Value);
                            }
                        }
                    }
                }

                if (tre >= AmountPoint)
                {

                    double aver = sum / (AmountPoint + 1);
                    u.Rows.Add(t.Series[ii].LegendText, q, aver);
                }

                if (tre < AmountPoint)
                {

                    double aver = sum / tre;
                    u.Rows.Add(t.Series[ii].LegendText, q, aver);
                }

            }//for
        }// Конец метод
        }
    }

