using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Converter
{
    class Current
    {
        #region Свойства

        //6 групп запаздывающих нейтронов
        //два массива для упрощения вычислений реактивности в дальнейшем
        double[] _one = new double[6];
        double[] _two = new double[6];

        //для поиска реактивности по формуле обращенного уравнения кинетики
        //это массив первых 6-ти значений, они все равны первому вычисленному току
        //так как нет предыдущего времени, а есть только первое значение
        double[] _psi01 = new double[6];
        double[] _psi02 = new double[6];

        public double Ro; ////возвращает реактивность, рассчитанную в Беттах
        private double _tok1Old = double.NaN;
        private double _tok2Old = double.NaN;

        public Current()
        {
            TimeOld = DateTime.MinValue;
        }

        public double Tok1New;
        public double Tok2New;

        public double Reactivity1;
        public double Reactivity2;
        public double ReactivityAverage;

        public DateTime TimeNow;
        public DateTime TimeOld;

        #endregion



        //TODO: Александр. Старые значения времени и токов нужно хранить в полях класса. Извне брать только текущие значения.
        //эти методы должны расчитывать реактивности из Ток1 и Ток2
        public void SearchReactivity(double[] l, double[] a, List<Sencors> N)
        {
            DateTime timeNow;
            for (int ll = 0; ll < N[0].MyListRecordsForOneKKS.Count - 1; ll++)
            {
                if (TimeOld.Equals(DateTime.MinValue) && _tok1Old.Equals(double.NaN) && _tok2Old.Equals(double.NaN))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        _psi01[i] = N[0].MyListRecordsForOneKKS[0].Value;
                    }
                    TimeOld = N[0].MyListRecordsForOneKKS[0].DateTime;

                    _tok1Old = N[0].MyListRecordsForOneKKS[0].Value;
                    timeNow = DateTime.Now;
                }
                else
                {
                    timeNow = N[0].MyListRecordsForOneKKS[ll].DateTime;
                    
                }
            

                var dt = timeNow - TimeOld;
               MessageBox.Show(dt.ToString());
                for (int i = 0; i < _one.Length; i++)
                {
                    double constTRaspada = l[i]*dt.TotalMilliseconds;
                    MessageBox.Show(dt.TotalMilliseconds.ToString());
                    _one[i] = Math.Exp(-constTRaspada);
                    _two[i] = (1 - _one[i])/constTRaspada;
                    _psi01[i] = _psi01[i] * _one[i] - (N[0].MyListRecordsForOneKKS[ll + 1].Value - _tok1Old) * _two[i] - _tok1Old * _one[i] + N[0].MyListRecordsForOneKKS[ll + 1].Value;
                    Reactivity1 += a[i]*_psi01[i];
                }

                //Зачем потребовалось создать _timeNow ?? Да просто иначе если бы в этой строке стояло бы DateTime.Now то это было бы уже другое время, нежели участвующее в формуле выше!!!
                TimeOld = timeNow;
                _tok1Old = Tok1New;
                Reactivity1 = 1 - Reactivity1 / Tok1New;
             //   MessageBox.Show(Reactivity1.ToString());
            }
        }

        class MyConst
        {
            #region Свойства

            //параметры запаздывающих нейтронов (постоянные распада - лямбда, взятые из методик физических испытаний)
            public static double[] LMetodiki = { 0.0127, 0.0317, 0.1180, 0.3170, 1.4000, 3.9200 };
            //параметры запаздывающих нейтронов (относительные групповые доли - альфа)
            public static double[] AApik = { 0.0340, 0.2020, 0.1840, 0.4030, 0.1430, 0.0340 };
            //коэффициент перевода из процентов в бетта эффективность
            private static double _beff = 0.74;

            #endregion
        }
    }
}
