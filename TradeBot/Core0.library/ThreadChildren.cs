using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Trading.DAL;

namespace Core0.library
{
    public class ThreadChildren
    {

        static int TIME_OUT_INTERVAL = 1000;
        static int MAX_THREAD_COUNT = 200;

        //static ThreadStart childrefTrending = new ThreadStart(CallToChildTrendingThread);
        static Thread[] Trade_status_threads = new Thread[MAX_THREAD_COUNT];
        static Thread Trending_chart_threads = null;
        static Thread MarketAnalysis_threads = null;
        static Thread MarketAnalysis_Workerthread = null;

        static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";
        
        public delegate void PriceUpdater(Scanner scObj);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="updater"></param>
        /// <param name="scanRowID"></param>
        /// <param name="exchange"></param>
        /// <param name="ticker"></param>
        public void CallToScannerThread(Func<Scanner, int> ScanUpdatePrice, int scanRowID, string exchange, string ticker)
        {
            //PriceUpdater ScanUpdatePrice = (PriceUpdater)updater;
            int start_at = DateTime.Now.Millisecond;
            int count = 0;


            bool bIsPurchased = false;
            float fetched_price = 0.0f;


            //int WAIT_LOSS_COUNTER = 20;

            string api_fetch_add = finance_google_url + exchange + ":" + ticker;

            //Calculating dates of past three months interval
            string sd = DateTime.Now.AddDays(-90).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";

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

                            fetched_price = Formulas.getCurrentTradePrice(jSonStr);

                            Core0.library.MarketAnalysisDataum objAnalysis = MarketAnalysis.Start_MarketAnalysisFor(exchange, ticker, 90, 10, 7);

                            Scanner scObj = new Scanner(scanRowID, ticker, objAnalysis.IsNRDay, objAnalysis.EMA, objAnalysis.SMA, objAnalysis.Close, objAnalysis.Volume, fetched_price, false);

                            ScanUpdatePrice(scObj);

                            Console.WriteLine(string.Format("Fetched  {0}:{1:0.00##}", ticker, fetched_price));

                            

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


        }

    }
}
