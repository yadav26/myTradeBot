using Quandl_FetchInterface;
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
        public static List<MarketAnalysisDataum> List_MarketAnalysisData { get; set; }

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

            if (null == List_MarketAnalysisData)
                List_MarketAnalysisData = new List<MarketAnalysisDataum>();
            else
                List_MarketAnalysisData.Clear();

            
            // Lets find the highest trading volume; in increasing order
            Algorithm_SelectIntraDayStocks.GetTop20_HighestTradingVolumeStocks(period, Exchange);
            //Map_trading_volume = Algorithm_SelectIntraDayStocks.Top20Stocks_TV;

            int Total_Stocks_to_Analyse = Algorithm_SelectIntraDayStocks.list_of_nse.Length;

            if (null == Map_trading_volume)
                Map_trading_volume = new SortedDictionary<double, string>();

            for (int i = 0; i < (Total_Stocks_to_Analyse/ 2) - 1; ++i )
            {
                
                string ticker_from_tv = Algorithm_SelectIntraDayStocks.list_of_nse[(i * 2) + 1].Trim();
                // Finding Exponential Moving Average

                MarketAnalysisDataum objAnalysisData = Start_MarketAnalysisFor(Exchange, ticker_from_tv, period, ema_window, nrn_window); ////new MarketAnalysisDataum();
                if (objAnalysisData == null)
                    continue;


                //Fill the local map. Donot pull from Algorithm_SelectIntraDayStocks
                Map_trading_volume.Add(-1 * objAnalysisData.Volume, ticker_from_tv);

                List_MarketAnalysisData.Add(objAnalysisData);

                if (progress != null)
                    progress.Report((2*i ) * 100 / Total_Stocks_to_Analyse);


            }


        }



        public static MarketAnalysisDataum Start_MarketAnalysisFor( string Exchange, string name, int period, int window, int nrn_window )
        {
            
            string ticker_from_tv = name;// Map_trading_volume.Values.ElementAt(i);
                                         // Finding Exponential Moving Average

            

            Algorithm_NRN objNRN = new Algorithm_NRN(Exchange, ticker_from_tv, nrn_window);

            Algorithm_ExpoMovingAverage ptr = new Algorithm_ExpoMovingAverage(Exchange, ticker_from_tv, period, window);
            List_EMA = ptr.Exponent_List;

            if (List_EMA.Count == 0)
                return null;

            MarketAnalysisDataum objAnalysisData = new MarketAnalysisDataum();
            objAnalysisData.Ticker = ticker_from_tv;
            objAnalysisData.SetExchange(Exchange);

            QHistory qhs = Algorithm_SelectIntraDayStocks.Get_TradingVolumeFor(period, Exchange, ticker_from_tv);

            objAnalysisData.Volume = (qhs== null ? 0 : qhs.Tade_Volume );
    

            //objAnalysisData.Volume = qhs.Tade_Volume;

            
            objAnalysisData.SMA = List_EMA[List_EMA.Count - 1].TodaySMA; //fixed bug , last of list is latest day
            objAnalysisData.EMA = List_EMA[List_EMA.Count - 1].TodayEMA;
            //objAnalysisData.DateDay = List_EMA[List_EMA.Count - 1].DateDay;
            objAnalysisData.Close = List_EMA[List_EMA.Count - 1].LastClose;
            objAnalysisData.IsNRDay = objNRN.bTodayIsNRDay;

            return (objAnalysisData);


        }

    }
}