using Google;
using Quandl_FetchInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculator;
using Trading.DAL;
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



        public static SortableBindingList<MarketAnalysisDataumModel> Start_MarketAnalysis(IProgress<int> progress, string Exchange)
        {
            int period = 90; //days
            int ema_window = 10;
            int nrn_window = 7;

            List_MarketAnalysisData = new SortableBindingList<MarketAnalysisDataumModel>();

            //FIRST TIME READ 
            if (false == File.Exists(@"MA.txt") ) // no Quick backed up data dump raw json file , go read from internet.
            {
                TickerList tc = new TickerList();
                
                foreach (string ticker in tc.TickerCollection.Values.ToList())
                {

                    MarketAnalysisDataumModel objAnalysisData = Start_MarketAnalysisFor(Exchange,
                                                                                        ticker,
                                                                                        period,
                                                                                        ema_window,
                                                                                        nrn_window,
                                                                                        0,
                                                                                        0
                                                                                        );
                    if (objAnalysisData == null)
                        continue;
                    
                    List_MarketAnalysisData.Add(objAnalysisData);

                    Thread.Sleep(4000);
                    
                }

            }
            else
            {

                string readRawString = FileWriteData.ReadFullFileToJsonFormattedString(@"MA.txt");

                string[] strarray = readRawString.Split(new[] { "{\"dataset\":" }, StringSplitOptions.RemoveEmptyEntries);


                foreach ( string inputRawJson in strarray )
                {
                    
                    string[] strarray1 = inputRawJson.Split(new[] { "\"data\"" }, StringSplitOptions.None);

                    string[] header_entities = JsonParser.getCleanHeaderString(strarray[0]);

                    List<StringParsedData> ls_temp = JsonParser.Get_ListStringDataObjectFromFile(Exchange, inputRawJson);
                    // = new SortedDictionary<int, StringParsedData>();

                    SortedDictionary<int, StringParsedData> Map_ClosePrice1 = new SortedDictionary<int, StringParsedData>();

                    foreach (StringParsedData spd in ls_temp)
                    {
                        Map_ClosePrice1.Add(spd.DateDay, spd);
                    }
                    
                    MarketAnalysisDataumModel objAnalysisData = Calculate_MA_For(Exchange, ls_temp[0].Ticker, period, ema_window, nrn_window,0,0, Map_ClosePrice1);
                    objAnalysisData.Ticker = ls_temp[0].Ticker;

                    List_MarketAnalysisData.Add(objAnalysisData);

                    
                }

                

            }


            return List_MarketAnalysisData;
        }


        public static MarketAnalysisDataumModel Calculate_MA_For(string Exchange, 
                                                                    string name, 
                                                                    int period, 
                                                                    int window, 
                                                                    int nrn_window, 
                                                                    double hv90, 
                                                                    double lv90, 
                                                                    SortedDictionary<int, StringParsedData> Map_ClosePrice)
        {

            //GEt high-90 and low-90
            int days = 90;
            float high = 0, low = 99999999;
            foreach (KeyValuePair<int, StringParsedData> data in Map_ClosePrice)
            {
                if (data.Value.Low < low)
                    low = data.Value.Low;
                if (data.Value.High > high)
                    high = data.Value.High;

                if (days-- <= 0)
                    break; ;


            }


            MarketAnalysisDataumModel objAnalysisData = new MarketAnalysisDataumModel();
            objAnalysisData.Ticker = name;
            objAnalysisData.SetExchange(Exchange);

            objAnalysisData.HighPrice90 = high;
            objAnalysisData.LowPrice90 = low;

            objAnalysisData.HighVolume90 = hv90;
            objAnalysisData.LowVolume90 = lv90;
            // string ticker_from_tv = name;// Map_trading_volume.Values.ElementAt(i);
            // Finding Exponential Moving Average
            float cp = 0.0f;

            Algorithm_NRN objNRN = new Algorithm_NRN(Map_ClosePrice, Exchange, name, nrn_window, out cp);
            objAnalysisData.IsNRDay = objNRN.bTodayIsNRDay;
            objAnalysisData.CurrentPrice = cp;

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


        public static MarketAnalysisDataumModel Start_MarketAnalysisFor( string Exchange, string name, int period, int window, int nrn_window, double hv90, double lv90 )
        {


            Debug.Assert(period != 0);
            SortedDictionary<int, StringParsedData> Map_ClosePrice = null;

            int for_first_sma_period = period + window;
                int start_day_for_expo_ma = period;

                //Calculating past twice of N periods date , we need enough buffered data,but will read only periods
                string sd = DateTime.Now.AddDays((for_first_sma_period * (-2))).ToString("yyyy-M-d");
                string ed = DateTime.Now.ToString("yyyy-M-d"); //"2017-07-14";

                name = (name == "MM" ? "M&M" : name);

                //SortedDictionary<int, StringParsedData> Map_ClosePrice = StringTypeParser.Get_gAPI_MapData(Exchange, name, 86400, for_first_sma_period * 3);
                Map_ClosePrice = JsonParser.Get_gAPI_MapStringObject(Exchange, name, 86400, for_first_sma_period * 2);
                if (null == Map_ClosePrice)
                {
                    return null;
                }



                return Calculate_MA_For(Exchange, name, period, window, nrn_window, hv90, lv90, Map_ClosePrice);


        }

    }
}