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
        public bool IsNRDay { get; set; }
        public float EMA { get; set; }
        public float SMA { get; set; }
        public float Close { get; set; }

        public double Volume { get; set; }
        double Trading_vol_Min { get; set; }
        int DateDay { get; set; }
        string Exchange { get; set; }

        public void SetExchange(string s) { Exchange = s; }
    }




    public static class MarketAnalysis
    {
        public static List<MovingAverageData> List_EMA { get; set; }
        public static List<MarketAnalysisDataum> List_MarketAnalysisData  { get; set; }
        public static  SortedDictionary<double, string> Map_trading_volume { get; set; }


        public static void Start_MarketAnalysis(IProgress<int> progress, string Exchange )
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

                Algorithm_NRN objNRN = new Algorithm_NRN(Exchange, ticker_from_tv, 7 );

                

                Algorithm_ExpoMovingAverage ptr = new Algorithm_ExpoMovingAverage(Exchange, ticker_from_tv, 90, 10);

                MarketAnalysisDataum objAnalysisData = new MarketAnalysisDataum();
                objAnalysisData.Ticker = ticker_from_tv;
                objAnalysisData.SetExchange(Exchange);
                objAnalysisData.Volume = Map_trading_volume.Keys.ElementAt(i);
                
                List_EMA = ptr.Exponent_List;

                if (List_EMA.Count == 0)
                    continue;

                objAnalysisData.SMA = List_EMA[List_EMA.Count-1].TodaySMA; //fixed bug , last of list is latest day
                objAnalysisData.EMA = List_EMA[List_EMA.Count - 1].TodayEMA;
                //objAnalysisData.DateDay = List_EMA[List_EMA.Count - 1].DateDay;
                objAnalysisData.Close = List_EMA[List_EMA.Count - 1].LastClose;
                objAnalysisData.IsNRDay = objNRN.bTodayIsNRDay;

                List_MarketAnalysisData.Add(objAnalysisData);


                if (progress != null)
                    progress.Report((i + 2) * 100 / Map_trading_volume.Count);


            }


        }
    }
}
