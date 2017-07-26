using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    public static class MarketAnalysis
    {
        public static List<MovingAverageData> MapEMA { get; set; }
        public static  SortedDictionary<double, string> Map_trading_volume { get; set; }


        public static void Start_MarketAnalysis()
        {
            int period = 90; //days

            // Lets find the highest trading volume; in increasing order
            Algorithm_SelectIntraDayStocks.GetTop20_HighestTradingVolumeStocks(period);

            Map_trading_volume = Algorithm_SelectIntraDayStocks.Top20Stocks_TV;

            for (int i = 0; i < Map_trading_volume.Count; ++i)
            {
                string ticker_from_tv = Map_trading_volume.Values.ElementAt(i);
                // Finding Exponential Moving Average
                Algorithm_ExpoMovingAverage ptr = new Algorithm_ExpoMovingAverage("NSE", ticker_from_tv, 90, 10);

                MapEMA = ptr.Exponent_List;

            }


        }
    }
}
