using Google;
using Quandl_FetchInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    public class MovingAverageData
    {
        public string Ticker { get; set;  }
        public string Exchange { get; set; }
        public float LastClose { get; set; }
        public float TodaySMA { get; set; }
        public float TodayEMA { get; set; }
        public int DateDay { get; set; }

    }


    public  class Algorithm_ExpoMovingAverage
    {
        //apply higher weightage to the most recent closing price
        /// <summary>
        /// EMA depends on calculation of the EMA of all prior days 
        /// Need more than far more than 10 days of data for accurate calculation
        /// 
        /// 
        /// Initial SMA: 10-period sum / 10 
        /// Multiplier: (2 / (Time periods + 1) ) = (2 / (10 + 1) ) = 0.1818 (18.18%)
        /// EMA: {Close - EMA(previous day)} x multiplier + EMA(previous day).
        ///the more data points you use, the more accurate your EMA will be.
        ///The goal is to maximize accuracy while minimizing calculation time.
        /// </summary>
        /// <returns></returns>
        /// Initial SMA: 10-period sum / 10 
        //Multiplier: (2 / (Time periods + 1) ) = (2 / (10 + 1) ) = 0.1818 (18.18%)

        //EMA: {Close - EMA(previous day)}
        //    x multiplier + EMA(previous day). 

        private static int Sliding_Window = 5;

        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public float Weight_Multiplier { get; set; }

        

        public List<MovingAverageData> Exponent_List { get; set; }


        public Algorithm_ExpoMovingAverage( string exch, string ticker, int periods, int window  )
        {

            Debug.Assert(periods != 0);

            Ticker = ticker;

            Exchange = exch;

            Sliding_Window = window;

            Exponent_List = new List<MovingAverageData>();

            //Calculate_WeightMulitplier(periods);

            int for_first_sma_period = periods + Sliding_Window;
            int start_day_for_expo_ma = periods;

            //Calculating past N periods date
            string sd = DateTime.Now.AddDays( (for_first_sma_period * (-1)) ).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";

            ///
            /// Will get data for 90days 
            /// https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=86400&p=90d&f=d,c
            ///

            //QHistory qhsNewObj = new QHistory(Exchange, Ticker, sd, ed);
            //List<QHistoryDatum> qHsList = qhsNewObj.GetQHistoryDatumList();
            //int lastIndex = qHsList.Count - 1;
            //foreach (QHistoryDatum qdatum in qHsList)
            //{
            //   //
            //}

            SortedDictionary<int, float> Map_ClosePrice = StringTypeParser.get_TickerClosePriceMap(Exchange, Ticker, sd, ed, 86400, for_first_sma_period);
            if( null == Map_ClosePrice)
            {

               return;
            }
                float sma = 0;
            int counter = Map_ClosePrice.Count - Sliding_Window;
            bool bIsFirstPreviousDay = false;
            float yesterday_ema = 0.0f;
            //for ( counter = Map_ClosePrice.Count - (1 + Sliding_Window); counter >= 0; counter -- )
            while ( --counter >= 0 )
            {
                sma = 0;
                float value = 0.0f;

                for ( int i = 1; i <= Sliding_Window; i++ )
                    value += Map_ClosePrice.Values.ElementAt(counter+i);

                sma = value / Sliding_Window;

                float yesterday_close = Map_ClosePrice.Values.ElementAt(counter+1);
                float today_close = Map_ClosePrice.Values.ElementAt(counter);

                MovingAverageData dataObj = new MovingAverageData();
                dataObj.DateDay = Map_ClosePrice.Keys.ElementAt(counter);
                dataObj.Ticker = Ticker;
                dataObj.Exchange = Exchange;
                dataObj.LastClose = Map_ClosePrice.Values.ElementAt(counter);
                dataObj.TodaySMA = sma;

                if (!bIsFirstPreviousDay)
                {
                    dataObj.TodayEMA = sma;
                    bIsFirstPreviousDay = true;
                }
                else
                {
                    Weight_Multiplier = (2.0f / (float)(Sliding_Window + 1));
                    dataObj.TodayEMA = ( ( today_close - yesterday_ema ) * Weight_Multiplier) + dataObj.LastClose;
                }

                yesterday_ema = dataObj.TodayEMA;

                Exponent_List.Add(dataObj);

            }

        }

    }
}
