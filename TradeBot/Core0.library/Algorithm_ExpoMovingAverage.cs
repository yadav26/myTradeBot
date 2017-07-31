﻿using Google;
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

    ////Rule 1) – Buy(fresh long) when the short term moving averages (50days )turns greater than the long term
    ////moving average(100 days).Stay in the trade as long as this condition is satisfied
    ////Rule 2) – Exit the long position(square off) when the short term moving average (50days )turns lesser than
    ////the longer term moving average ( 100 days )


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
        //x multiplier + EMA(previous day). 

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
            Ticker = (Ticker == "MM" ? "M&M" : Ticker);
            SortedDictionary<int, float> Map_ClosePrice = StringTypeParser.get_TickerClosePriceMap(Exchange, Ticker, sd, ed, 86400, for_first_sma_period);
            if( null == Map_ClosePrice)
            {

               return;
            }
            float sma = 0;

            bool bIsFirstAssignmentDay = false;
            float yesterday_ema = 0.0f;

            for (int i = 0; i <= Sliding_Window; i++)
                sma += Map_ClosePrice.Values.ElementAt(i);

            sma = sma / 2;

            int counter = Sliding_Window+1; // 1 -> is oldest and last in latest in google return list

            Weight_Multiplier = (2.0f / (float)(Sliding_Window + 1));

            while ( counter < Map_ClosePrice.Count )
            {
                MovingAverageData dataObj = new MovingAverageData();
                float yesterday_close = Map_ClosePrice.Values.ElementAt(counter - 1);
                float today_close = Map_ClosePrice.Values.ElementAt(counter);

                dataObj.DateDay = Map_ClosePrice.Keys.ElementAt(counter);
                dataObj.Ticker = Ticker;
                dataObj.Exchange = Exchange;
                dataObj.LastClose = today_close;// Map_ClosePrice.Values.ElementAt(counter);

                if ( ! bIsFirstAssignmentDay)
                {
                    counter = Sliding_Window;
                    dataObj.TodayEMA = sma;
                    bIsFirstAssignmentDay = true;
                }
                else
                {
                    float value = 0.0f;

                    for (int i = counter; i > (counter - Sliding_Window); i-- )
                        value += Map_ClosePrice.Values.ElementAt( i );

                    sma = value / Sliding_Window;
                    dataObj.TodayEMA = ((today_close - yesterday_ema) * Weight_Multiplier) + dataObj.LastClose;

                }

                dataObj.TodaySMA = sma;

                sma = 0;

                yesterday_ema = dataObj.TodayEMA;

                Exponent_List.Add(dataObj);

                counter++;
            }

        }

    }
}
