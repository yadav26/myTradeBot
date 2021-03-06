﻿using Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;

namespace Core0.library
{
    class Algorithm_SimpleMovingAverage
    {

        public float LastClose { get; set; }
        public float SMA { get; set; }
        public int Window { get; set; }
        public int Period { get; set; }
        public int DateDay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="window"></param>
        /// <param name="period"></param>
        public Algorithm_SimpleMovingAverage(SortedDictionary<int, StringParsedData> map, int period, int window)
        {
            float recent_sma = 0;
            int start_day_index = map.Count - period - window;

            int ema_start_from = start_day_index + window;

            SortedDictionary<int, StringParsedData> reverseMap = new SortedDictionary<int, StringParsedData>();
            foreach (var kvp in map) //(int i = map.Count - 1; i >= 0; --i)
            {
                reverseMap.Add(map.Count - kvp.Key - 1, kvp.Value);
            }

            if(reverseMap.Count > 1)
            {
                for (int i = reverseMap.Count - 1; i > reverseMap.Count - 1 - window && i > 0; i--)
                {
                    recent_sma += reverseMap.ElementAt(i).Value.Close;
                }
            }
            else
            {
                recent_sma = reverseMap.ElementAt(0).Value.Close;
            }


            SMA = Formulas.banker_ceil(recent_sma / window); 
        }

        public int CalculateSimple_MA(SortedDictionary<int, StringParsedData> map, int period, int window)
        {
            //get last five days closing price
            // get simple average; compre with last closing price 
            //if lesser then MA in lagging
            //else MA is rising -> buy
            SortedDictionary<int, StringParsedData> reverseMap = new SortedDictionary<int, StringParsedData>();
            foreach (var kvp in map) //(int i = map.Count - 1; i >= 0; --i)
            {
                reverseMap.Add(map.Count - kvp.Key - 1, kvp.Value);
            }
            float sma = GetSMA(reverseMap, window, period);
            return 0;
        }

        float GetSMA(SortedDictionary<int, StringParsedData> map, int window, int period)
        {

            float sum = 0;

            if (period + window > map.Count)
                return 0;

            int start_day_index = map.Count - period - window;

            int sma_start_from = start_day_index + window;

            float finalSma = 0.0f;

            float lastClose = 0;

            for (int i = sma_start_from; i < map.Count; i++)
            {
                sum = 0;

                lastClose = map.ElementAt(i).Value.Close;

                for (int j = i; j > i - window; j--)
                {

                    sum += map.ElementAt(j).Value.Close;

                    //Console.Write("{0}-{1}-{2} ", j, kvp.Value, sum);
                }

                finalSma = sum / window;

                //Console.WriteLine("\nLoc:{0} Close:{1} SMA-{2}", i, lastClose, finalSma);

            }

            //Console.WriteLine("\nLoc:{0} Close:{1} SMA-{2}", i, lastClose, finalSma);

            return finalSma;
        }
        
    }

}
