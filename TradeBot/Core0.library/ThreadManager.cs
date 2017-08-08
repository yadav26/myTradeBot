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
using System.Windows.Forms;
using Trading.DAL;

namespace Core0.library
{
    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        DialogResult _result;
        DialogResult _timerResult;
        AutoClosingMessageBox(string text, string caption, int timeout, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult timerResult = DialogResult.None)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            _timerResult = timerResult;
            using (_timeoutTimer)
                _result = MessageBox.Show(text, caption, buttons);
        }
        public static DialogResult Show(string text, string caption, int timeout, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult timerResult = DialogResult.None)
        {
            return new AutoClosingMessageBox(text, caption, timeout, buttons, timerResult)._result;
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
            _result = _timerResult;
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }




    public class ThreadManager
    {
        static int TIME_OUT_INTERVAL = 1000;
        static int MAX_THREAD_COUNT = 200;

        public delegate void PriceUpdater(Scanner scObj);

        public delegate void PollUpdater(Dictionary<string, float> scobj);

        //static ThreadStart childrefTrending = new ThreadStart(CallToChildTrendingThread);
        static Thread[] Trade_status_threads = new Thread[MAX_THREAD_COUNT];
        static Thread Trending_chart_threads = null;
        static Thread MarketAnalysis_threads = null;
        static Thread PollthreadHandle = null;
        static Thread MarketAnalysis_Workerthread = null;
        
        static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";

        public static SortableBindingList<MarketAnalysisDataum> ls_marketData = null;

        public List<Thread> List_ChildThreadCol = new List<Thread>();
        public static SortedDictionary<string, Thread> Map_ChildScannerThreadCol = new SortedDictionary<string, Thread>();
        public static SortedDictionary<string, Thread> Map_ChildTradingThreadCol = new SortedDictionary<string, Thread>();


        static void CallToChildTrendingThread( int order_id )
        {
            // Read from DB -------> ORDERID
            //string ticker = Order.Ticker;
            //string exch = Order.Exchange;
            int interval = 60;
            //int start_at = DateTime.Now.Millisecond;
            string exch = "NSE";
            string ticker = "SBIN";
            Daily_Reader todayReader1 = new Daily_Reader();

            todayReader1.parser(exch, ticker, interval, 1); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

            List <StringParsedData> ghs1 = todayReader1.GetGHistoryList();
            List<double> sr1 = new List<double>();
            List<double> sr2 = new List<double>();

            if (ghs1 == null)
            {
                sr1 = null;
                sr2 = null;
                Debug.Assert(ghs1 != null);
                return;
            }
            


            foreach (StringParsedData dataum in ghs1)
                sr1.Add( dataum.Open );

            todayReader1.Flush_HistoryList();

            //////////////////////////////////////////////////////////////////////
            Daily_Reader todayReader2 = new Daily_Reader();
            todayReader2.parser(exch, "NIFTY", interval, 1); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

            List<StringParsedData> ghs2 = todayReader2.GetGHistoryList();


            foreach (StringParsedData dataum in ghs2)
                sr2.Add(dataum.Open);

            todayReader2.Flush_HistoryList();

            DataForChartRender.Series1 = null;
            DataForChartRender.Series2 = null;

            DataForChartRender.Series1 = sr1;
            DataForChartRender.Series2 = sr2;

        }

        public static SortableBindingList<MarketAnalysisDataum> childWorkerMarketAnalysis(IProgress<int> progress, string exch )
        {

            MarketAnalysis.Start_MarketAnalysis(progress, exch);
            
            return MarketAnalysis.List_MarketAnalysisData;
        }



        public static void ChildMarketAnalysisThread(IProgress<int> progress, string str )
        {
            ThreadManager.LaunchChildMarketAnalysisThread(progress, str);

            return;
        }
        
        public static SortableBindingList<MarketAnalysisDataum> LaunchChildMarketAnalysisThread(IProgress<int> progress, string exchange )
        {
            //string exchange = "NSE";
            if (ls_marketData == null)
                ls_marketData = new SortableBindingList<MarketAnalysisDataum>();
            
            ls_marketData.Clear();

            MarketAnalysis_Workerthread = new Thread(() => { ls_marketData = childWorkerMarketAnalysis(progress, exchange); });

            //Trade_status_threads[index].Name = name;
            MarketAnalysis_Workerthread.Start();

            MarketAnalysis_Workerthread.Join();

            return ls_marketData;
        }

        //public static Thread LaunchTradingThread(string name, int numbers, int index, object updater, string exchange, string ticker )
        //{
        //    //Trade_status_threads[index] = new Thread(new ParameterizedThreadStart(CallToChildThread));
        //    Trade_status_threads[index] = new Thread(() => {  CallToChildThread( updater, index, exchange, ticker); });
        //    //Thread th = new Thread(new ParameterizedThreadStart(CallToChildThread));
        //    //Trade_status_threads[index].Name = name;
        //    Trade_status_threads[index].Start();
        //    return Trade_status_threads[index];
        //}

        public static void TerminateAllScannerThread()
        {
            foreach(KeyValuePair <string, Thread> kvp in  Map_ChildScannerThreadCol)
            {
                kvp.Value.Abort();
            }
        }

        public static void TerminateAllTradingThread()
        {
            foreach (KeyValuePair<string, Thread> kvp in Map_ChildTradingThreadCol)
            {
                kvp.Value.Abort();
            }
        }


        
        public static void LaunchScannerThread(string name, int numbers, int index, Func<Scanner, int> updater, string exchange)
        {
            if(Map_ChildScannerThreadCol.Count == 0 )
            {
                ThreadChildren ths = new ThreadChildren();
                Thread ChildThread = new Thread(() => { ths.CallToScannerThread(updater, index, exchange, name); });
                Map_ChildScannerThreadCol.Add(name, ChildThread);
                ChildThread.Start();
            }
            else
            {
                bool bIsAlreadyScan = false;
                foreach (System.Collections.Generic.KeyValuePair<string, Thread> kvp in Map_ChildScannerThreadCol)
                {
                    if (kvp.Key == name)
                    {
                        MessageBox.Show("This stock is already under scanner.",
                                        "FYI",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                        bIsAlreadyScan = true;
                    }

                }

                if(bIsAlreadyScan == false)
                {
                    ThreadChildren ths = new ThreadChildren();
                    Thread ChildThread = new Thread(() => { ths.CallToScannerThread(updater, index, exchange, name); });
                    Map_ChildScannerThreadCol.Add(name, ChildThread);
                    ChildThread.Start();
                }
            }
            
            //Trade_status_threads[index] = new Thread(new ParameterizedThreadStart(CallToChildThread));
            // Trade_status_threads[index] = new Thread(() => { CallToScannerThread(updater, index, exchange, ticker); });
            //Thread th = new Thread(new ParameterizedThreadStart(CallToScannerThread));
            //Trade_status_threads[index].Name = name;

            return ;
        }

        
        public static void LaunchTradingThread(string name, int numbers, int OrderID, Func<CurrentOrderUpdater, int> updater, string exchange)
        {
            //Trade_status_threads[index] = new Thread(new ParameterizedThreadStart(CallToChildThread));

            if (Map_ChildTradingThreadCol.Count == 0)
            {
                ThreadChildren ths = new ThreadChildren();
                Thread ChildThread = new Thread(() => { ths.CallToTradingThread(updater, OrderID, exchange, name); });
                Map_ChildTradingThreadCol.Add(name, ChildThread);
                ChildThread.Start();
            }
            else
            {
                bool bIsAlreadyScan = false;
                foreach (System.Collections.Generic.KeyValuePair<string, Thread> kvp in Map_ChildTradingThreadCol)
                {
                    if (kvp.Key == name)
                    {
                        DialogResult res = MessageBox.Show("This stock is already under Active Order, Buy more ??.",
                                        "FYI",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                        
                        //bIsAlreadyScan = false;
                    }

                }

                if (bIsAlreadyScan == false)
                {
                    ThreadChildren ths = new ThreadChildren();
                    Thread ChildThread = new Thread(() => { ths.CallToTradingThread(updater, OrderID, exchange, name); });
                    try
                    {
                        Map_ChildTradingThreadCol.Add(name, ChildThread);
                    }
                    
                    catch(System.ArgumentException ex)
                    {

                    }
                    ChildThread.Start();
                }
            }

            //Trade_status_threads[index] = new Thread(new ParameterizedThreadStart(CallToChildThread));
            // Trade_status_threads[index] = new Thread(() => { CallToScannerThread(updater, index, exchange, ticker); });
            //Thread th = new Thread(new ParameterizedThreadStart(CallToScannerThread));
            //Trade_status_threads[index].Name = name;

            return;
        }



        public static Thread LaunchTrendingChartThread(int index)
        {
            Trending_chart_threads = new Thread(() => CallToChildTrendingThread(index));
            //Trending_chart_threads.Name = name;
            Trending_chart_threads.Start();
            return Trending_chart_threads;
        }

        public static Thread LaunchMarketAnalysisThread(string exch )
        {
            //MarketAnalysis_threads = new Thread(() => ChildMarketAnalysisThread(exch));
            ////Trending_chart_threads.Name = name;
            MarketAnalysis_threads.Start();
            return MarketAnalysis_threads;
        }

        public static Thread LaunchMarketAnalysisThread_Progress(IProgress<int> progress, string exch)
        {
            MarketAnalysis_threads = new Thread(() => ChildMarketAnalysisThread(progress, exch));
            //Trending_chart_threads.Name = name;
            MarketAnalysis_threads.Start();
            return MarketAnalysis_threads;
        }

        public static void ExitPollingThread()
        {
            if (null != PollthreadHandle)
            {
                PollthreadHandle.Abort();
                PollthreadHandle = null;
            }

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

        private static readonly Object _Lock = new Object();


        private static void PricePollingThread(object obj, Func<Dictionary<string,float>, int > UpdatePolledDate )
        {


            Dictionary<string, float> sharedActiveStockList = (Dictionary<string, float>)obj;


                using (WebClient wc = new WebClient())
                {
                Dictionary<string, float> tempDic = sharedActiveStockList;
                    while (true) // cannot stuck at forever; after this count over we will sale it @ 1% loss
                    {
                            String jSonStr = string.Empty;

                            // do any background work
                            try
                            {
                                
                                Thread.Sleep(5000);
                                if (sharedActiveStockList.Count == 0)
                                    continue;

                                lock (sharedActiveStockList)
                                {

                                    tempDic = Google.StringTypeParser.Get_gAPI_MapLatestPrice("NSE", sharedActiveStockList);//Formulas.getCurrentTradePrice(jSonStr);
                                    if(tempDic.Count == 0)
                                    {
                                        //MessageBox.Show("Error, because of following reasons \n1. Internet not working, \n2. Market is closed.",
                                        //                "Error Reason", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                        var userResult = AutoClosingMessageBox.Show("Error, because of following reasons \n1. Internet not working, \n2. Market is closed.",
                                            "Error Reason", 3000, MessageBoxButtons.OK);
                                        if (userResult == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            // do something
                                        }
                                    }
                            
                                }

                                UpdatePolledDate(tempDic);

                                // Console.WriteLine(string.Format("Fetched  {0}:{1:0.00##}", ticker, fetched_price));
                                // algo_gp.GreedyPeek_Strategy_Execute(fetched_price, 100);

                            }
                            catch (WebException ex)
                            {
                                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                                {
                                    var resp = (HttpWebResponse)ex.Response;
                                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                                    {
                                        //Handle it
                                        Console.WriteLine("End resp.StatusCode ==>api fetch failed." );
                                    }
                                }
                                //Handle it
                                return;
                            }
                            

                            //Console.WriteLine(lstBusinessModel.Count);


                    } // forever looping block.

                } //end of using block

        }


        public static Thread StartPricePollingThread( object ActiveStocksList, Func<Dictionary<string, float>, int> func1)
        {

            PollthreadHandle = new Thread(() => PricePollingThread(ActiveStocksList, func1));
            //Trending_chart_threads.Name = name;
            PollthreadHandle.Start();
            return PollthreadHandle;
        }
    }
}
