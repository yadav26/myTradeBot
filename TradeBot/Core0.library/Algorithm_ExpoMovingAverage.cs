using Google;
using Quandl_FetchInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;
using Trading.Entity;

namespace Core0.library
{

    //As a general guideline, 
    //BUY  -> if the price is above a moving average the trend is up.
    //SELL -> If the price is below a moving average the trend is down

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


        public float Weight_Multiplier { get; set; }
        public float EMA { get; set; }
        

        public List<MovingAverageData> Exponent_List { get; set; }


        //public Algorithm_ExpoMovingAverage_old( string exch, string ticker, int periods, int window  )
        //{

        //    Debug.Assert(periods != 0);

        //    Ticker = ticker;

        //    Exchange = exch;

        //    Sliding_Window = window;

        //    Exponent_List = new List<MovingAverageData>();

        //    //Calculate_WeightMulitplier(periods);

        //    int for_first_sma_period = periods + Sliding_Window;
        //    int start_day_for_expo_ma = periods;

        //    //Calculating past N periods date
        //    string sd = DateTime.Now.AddDays( (for_first_sma_period * (-1)) ).ToString("yyyy-M-d");
        //    string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";

        //    ///
        //    /// Will get data for 90days 
        //    /// https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=86400&p=90d&f=d,c
        //    ///

        //    //QHistory qhsNewObj = new QHistory(Exchange, Ticker, sd, ed);
        //    //List<QHistoryDatum> qHsList = qhsNewObj.GetQHistoryDatumList();
        //    //int lastIndex = qHsList.Count - 1;
        //    //foreach (QHistoryDatum qdatum in qHsList)
        //    //{
        //    //   //
        //    //}
        //    Ticker = (Ticker == "MM" ? "M&M" : Ticker);

        //    SortedDictionary<int, float> Map_ClosePrice = null;// new SortedDictionary<int, float>();

        //    StringTypeParser.get_TickerClosePriceMap(out Map_ClosePrice, Exchange, Ticker, sd, ed, 86400, for_first_sma_period);
        //    if( null == Map_ClosePrice)
        //    {
        //       return;
        //    }

        //    float sma = 0;

        //    bool bIsFirstAssignmentDay = false;
        //    float yesterday_ema = 0.0f;

        //    for (int i = 0; i <= Sliding_Window; i++)
        //        sma += Map_ClosePrice.Values.ElementAt(i);

        //    sma = sma / Sliding_Window;

        //    int counter = Sliding_Window+1; // 1 -> is oldest and last in latest in google return list

        //    Weight_Multiplier = (2.0f / (float)(Sliding_Window + 1));

        //    while ( counter < Map_ClosePrice.Count )
        //    {
        //        MovingAverageData dataObj = new MovingAverageData();
        //        float yesterday_close = Map_ClosePrice.Values.ElementAt(counter - 1);
        //        float today_close = Map_ClosePrice.Values.ElementAt(counter);

        //        dataObj.DateDay = Map_ClosePrice.Keys.ElementAt(counter);
        //        dataObj.Ticker = Ticker;
        //        dataObj.Exchange = Exchange;
        //        dataObj.LastClose = today_close;// Map_ClosePrice.Values.ElementAt(counter);

        //        if ( ! bIsFirstAssignmentDay)
        //        {
        //            counter = Sliding_Window;
        //            dataObj.TodayEMA = sma;
        //            bIsFirstAssignmentDay = true;
        //        }
        //        else
        //        {
        //            float value = 0.0f;

        //            for (int i = counter; i > (counter - Sliding_Window); i-- )
        //                value += Map_ClosePrice.Values.ElementAt( i );

        //            sma = value / Sliding_Window;
        //            dataObj.TodayEMA = ((today_close - yesterday_ema) * Weight_Multiplier) + dataObj.LastClose;

        //        }

        //        dataObj.TodaySMA = sma;

        //        sma = 0;

        //        yesterday_ema = dataObj.TodayEMA;

        //        Exponent_List.Add(dataObj);

        //        counter++;
        //    }

        //}
        public Algorithm_ExpoMovingAverage(List< StringParsedData> ListTodayData, int periods, int window)
        {


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
            int index = 0;
            window = 10;
            periods = 90;
            SortedDictionary<int, StringParsedData> reverseMap = new SortedDictionary<int, StringParsedData>();
            foreach (StringParsedData data in ListTodayData) 
            {
                reverseMap.Add(index ++, data);
            }

            float TodayEMA = GetNewEMA(reverseMap, periods, window);

            this.EMA = Formulas.banker_ceil(TodayEMA);


        }

        public Algorithm_ExpoMovingAverage(SortedDictionary<int, StringParsedData> map, int periods, int window)
        {


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
            SortedDictionary<int, StringParsedData> reverseMap = new SortedDictionary<int, StringParsedData>();
            foreach( var kvp in map) //(int i = map.Count - 1; i >= 0; --i)
            {
                reverseMap.Add(map.Count-kvp.Key-1, kvp.Value); 
            }

            float TodayEMA = GetNewEMA(reverseMap, periods, window);

            this.EMA = Formulas.banker_ceil(TodayEMA);


        }

        public Algorithm_ExpoMovingAverage()
        {
        }

        public float GetNewEMA(SortedDictionary<int, StringParsedData> map, int period, int window)
        {

            float sum = 0;


            if (period + window > map.Count)
                return 0;


            int start_day_index = map.Count - period - window;

            int ema_start_from = start_day_index + window;


            float lastClose = 0;

            bool bIsFirstEMACalculation = true;


            float yesterday_ema = 0;

            float cal_ema = 0;

            float Weight_Multiplier = (2.0f / (float)(window + 1));

            for (int i = ema_start_from; i < map.Count; i++)
            {

                float today_close = map.ElementAt(i).Value.Close;

                sum = 0;

                lastClose = today_close;

                if (bIsFirstEMACalculation == true)
                {
                    for (int j = i; j > i - window; j--)
                    {
                        float close_val = map.ElementAt(j).Value.Close;
                        sum += close_val;

                    }
                    bIsFirstEMACalculation = false;

                    cal_ema = sum / window;

                }
                else
                {
                    cal_ema = ((today_close - yesterday_ema) * Weight_Multiplier) + today_close;

                }


                yesterday_ema = cal_ema;

            }

            return yesterday_ema;
        }

        public int Warm_up_time(string exch, string ticker, string sd, string ed)
        {
            throw new NotImplementedException();
        }

        public float Execute_Strategy(Func<CurrentOrderUpdater, int> func1, CurrentOrderUpdater objCurrentStatus, float fetched_price, int units)
        {
            throw new NotImplementedException();
        }
    }
}
