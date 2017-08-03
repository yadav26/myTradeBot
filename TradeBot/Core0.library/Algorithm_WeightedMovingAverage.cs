using Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    public class Algorithm_WeightedMovingAverage
    {
        private static int Sliding_Window = 5;

        public float WMA { get; set; }


        


        public  Algorithm_WeightedMovingAverage(SortedDictionary<int, StringParsedData> map, int window, int period)
        {
            //Force 5 days Weighted Moving average
            window = 5;
            float day_minus_0 = map.ElementAt(0).Value.Close;
            this.WMA = Formulas.banker_ceil( (float)(day_minus_0 * (5.0f / 15.0f)) );

            float day_minus_1 = map.ElementAt(1).Value.Close;
            this.WMA += Formulas.banker_ceil(day_minus_1 * (4.0f / 15.0f));

            float day_minus_2 = map.ElementAt(2).Value.Close;
            this.WMA += Formulas.banker_ceil(day_minus_2 * (3.0f / 15.0f));

            float day_minus_3 = map.ElementAt(3).Value.Close;
            this.WMA += Formulas.banker_ceil(day_minus_3 * (2.0f / 15.0f));

            float day_minus_4 = map.ElementAt(4).Value.Close;
            this.WMA += Formulas.banker_ceil(day_minus_4 * (1.0f / 15.0f));
            
            
            //if (period + window > map.Count)
            //    return 0;


            //int start_day_index = map.Count - period - window;

            //int ma_start_from = start_day_index + window;


            //float lastClose = 0;


            //float cal_wma = 0;

            ////float Weight_Multiplier = (2.0f / (float)(window + 1));

            //for (int i = ma_start_from; i < map.Count; i++)
            //{
            //    //float yesterday_close = map.Values.ElementAt(i - 1);
            //    float today_close = map.Values.ElementAt(i);

            //    MovingAverageData dataObj = new MovingAverageData();

            //    dataObj.DateDay = map.Keys.ElementAt(i);
            //    dataObj.Ticker = "mt";
            //    dataObj.Exchange = "mE";
            //    dataObj.LastClose = today_close;

            //    sum = 0;

            //    for (int j = i; j > i - window; j--)
            //    {
            //        KeyValuePair<int, float> kvp = map.ElementAt(j);
            //        sum += (kvp.Value * (j / 15) );
            //        //Console.Write("I{0}-C{1}-S{2} ", j, kvp.Value, sum);
            //    }


            //    cal_wma = sum;

            //    //Console.WriteLine("\nFIRST : Loc:{0} Close:{1} SMA==EMA-{2}", i, today_close, cal_ema);

            //    dataObj.TodayWMA = cal_wma;


            //}

            ////Console.WriteLine("\nLoc:{0} Close:{1} SMA-{2}", i, lastClose, finalSma);


        }

    }
}
