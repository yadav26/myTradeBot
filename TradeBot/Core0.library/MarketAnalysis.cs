using Google;
using Quandl_FetchInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;
using Trading.Model;

namespace Core0.library
{

    public static class MarketAnalysis
    {
        public static List<MovingAverageData> List_EMA { get; set; }

        public static SortableBindingList<MarketAnalysisDataumModel> List_MarketAnalysisData { get; set; }

        /// <summary>
        /// Store <tradeVolume , StockName>
        /// </summary>
        /// 
        public static SortedDictionary<double, string> Map_trading_volume { get; set; }


        public static void Start_MarketAnalysis(IProgress<int> progress, string Exchange)
        {
            int period = 90; //days
            int ema_window = 10;
            int nrn_window = 7;

            //if (null == List_MarketAnalysisData)
                List_MarketAnalysisData = new SortableBindingList<MarketAnalysisDataumModel>();
            //else
            //  List_MarketAnalysisData.Clear();


            // Lets find the highest trading volume; in increasing order
            SortedDictionary<string, QHistory> map_TickerQhistory  = Algorithm_SelectIntraDayStocks.Get_TickerQHistory(progress, period, Exchange);
            //Map_trading_volume = Algorithm_SelectIntraDayStocks.Top20Stocks_TV;

            int Total_Stocks_to_Analyse = map_TickerQhistory.Count;

            //if (null == Map_trading_volume)
            //    Map_trading_volume = new SortedDictionary<double, string>();


            /////
            //////In case of re-entry
            //Map_trading_volume.Clear();


            //for (int i = 0; i < (Total_Stocks_to_Analyse/ 2) - 1; ++i )
            foreach( KeyValuePair<string,QHistory> kvp in map_TickerQhistory)
            {

                string ticker_from_tv = kvp.Key;
                // Finding Exponential Moving Average

                MarketAnalysisDataumModel objAnalysisData = Start_MarketAnalysisFor(Exchange, 
                                                                                    ticker_from_tv, 
                                                                                    period, 
                                                                                    ema_window, 
                                                                                    nrn_window, 
                                                                                    kvp.Value.Max_Trading_Volume, 
                                                                                    kvp.Value.Min_Trading_Volume 
                                                                                    ); 
                if (objAnalysisData == null)
                    continue;


                //Fill the local map. Donot pull from Algorithm_SelectIntraDayStocks
                //Map_trading_volume.Add(-1 * objAnalysisData.Volume, ticker_from_tv);

                List_MarketAnalysisData.Add(objAnalysisData);



            }


        }



        public static MarketAnalysisDataumModel Start_MarketAnalysisFor( string Exchange, string name, int period, int window, int nrn_window, double hv90, double lv90 )
        {


            Debug.Assert(period != 0);

            MarketAnalysisDataumModel objAnalysisData = new MarketAnalysisDataumModel();
            objAnalysisData.Ticker = name;
            objAnalysisData.SetExchange(Exchange);

            //Calculate_WeightMulitplier(periods);

            int for_first_sma_period = period + window;
            int start_day_for_expo_ma = period;

            //Calculating past twice of N periods date , we need enough buffered data,but will read only periods
            string sd = DateTime.Now.AddDays((for_first_sma_period * (-2))).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";

            name = (name == "MM" ? "M&M" : name);

            //SortedDictionary<int, StringParsedData> Map_ClosePrice = StringTypeParser.Get_gAPI_MapData(Exchange, name, 86400, for_first_sma_period * 3);
            SortedDictionary<int, StringParsedData> Map_ClosePrice = JsonParser.Get_gAPI_MapStringObject( Exchange, name, 86400, for_first_sma_period*2);
            if (null == Map_ClosePrice)
            {
                return null;
            }

            //GEt high-90 and low-90
            int days = 90;
            float high = 0, low = 99999999;
            foreach(KeyValuePair<int, StringParsedData > data in Map_ClosePrice)
            {
                if (data.Value.Low < low)
                    low = data.Value.Low;
                if (data.Value.High > high)
                    high = data.Value.High;

                if (days-- <= 0)
                    break; ;


            }
            objAnalysisData.HighPrice90 = high;
            objAnalysisData.LowPrice90 = low;

            objAnalysisData.HighVolume90 = hv90;
            objAnalysisData.LowVolume90 = lv90;
            // string ticker_from_tv = name;// Map_trading_volume.Values.ElementAt(i);
            // Finding Exponential Moving Average
            float cp = 0.0f;

            Algorithm_NRN objNRN = new Algorithm_NRN(Map_ClosePrice, Exchange, name, nrn_window, out cp);
            objAnalysisData.IsNRDay = objNRN.bTodayIsNRDay;
            objAnalysisData.Current = cp;

            Algorithm_ExpoMovingAverage objEma = new Algorithm_ExpoMovingAverage(Map_ClosePrice, period, window);
            objAnalysisData.EMA = Formulas.banker_ceil(objEma.EMA);

            Algorithm_SimpleMovingAverage objSma = new Algorithm_SimpleMovingAverage(Map_ClosePrice, period, window);
            objAnalysisData.SMA = Formulas.banker_ceil(objSma.SMA);

            Algorithm_WeightedMovingAverage objWma = new Algorithm_WeightedMovingAverage(Map_ClosePrice, period, window);
            objAnalysisData.WMA = Formulas.banker_ceil(objWma.WMA);


            objAnalysisData.Close = Map_ClosePrice.ElementAt(0).Value.Close;

            objAnalysisData.Volume = Algorithm_SelectIntraDayStocks.Extract_Volume(Map_ClosePrice);

          Algorithm_VolumeWeightMA objVWMA = new Algorithm_VolumeWeightMA(Map_ClosePrice, period, window);
            objAnalysisData.VWMA = objVWMA.VWMA;

            return (objAnalysisData);


        }

    }
}