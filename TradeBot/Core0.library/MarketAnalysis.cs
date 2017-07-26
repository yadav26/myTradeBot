using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{

    public class MarketAnalysisDataum
    {
        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public float LastClose { get; set; }
        public float TodaySMA { get; set; }
        public float TodayEMA { get; set; }
        public int DateDay { get; set; }
        public double Trading_vol_Max { get; set; }
        public double Trading_vol_Min { get; set; }


    }




    public static class MarketAnalysis
    {
        public static List<MovingAverageData> List_EMA { get; set; }
        public static List<MarketAnalysisDataum> List_MarketAnalysisData  { get; set; }
        public static  SortedDictionary<double, string> Map_trading_volume { get; set; }


        public static void Start_MarketAnalysis(string Exchange )
        {
            int period = 90; //days

            List_MarketAnalysisData = new List<MarketAnalysisDataum>();

            List_MarketAnalysisData.Clear();

            // Lets find the highest trading volume; in increasing order
            Algorithm_SelectIntraDayStocks.GetTop20_HighestTradingVolumeStocks(period, Exchange );

            Map_trading_volume = Algorithm_SelectIntraDayStocks.Top20Stocks_TV;
            
            for (int i = 0; i < Map_trading_volume.Count; ++i)
            {
                string ticker_from_tv = Map_trading_volume.Values.ElementAt(i);
                // Finding Exponential Moving Average
                Algorithm_ExpoMovingAverage ptr = new Algorithm_ExpoMovingAverage(Exchange, ticker_from_tv, 90, 10);

                MarketAnalysisDataum objAnalysisData = new MarketAnalysisDataum();
                objAnalysisData.Ticker = ticker_from_tv;
                objAnalysisData.Exchange = Exchange;
                objAnalysisData.Trading_vol_Max = Map_trading_volume.Keys.ElementAt(i);
                
                List_EMA = ptr.Exponent_List;

                objAnalysisData.TodaySMA = List_EMA[List_EMA.Count-1].TodaySMA; //fixed bug , last of list is latest day
                objAnalysisData.TodayEMA = List_EMA[List_EMA.Count - 1].TodayEMA;
                objAnalysisData.DateDay = List_EMA[List_EMA.Count - 1].DateDay;
                objAnalysisData.LastClose = List_EMA[List_EMA.Count - 1].LastClose;

                List_MarketAnalysisData.Add(objAnalysisData);
            }


        }
    }
}
