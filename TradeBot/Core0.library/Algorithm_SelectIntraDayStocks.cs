using Quandl_FetchInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google;
using Trading.Model;

namespace Core0.library
{
    //Trade liquid stocks
    //Avoid unpredictable(chaotic) stocks
    //Trade stocks with good correlation
    //Move with the trend
    //Research
    //200 - days moving average
    //NR7 - NR7 is term given to a day that has the daily range smallest of last 7 days including that day.
    //Trading volume   In order to pick Stocks for Intraday Trading you can compare the previous day’s volume 
    //of stock with the average volume of last 10 days.  
    //If you observe that the volume is rising, then that stock is a perfect Intraday candidate.

    public class Algorithm_SelectIntraDayStocks
    {


        //public static List<QHistoryDatum> t_history_list { get; set; }// = new List<QHistoryDatum>();

        public static SortedDictionary< string, QHistory > MapTickerSortedVolume = null;

        public static SortedDictionary<string, QHistory> Get_TickerQHistory(IProgress<int> progress, int period, string exchange )
        {
            if(null == MapTickerSortedVolume)
                MapTickerSortedVolume = new SortedDictionary<string, QHistory>();
            else
                MapTickerSortedVolume.Clear();

            TickerList tickMap = new TickerList();
            List <string> TickerList = tickMap.TickerCollection.Values.ToList();


            foreach(string ticker in TickerList )
            {

                QHistory hsNewObj = Get_TradingVolumeFor(period, exchange, ticker);

                if (null == hsNewObj)
                    continue;

                //double Avergae_tv = (hsNewObj.Min_Trading_Volume + hsNewObj.Max_Trading_Volume) / 2;

                MapTickerSortedVolume.Add(  ticker, hsNewObj );


            }

            return MapTickerSortedVolume;
        }//end of GetTop20_HighestTradingVolumeStocks


        public static QHistory Get_TradingVolumeFor(int period, string exchange, string name)
        {

            string sd = DateTime.Now.AddDays(-1 * period).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;

            string ticker = name;// list_of_nse[threadCnt * 2 + 1].Trim();

            QHistory hsNewObj = new QHistory(exchange, ticker, sd, ed);

            if (0 == hsNewObj.getHistoryCount())
                return null;

            return hsNewObj;
        }//end of GetTop20_HighestTradingVolumeStocks


        public static double Extract_Volume (SortedDictionary<int, StringParsedData> map )
        {
            double highest_volume = -1;
            double lowest_volume = 10000000000;

            foreach( KeyValuePair<int, StringParsedData> kvp in map )
            {
                double volume = kvp.Value.Volume;
                if (volume > highest_volume)
                    highest_volume = volume;

                if (volume < lowest_volume)
                    lowest_volume = volume;
            }

            return map.ElementAt(0).Value.Volume;

        }
    } //end of class


}
