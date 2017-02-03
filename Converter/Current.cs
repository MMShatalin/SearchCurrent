﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Converter
{
    internal class Current
    {
        #region Свойства

        //6 групп запаздывающих нейтронов
        //два массива для упрощения вычислений реактивности в дальнейшем
        private double[] _one = new double[6];
        private double[] _two = new double[6];

        //для поиска реактивности по формуле обращенного уравнения кинетики
        //это массив первых 6-ти значений, они все равны первому вычисленному току
        //так как нет предыдущего времени, а есть только первое значение
         double[] psi0 = new double[6];
       //  double[] FNext = new double[6];
        public double Ro; ////возвращает реактивность, рассчитанную в Беттах
        private double _tok1Old = double.NaN;
        private double _tok2Old = double.NaN;

     public double Tok1New;
        public double Tok2New;
        List<double> MyReac = new List<double>();
        public double Reactivity1;
        public double Reactivity2;
        public double ReactivityAverage;

        public double TimeNow;
        public double TimeOld;

        #endregion

        //токи типа должны быть вычислены до этого в методе
        //в методе также должна быть переменная времени
        //в данном случае берем Sensors, а не живые данные
        public void AddFirstData(Sencors R)
        {
            this._tok1Old = R.MyListRecordsForOneKKS[0].Value;
            this.TimeOld = R.MyListRecordsForOneKKS[0].ValueTimeForDAT;
            for (int i = 0; i < 6; i++)
            {
                psi0[i] = this._tok1Old;
            }
        }
        public void AddData(Sencors R, List<double>  MyListReactivity)
        {
            this._tok1Old = R.MyListRecordsForOneKKS[0].Value;
            this.TimeOld = R.MyListRecordsForOneKKS[0].ValueTimeForDAT;
            for (int i = 0; i < 6; i++)
            {
                psi0[i] = this._tok1Old;
            }

            for (int k = 1; k < R.MyListRecordsForOneKKS.Count; k++)
            {
                double deltaT = R.MyListRecordsForOneKKS[k].ValueTimeForDAT - TimeOld;
             //   var dt = R.MyListRecordsForOneKKS[k].Value - _tok1Old;
                Ro = 0;
             
                for (int i = 0; i < 6; i++)
                {
                    double constTRaspada = MyConst.LMetodiki[i] * deltaT;
                    _one[i] = Math.Exp(-constTRaspada);
                    _two[i] = (1 - _one[i]) / constTRaspada;
                    psi0[i] = psi0[i] * _one[i] - (R.MyListRecordsForOneKKS[k].Value - _tok1Old) * (_two[i]) - _tok1Old * _one[i] + R.MyListRecordsForOneKKS[k].Value; 
                    double yt = MyConst.AApik[i] * psi0[i];
                    Ro = Ro + yt;
                }
                Ro = 1 - Ro / R.MyListRecordsForOneKKS[k].Value;
                MyListReactivity.Add(Ro);
                _tok1Old = R.MyListRecordsForOneKKS[k].Value;
                TimeOld = R.MyListRecordsForOneKKS[k].ValueTimeForDAT;
            }
        }
    }



    class MyConst
        {
            #region Свойства

            //параметры запаздывающих нейтронов (постоянные распада - лямбда, взятые из методик физических испытаний)
            public static double[] LMetodiki = { 0.0127, 0.0317, 0.1180, 0.3170, 1.4000, 3.9200 };
            //параметры запаздывающих нейтронов (относительные групповые доли - альфа)
            public static double[] AApik = { 0.0340, 0.2010, 0.1840, 0.4040, 0.1430, 0.0340 };
            //коэффициент перевода из процентов в бетта эффективность
            private static double _beff = 0.72;

            #endregion
        }
    
}
