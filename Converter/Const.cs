using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Converter
{
    class Const
    {
        /// <summary>
        /// Значения долей запаздывающих нейтронов в каждой группе
        /// </summary>
     //   public static double[] a = { 0.033, 0.219, 0.196, 0.395, 0.115, 0.042 };
        /// <summary>
        /// Постоянные времени распада ядер-предшественников запаздывающих нейтров j-й группы
        /// </summary>
     //   public static double[] Const_t_raspada = { 0.0124, 0.0305, 0.111, 0.301, 1.14, 3.01 };
        /// <summary>
        /// Необходимо уточнить!!!
        /// </summary>
        /// 
   //     public static double[] l_aknp1_01 = { 
                                               // 0.0124, 0.0305, 0.111, 0.301, 1.14, 3.01 
                                         //   };
     //   public static double[] betta_aknp_01 = { 
                                              //     0.02132, 0.151, 0.139, 0.2919, 0.09982, 0.0354 
                                            //   };

       public static double[] l_metodiki = {
                                               0.0127, 0.0317, 0.118, 0.317, 1.40, 3.92 
                                          };
     //  public static double[] betta_metodiki = {
                                               //    0.025, 0.149, 0.136, 0.298, 0.106, 0.025
                                           //    };

      //  public static double[] aAKR = {0.029, 0.204, 0.188, 0.395, 0.135, 0.048};

       public static double[] aAPIK = { 0.034, 0.202, 0.184, 0.403, 0.143, 0.034 };
        public static double Beff = 0.74;
    }
}
