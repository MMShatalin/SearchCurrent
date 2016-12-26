using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Converter
{
    class Reactivity
    {
        public static List<double> t = new List<double>();
        public static List<double> t1 = new List<double>();
        private double Time_Old;
        private double Time_Now;
     //   private double jOld;
        public double Ro; ////возвращает реактивность, рассчитанную в Беттах
        public double dT;

        double[] one = new double[6];
        double[] two = new double[6];
        double[] psi0 = new double[6];
        List<double> MyPsi = new List<double>();
        public void PSI0(double J_Old)
        {
            for (int i = 0; i < 6; i++)
            {
                psi0[i] = J_Old;
            }  
        }

     
        public double SearchR(double Time_Now, double Time_Old, double J_Now, double J_Old,  double[] l, double[] a)
        {
            double R = 0;

            dT = Time_Now - Time_Old;

            for (int i = 0; i < one.Length; i++)
            {
                double Const_t_raspada = l[i] * dT;
                one[i] = Math.Exp(-Const_t_raspada);
                two[i] = (1 - one[i]) / Const_t_raspada;
                psi0[i] = psi0[i] * one[i] - (J_Now - J_Old) * (two[i]) - J_Old * one[i] + J_Now;
                double yt = a[i] * psi0[i];
                R = R + yt;              
            }
            R = 1 - R / J_Now;
            return R;
        }      
    }
}