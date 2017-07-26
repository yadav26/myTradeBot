﻿using Google;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        //static ThreadStart childrefTrending = new ThreadStart(CallToChildTrendingThread);
        static Thread[] Trade_status_threads = new Thread[MAX_THREAD_COUNT];
        static Thread Trending_chart_threads = null;
        static Thread MarketAnalysis_threads = null;
        static Thread MarketAnalysis_Workerthread = null;
        
        static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";


        static void CallToChildThread( )
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

            int input_algo = 2;
            Algorithm_GreedyPeek algo_gp = null;
            Algorithm_MinProfit algo = null;

            if (input_algo == 1)
            {
                algo = new Algorithm_MinProfit();
                algo.Warm_up_time(exchange, ticker, sd, ed);
            }else if (input_algo == 2)
            { 
                algo_gp = new Algorithm_GreedyPeek();
                algo_gp.Warm_up_time(exchange, ticker, sd, ed);
            }
            else if (input_algo == 3)
            {

            }
            else
            {

            }
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
                //Console.WriteLine(string.Format("QHistory Least:{0:0.00##}", hsmin));
                //Console.WriteLine(string.Format("QHistory Maxim:{0:0.00##}", hsmax));
                //Console.WriteLine(string.Format("QHistory Mean :{0:0.00##}", hsmean));
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

                                fetched_price = Formulas.getCurrentTradePrice(jSonStr);

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


        static void CallToChildTrendingThread( string ticker )
        {
            string exch = "NSE";
            int interval = 60;
            //int start_at = DateTime.Now.Millisecond;

            Daily_Reader todayReader1 = new Daily_Reader();
            todayReader1.parser(exch, ticker, interval, "1d"); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

            List <GHistoryDatum> ghs1 = todayReader1.GetGHistoryList();

            Debug.Assert(ghs1 != null);

            List<double> sr1 = new List<double>();

            foreach (GHistoryDatum dataum in ghs1)
                sr1.Add( double.Parse(dataum.Open) );

            todayReader1.Flush_HistoryList();

            //////////////////////////////////////////////////////////////////////
            Daily_Reader todayReader2 = new Daily_Reader();
            todayReader2.parser(exch, "NIFTY", interval, "1d"); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

            List<GHistoryDatum> ghs2 = todayReader2.GetGHistoryList();
            List<double> sr2 = new List<double>();

            foreach (GHistoryDatum dataum in ghs2)
                sr2.Add(double.Parse(dataum.Open));

            todayReader2.Flush_HistoryList();

            DataForChartRender.Series1 = null;
            DataForChartRender.Series2 = null;

            DataForChartRender.Series1 = sr1;
            DataForChartRender.Series2 = sr2;

        }

        public static void childWorkerMarketAnalysis()
        {

            MarketAnalysis.Start_MarketAnalysis();
            return;
        }



        public static void ChildMarketAnalysisThread( string str )
        {
            ThreadManager.LaunchChildMarketAnalysisThread();

            return;
        }
        public static Thread LaunchChildMarketAnalysisThread()
        {
            MarketAnalysis_Workerthread = new Thread(childWorkerMarketAnalysis);
            //Trade_status_threads[index].Name = name;
            MarketAnalysis_Workerthread.Start();
            return MarketAnalysis_Workerthread;
        }

        public static Thread LaunchTradingThread(string name, int numbers, int index)
        {
            Trade_status_threads[index] = new Thread(childref);
            Trade_status_threads[index].Name = name;
            Trade_status_threads[index].Start();
            return Trade_status_threads[index];
        }

        public static Thread LaunchTrendingChartThread(string name, int index)
        {
            Trending_chart_threads = new Thread(() => CallToChildTrendingThread(name));
            Trending_chart_threads.Name = name;
            Trending_chart_threads.Start();
            return Trending_chart_threads;
        }

        public static Thread LaunchMarketAnalysisThread(string name, int index)
        {
            MarketAnalysis_threads = new Thread(() => ChildMarketAnalysisThread(name));
            //Trending_chart_threads.Name = name;
            MarketAnalysis_threads.Start();
            return MarketAnalysis_threads;
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