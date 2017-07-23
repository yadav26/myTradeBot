using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core0.library
{

    public class ThreadManager
    {
        static int TIME_OUT_INTERVAL = 1000;
        static int MAX_THREAD_COUNT = 200;

        static ThreadStart childref = new ThreadStart(CallToChildThread);
        static Thread[] Trade_status_threads = new Thread[MAX_THREAD_COUNT];

        static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";


        static void CallToChildThread()
        {
            int start_at = DateTime.Now.Millisecond;
            int count = 0;


            //string exchange = "NASDAQ";
            //string ticker = "AMD";

            string exchange = "NSE";
            //string ticker = "ITC";

            string ticker = "SBIN"; //Thread.CurrentThread.Name;


            bool bIsPurchased = false;
            float fetched_price = 0.0f;




            //int WAIT_LOSS_COUNTER = 20;

            string api_fetch_add = finance_google_url + exchange + ":" + ticker;

            //Calculating dates of past three months interval
            string sd = DateTime.Now.AddDays(-90).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";

            //Algorithm_MinProfit algo = new Algorithm_MinProfit();
            //algo.Warm_up_time(exchange, ticker, sd, ed);

            Algorithm_GreedyPeek algo_gp = new Algorithm_GreedyPeek();
            algo_gp.Warm_up_time(exchange, ticker, sd, ed);
            //float tomin = algo.getMinPrice();
            //float tomax = algo.getMaxPrice();
            //float tomean = algo.getMeanPrice();
            //float hsmin = algo.getHsMinPrice();
            //float hsmax = algo.getHsMaxPrice();
            //float hsmean = algo.getHsMeanPrice();

            //Console.WriteLine("\n------------------------STATISTICS.");
            //Console.WriteLine(ticker);
            //Console.WriteLine("Start :"+ sd +", End :"+ed);
            //Console.WriteLine(string.Format("Today Least:{0:0.00##}", tomin));
            //Console.WriteLine(string.Format("Today Maxim:{0:0.00##}", tomax));
            //Console.WriteLine(string.Format("Today Mean :{0:0.00##}", tomean));
            //Console.WriteLine(string.Format("History Least:{0:0.00##}", hsmin));
            //Console.WriteLine(string.Format("History Maxim:{0:0.00##}", hsmax));
            //Console.WriteLine(string.Format("History Mean :{0:0.00##}", hsmean));
            //Console.WriteLine("------------------------ END.\n");

            using (WebClient wc = new WebClient())
            {
                while (count++ < TIME_OUT_INTERVAL) // cannot stuck at forever; after this count over we will sale it @ 1% loss
                {


                    try // THREAD TRY block
                    {
                        String jSonStr = string.Empty;

                        // do any background work
                        try
                        {

                            jSonStr = wc.DownloadString(api_fetch_add);
                            jSonStr = Regex.Replace(jSonStr, @"\t|\n|\r|//|\[|\]|\ ", "").Trim();

                            fetched_price = Class1.getCurrentTradePrice(jSonStr);

                            Console.WriteLine(string.Format("Fetched  {0}:{1:0.00##}", ticker, fetched_price));

                            algo_gp.GreedyPeek_Strategy_Execute(fetched_price, 100);


                        }
                        catch (WebException ex)
                        {
                            if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                            {
                                var resp = (HttpWebResponse)ex.Response;
                                if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                                {
                                    //Handle it
                                    Console.WriteLine("End resp.StatusCode ==>" + api_fetch_add);
                                }
                            }
                            //Handle it
                            return;
                        }
                        Thread.Sleep(5000);

                        //Console.WriteLine(lstBusinessModel.Count);
                    }// THREAD TRY block


                    catch (ThreadAbortException e)
                    {
                        Console.WriteLine("Thread Abort Exception Err :" + e.ToString());
                    }

                }
            }

            int end_at = DateTime.Now.Millisecond;
            Console.WriteLine("Time spent in thread for trade surge = " + (end_at - start_at));

            // Fixed bug ..if timeout occur and stock did liquidate , call explicitly to liquidate.
            if (bIsPurchased)
            {
                // if we are here means, that stock failed to liquidate after 500 seconds
                // price didnt touch to BE or may be running in loss tolerance limit.

                /// lets exit from this
                /// 
                //SALE_ALL_STOCKS(fetched_price);
                //{
                //    float zerTax = Class1.getZerodha_Deductions(recent_purchased_price, fetched_price, total_units_purchased);

                //    float curr_trade_profit = ((fetched_price - recent_purchased_price) * total_units_purchased) - zerTax;

                //    gross_profit_made += curr_trade_profit;
                //    Console.WriteLine("------------------------TRADE stats.");
                //    Console.WriteLine(string.Format("Purcased:{0:0.00##}", recent_purchased_price));
                //    Console.WriteLine(string.Format("SOLD at :{0:0.00##}", fetched_price));
                //    Console.WriteLine(string.Format("Tax paid:{0:0.00##}", zerTax));
                //    Console.WriteLine(string.Format("Net P/L :{0:0.00##}", curr_trade_profit));
                //    Console.WriteLine(string.Format("====Gross P/L:{0:0.00##}", gross_profit_made));
                //    Console.WriteLine("------------------------ END.");

                //}

            }
        }


        public static void LaunchTradingThread(string name, int numbers, int index)
        {
            Trade_status_threads[index] = new Thread(childref);
            Trade_status_threads[index].Name = name;
            Trade_status_threads[index].Start();
        }


        public static void ExitTradingThread(int index)
        {
            if (null != Trade_status_threads[index])
            {
                Trade_status_threads[index].Abort();
                Trade_status_threads[index] = null;
            }
        }

        public static void CleanUpAllThreads( )
        {
            foreach (Thread t in Trade_status_threads)
                if( t != null )
                    t.Abort();
        }
    }
}
