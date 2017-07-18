using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Quandl_FetchInterface;
using Google;

namespace Core0.library
{
    // This algorithm is about to run the bot @ minimum profit 
    // i.e. 1.5% of investment above break-even cost.
    // so we will poll for the minimum threshold price and once attained, will liquidate all assests.

    // Stop loss TOLERANCE is @ 1% including BE , i.e. purchase cost * 0.99 ; 
    public class Algorithm_MinProfit
    {
        

        public static Tuple<float, float, float, float,float, float> Warm_up_time(string exch, string ticker, string sd, string ed)
        {
            List<GHistoryDatum> gt_history_list = new List<GHistoryDatum>();
            Console.WriteLine("WARMIN UP STARTED .......");


            Daily_Reader todayReader = new Daily_Reader();
            todayReader.parser(exch, ticker);

            float today_min = todayReader.TodayMin;
            float today_max = todayReader.TodayMax;
            float today_mean = todayReader.TodayMean;

            
            List<HistoryDatum> t_history_list = new List<HistoryDatum>();
            History hsNewObj = new History(exch, ticker, sd, ed);
            //Console.WriteLine("Fetched history STARTED .......");
            if (hsNewObj.getHistoryCount() == 0)
                return Tuple.Create(0.0f,0.0f,0.0f,0.0f,0.0f,0.0f);


            float history_lowest_price = Class1.banker_ceil(hsNewObj.Min); 
            float history_highest_price = Class1.banker_ceil(hsNewObj.Max); 
            float history_mean_closing_price = Class1.banker_ceil(hsNewObj.Mean);
            //get last 6 months history 2 quarter results
            //calculate highest
            //calculate lowest
            //calculate average / mean
            /*
                        float[] position = new float[100];

                        string api_fetch_string = @"http://finance.google.co.uk/finance/info?client=ig&q=NSE:SBIN";

                        float last_position = 0.0f;

                        using (WebClient wc = new WebClient())
                        {

                            int count = 0;

                            while (count < 100) // fetch 100 non-repeatitive reads for analysis
                            {
                                Console.WriteLine("FETCH {0} .......", count);
                                try
                                {
                                    // do some work, like counting to 10
                                    // for (int counter = 0; counter <= 10; counter++)
                                    String jSonStr = string.Empty;

                                    //string symbol = Thread.CurrentThread.Name;

                                    //string api_fetch_string = @"http://finance.google.co.uk/finance/info?client=ig&q=NSE:" + symbol;//ConfigurationManager.AppSettings[AppConstant.NSEURL].ToString() + symbol;
                                    // Console.WriteLine("Start = " + api_fetch_string);
                                    // do any background work
                                    try
                                    {
                                        // JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                                        jSonStr = wc.DownloadString(api_fetch_string);

                                        jSonStr = Regex.Replace(jSonStr, @"\t|\n|\r|//|\[|\]|\ ", "").Trim();

                                        float new_position = Class1.getCurrentTradePrice(jSonStr);
                                        if (last_position == new_position)
                                        {
                                            Thread.Sleep(5000);
                                            continue;
                                        }


                                        position[count] = new_position;
                                        count++;


                                        last_position = new_position;

                                    }
                                    catch (WebException ex)
                                    {
                                        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                                        {
                                            var resp = (HttpWebResponse)ex.Response;
                                            if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                                            {
                                                //Handle it
                                                Console.WriteLine("End resp.StatusCode ==>" + api_fetch_string);
                                            }
                                        }
                                        //Handle it
                                        return 0;
                                    }
                                    Thread.Sleep(5000);
                                }
                                catch (ThreadAbortException e)
                                {
                                    Console.WriteLine("Thread Abort Exception");
                                }

                            } // end of counter
                        }


                        for (int x=0;x<100;x++)
                            Console.WriteLine(string.Format("Position[0]:{1:0.00##}", x, position[x]));
                        */

            //generate_statistics( );

            return Tuple.Create(today_min, today_max, today_mean, history_lowest_price, history_highest_price, history_mean_closing_price);


        }

        //-------
        public static Tuple<float,float,float,float>generate_statistics( float price )
        {
            //**** already fixed and calculations in Warm_up_time******

            //get This year least price
            //Fetch history -> create db -> get min price
            //get one year max price
            //Fetch history -> create db -> get max price

            //get today open price
            //get today current priceflo
            //calc be
            // set min profit price
            //

            float stop_loss = price - (price * 0.01f);
            float be = Class1.getBreakEvenPrice(price);
            float target = be + (price * 0.015f);
            float lpet = be * 1.0001f;

            return Tuple.Create(stop_loss,be,target,lpet);
        }


    }
}
