﻿using Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;

namespace Core0.library
{
   public class Algorithm_VolumeWeightMA
    {
        public float VWMA { get; set; }

        public Algorithm_VolumeWeightMA(List<StringParsedData> ListTodayHistory)
        {

            if (ListTodayHistory == null)
                return;

            //Force 5 days Weighted Moving average
            int w = ListTodayHistory.Count < 5 ? ListTodayHistory.Count : 5 ;

            double volume = 0;
            double PVn = 0;
            for (int day = 0; day < w; day++)
            {
                PVn += (ListTodayHistory.ElementAt(day).Close * ListTodayHistory.ElementAt(day).Volume);
                volume += ListTodayHistory.ElementAt(day).Volume;
            }

            VWMA = Formulas.banker_ceil((float)(PVn / volume));


        }

        public Algorithm_VolumeWeightMA(SortedDictionary<int, StringParsedData> map, int p, int w )
        {

            //Force 5 days Weighted Moving average
            w = 5;


            float wvma = 0;
            double volume = 0;
            double PVn = 0;
            if( map.Count > 1)
            {
                for (int day = 0; day < w && map.Count > day; day++)
                {
                    PVn += (map.ElementAt(day).Value.Close * map.ElementAt(day).Value.Volume);
                    volume += map.ElementAt(day).Value.Volume;
                }
            }
            else
            {
                PVn += (map.ElementAt(0).Value.Close * map.ElementAt(0).Value.Volume);
            }


            VWMA = Formulas.banker_ceil((float)(PVn / volume));


        }

    }
}
