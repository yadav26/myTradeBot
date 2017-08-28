using AlgoCollection;
using FinanceManagerLib;
using Google;
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
using TaxCalculator;
using Trading.Entity;
using Trading.Model;

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

        public delegate Dictionary<string, UpdateScannerGridObject> ActiveOrderGridUpdater(List<ActiveOrder> scobj);

        public delegate List<ActiveOrder> CompleteOrdersGridUpdater(List<CompletedOrders> lsCompobj);

        //static ThreadStart childrefTrending = new ThreadStart(CallToChildTrendingThread);
        static Thread[] Trade_status_threads = new Thread[MAX_THREAD_COUNT];

        //Hardcoding number of placing orders.
        static List<Thread> HandleToPlacePurchaseOrderThread = new List<Thread>();
        static List<Thread> HandleToSaleOrderThread = new List<Thread>();

        static Thread Trending_chart_threads = null;
        static Thread MarketAnalysis_threads = null;
        static Thread PollthreadHandle = null;
        static Thread MarketAnalysis_Workerthread = null;
        static Thread AlgorithmThreadHandle = null;

        //static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";

        public static SortableBindingList<MarketAnalysisDataumModel> ls_marketData = new SortableBindingList<MarketAnalysisDataumModel>();

        public List<Thread> List_ChildThreadCol = new List<Thread>();
        public static SortedDictionary<string, Thread> Map_ChildScannerThreadCol = new SortedDictionary<string, Thread>();
        public static SortedDictionary<string, Thread> Map_ChildTradingThreadCol = new SortedDictionary<string, Thread>();


        /// <summary>
        /// This list will be the data source for the ActiveOrder grid.
        /// </summary>
        public static List<ActiveOrder> List_ActiveOrders = new List<ActiveOrder>();
        public static List<CompletedOrders> List_CompletedOrders = new List<CompletedOrders>();

        static void CallToChildTrendingThread(int order_id)
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

            List<StringParsedData> ghs1 = todayReader1.GetGHistoryList();
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
                sr1.Add(dataum.Open);

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

        public static SortableBindingList<MarketAnalysisDataumModel> childWorkerMarketAnalysis(IProgress<int> progress, string exch)
        {

            MarketAnalysis.Start_MarketAnalysis(progress, exch);

            return MarketAnalysis.List_MarketAnalysisData;
        }



        public static void ChildMarketAnalysisThread(IProgress<int> progress, string str)
        {
            ThreadManager.LaunchChildMarketAnalysisThread(progress, str);

            return;
        }

        public static SortableBindingList<MarketAnalysisDataumModel> LaunchChildMarketAnalysisThread(IProgress<int> progress, string exchange)
        {

            MarketAnalysis_Workerthread = new Thread(() => { ls_marketData = childWorkerMarketAnalysis(progress, exchange); });

            MarketAnalysis_Workerthread.Start();

            MarketAnalysis_Workerthread.Join();

            return ls_marketData;
        }

        public static void TerminateAllScannerThread()
        {
            foreach (KeyValuePair<string, Thread> kvp in Map_ChildScannerThreadCol)
            {
                kvp.Value.Abort();
            }

            
            //aborting thread for all scanner rows individually.
            foreach( Thread thAlgo in HandleToPlacePurchaseOrderThread )
            {
                if( thAlgo != null )
                {
                    thAlgo.Abort();
                }
            }

            //abort parent thread
            if (null != AlgorithmThreadHandle)
                AlgorithmThreadHandle.Abort();

            //abort parent price polling and grid rows updating thread
            if (null != PollthreadHandle)
                PollthreadHandle.Abort();
        }

        public static void TerminateAllTradingThread()
        {
            foreach (KeyValuePair<string, Thread> kvp in Map_ChildTradingThreadCol)
            {
                kvp.Value.Abort();
            }
        }

        public static Thread LaunchTrendingChartThread(int index)
        {
            Trending_chart_threads = new Thread(() => CallToChildTrendingThread(index));
            //Trending_chart_threads.Name = name;
            Trending_chart_threads.Start();
            return Trending_chart_threads;
        }

        public static Thread LaunchMarketAnalysisThread(string exch)
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

        public static void CleanUpAllThreads()
        {
            foreach (Thread t in Trade_status_threads)
                if (t != null)
                    t.Abort();
        }


        private static void PricePollingThread(object obj, Func<Dictionary<string, UpdateScannerGridObject>, int> UpdatePolledDate)
        {
            //Dictionary<string, float> sharedActiveStockList = (Dictionary<string, float>)obj;
            List<string> sharedActiveStockList = (List<string>)obj;
            string exchange = "NSE";

            using (WebClient wc = new WebClient())
            {
                Dictionary<string, UpdateScannerGridObject> tempDic = new Dictionary<string, UpdateScannerGridObject>(); //sharedActiveStockList;
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

                            //tempDic = Google.StringTypeParser.Get_gAPI_MapLatestPrice("NSE", sharedActiveStockList);
                            //Formulas.getCurrentTradePrice(jSonStr);
                            TranslateJsonToObject JsonInfoObject = new TranslateJsonToObject();
                            tempDic = JsonInfoObject.GetMapOfTickerCurrentPrice(exchange, sharedActiveStockList);
                            if (null == tempDic || tempDic.Count == 0)
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
                            else
                            {

                                foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in tempDic)
                                {
                                    Thread.Sleep(2000);

                                    Daily_Reader todayReader1 = new Daily_Reader();
                                    todayReader1.parser(exchange, kvp.Key, 60, 1); // 600 = 10 sec, 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y
                                    List<StringParsedData> ghs1 = todayReader1.GetGHistoryList();
                                    if (ghs1 == null)
                                    {
                                        UpdatePolledDate(tempDic);
                                        continue;
                                    }


                                    Algorithm_VolumeWeightMA objVWMA = new Algorithm_VolumeWeightMA(ghs1);
                                    kvp.Value.TVWMA = objVWMA.VWMA;

                                    Algorithm_WeightedMovingAverage owma = new Algorithm_WeightedMovingAverage(ghs1);
                                    kvp.Value.TWMA = owma.WMA;

                                    Algorithm_ExpoMovingAverage objEma = new Algorithm_ExpoMovingAverage(ghs1, 90, 10);
                                    kvp.Value.TEMA = Formulas.banker_ceil(objEma.EMA);

                                    kvp.Value.THighest = todayReader1.TodayMax;
                                    kvp.Value.TLowest = todayReader1.TodayMin;

                                    kvp.Value.TVolume = ghs1[ghs1.Count-1].Volume;
                                    //if( )
                                    //kvp.Value.High90 = kvp.Value.High90;
                                    //kvp.Value.Low90 = kvp.Value.Low90;
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
                                Console.WriteLine("End resp.StatusCode ==>api fetch failed.");
                            }
                        }
                        //Handle it
                        return;
                    }


                    //Console.WriteLine(lstBusinessModel.Count);


                } // forever looping block.

            } //end of using block

        }

        private static void RunAlgorithmOnRowTicker(object obj, ActiveOrderGridUpdater UpdateAcviteOrderGrid)
        {
            UpdateScannerGridObject objScanner = (UpdateScannerGridObject)obj;

            string Ticker = objScanner.Ticker;

            int id_algo = 0;

            Algorithm algoObj = null;

            switch (id_algo)
            {
                case 0:
                    algoObj = new Buy_MedianPrice(  objScanner.Ticker, 
                                                    objScanner.CurrentPrice, 
                                                    objScanner.TLowest, 
                                                    objScanner.THighest, 
                                                    objScanner.Low90, 
                                                    objScanner.High90 );
                    break;

                case 1:
                    algoObj = new Algorithm_GreedyPeek();
                    break;


                default:
                    DialogResult res = MessageBox.Show("Invalid Algorithm to apply, please check input.",
                                                        "FYI",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Warning);
                    break;
            }


            int nPriority = 5;
            AccountHandler  AccObj= AccountHandler.GetHandlerObject();

            int nUnits = AccObj.GetUnitsToBet(nPriority, objScanner.CurrentPrice);
            
            ActiveOrder activeOrder = algoObj.Execute_Strategy(objScanner, nUnits);
            lock (List_ActiveOrders)
            {
                if (null != activeOrder)
                {
                    bool isAvaialbale = List_ActiveOrders.Where(x => x.Ticker.Equals(objScanner.Ticker)).Any();
                    if (isAvaialbale == false)
                    {
                        List_ActiveOrders.Add(activeOrder);
                    }
                    
                }
   
            }
            UpdateAcviteOrderGrid(List_ActiveOrders);
            return;
        }

        
        private static void PlaceOrdersThread(object obj, ActiveOrderGridUpdater func1)
        {

            Dictionary<string, UpdateScannerGridObject> mapObject = (Dictionary<string, UpdateScannerGridObject>)obj;



            while (true)
            {

                // do any background work
                try
                {
                    mapObject = func1(List_ActiveOrders);

                    Thread.Sleep(5000);

                    if (mapObject.Count == 0)
                        continue;

                    //Fixing bug; on each iteration new threads generated and never get killed. 
                    //Here killing all algo threads and clearing the list.
                    foreach (Thread th in HandleToPlacePurchaseOrderThread)
                    {
                        if( null != th)
                            th.Abort();
                    }
                    HandleToPlacePurchaseOrderThread.Clear();
                    //----> Fixed bugs ends

                    foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in mapObject)
                    {
                        Thread thPlaceOrder = new Thread(() => RunAlgorithmOnRowTicker(kvp.Value, func1));

                        HandleToPlacePurchaseOrderThread.Add( thPlaceOrder );

                        thPlaceOrder.Start();
                        
                    }

                    //Fixed bug; we might have accidentally terminated any thread, safeguarding lets all buy algo finish
                    // and then will go back to clear all.
                    //IT might slow down.. have to check test and validate the difference
                    foreach (Thread th in HandleToPlacePurchaseOrderThread)
                        th.Join();

                }
                catch (Exception e)
                {

                }


            }

            return;    
            
        }


        private static void SaleOrdersThreadCallBack(object objao, object objco, CompleteOrdersGridUpdater func)
        {

            List<ActiveOrder> list_CurrentlyTrading = (List<ActiveOrder>)objao;
            List<CompletedOrders> list_CompletedOrders = (List<CompletedOrders>) objco;

            while (true)
            {

                // do any background work
                try
                {
                    //delegate will call the function in Form1.cs and retrieve the active order list, local in UI.
                    list_CurrentlyTrading = func(list_CompletedOrders);

                    Thread.Sleep(5000);
                    if (list_CurrentlyTrading.Count == 0)
                        continue;

                    //Fixing bug; on each iteration new threads generated and never get killed. 
                    //Here killing all algo threads and clearing the list.
                    foreach (Thread th in HandleToSaleOrderThread)
                    {
                        if (null != th)
                            th.Abort();
                    }
                    HandleToSaleOrderThread.Clear();
                    //----> Fixed bugs ends

                    foreach ( ActiveOrder stock in list_CurrentlyTrading )
                    {
                        Thread thPlacSaleOrder = new Thread(() => RunSaleAlgorithmOnTicker( stock, func));

                        HandleToSaleOrderThread.Add(thPlacSaleOrder);

                        thPlacSaleOrder.Start();

                    }

                    foreach (Thread th in HandleToSaleOrderThread)
                        th.Join();

                }
                catch (Exception e)
                {

                }

                
            }

            return;
            
        }

        private static void RunSaleAlgorithmOnTicker(ActiveOrder stock, CompleteOrdersGridUpdater UpdateOrderGrids)
        {

            //UpdateScannerGridObject objScanner = (UpdateScannerGridObject)obj;

            string Ticker = stock.Ticker;

            int id_algo = stock.id_algorithm_sale;

            Algorithm algoObj = null;

            switch (id_algo)
            {
                case 0:
                    algoObj = new Sell_MinProfit(  stock );
                    break;

                case 1:
                    algoObj = new Algorithm_GreedyPeek();
                    break;


                default:
                    DialogResult res = MessageBox.Show("Invalid Algorithm to apply, please check input.",
                                                        "FYI",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Warning);
                    break;
            }


            int nPriority = 5;
            AccountHandler AccObj = AccountHandler.GetHandlerObject();

            int nUnits = AccObj.GetUnitsToBet(nPriority, stock.Current_Price);

             SaleOrder so = algoObj.Execute_Strategy(stock);

            lock (List_ActiveOrders)
            {
                //bool isAvaialbale = List_ActiveOrders.Where(x => x.Ticker.Equals(stock.Ticker)).Any();
                //if (isAvaialbale == false)
                //{
                    if (null != so)
                    {
                        stock.OrderSaleDetails = so;
                        List_CompletedOrders.Add(new CompletedOrders(stock));
                        
                    }
                //}
            }

            UpdateOrderGrids(List_CompletedOrders);
            //return;

            //throw new NotImplementedException();
        }

        public static Thread StartPricePollingThread( object ActiveStocksList, Func<Dictionary<string, UpdateScannerGridObject>, int> func1)
        {

            PollthreadHandle = new Thread(() => PricePollingThread(ActiveStocksList, func1));
            //Trending_chart_threads.Name = name;
            PollthreadHandle.Start();
            return PollthreadHandle;
        }

        public static Thread StartAlgorithmThread(object MapScanner, ActiveOrderGridUpdater func1)
        {

            AlgorithmThreadHandle = new Thread(() => PlaceOrdersThread(MapScanner, func1));
            //Trending_chart_threads.Name = name;
            AlgorithmThreadHandle.Start();
            return AlgorithmThreadHandle;
        }

        public static Thread StartSaleOrderThreads(object ao, object co, CompleteOrdersGridUpdater func_updater)
        {
            AlgorithmThreadHandle = new Thread(() => SaleOrdersThreadCallBack(ao, co, func_updater));
            //Trending_chart_threads.Name = name;
            AlgorithmThreadHandle.Start();
            return AlgorithmThreadHandle;
        }


    }
}
