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

        public delegate void UpdateScannerGridList(List<string> ls_tickerEnqueue);

        public delegate void UpdateMarketHistoryGrid(object marketData);
        public delegate int FillScannerGrid(object marketData);

        public delegate void OnlyPriceUpdater(Dictionary<string, UpdateScannerGridObject> currMarketData);
        public delegate int MAFiledUpdater(Dictionary<string, UpdateScannerGridObject> scobj);

        static Thread[] Trade_status_threads = new Thread[MAX_THREAD_COUNT];

        //Hardcoding number of placing orders.
        static List<Thread> HandleToPlacePurchaseOrderThread = new List<Thread>();
        static List<Thread> HandleToSaleOrderThread = new List<Thread>();

        static Thread Trending_chart_threads = null;
        static Thread MarketAnalysis_threads = null;
        static Thread PollthreadHandle = null;
        static Thread MarketAnalysis_Todaythread = null;
        static Thread AlgorithmThreadHandle = null;
        static Thread AlgorithmSaleThreadHandle = null;

        private static AutoResetEvent waitHistoryHandle = new AutoResetEvent(false);

        //static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";

        public static Dictionary<string, UpdateScannerGridObject> MapScannerDataTM = new Dictionary<string, UpdateScannerGridObject>();
        //public static Dictionary<string, UpdateScannerGridObject> Ls_ScannerData = new Dictionary<string, UpdateScannerGridObject>();

        public static object ls_marketData = null;

        public List<Thread> List_ChildThreadCol = new List<Thread>();
        public static SortedDictionary<string, Thread> Map_ChildScannerThreadCol = new SortedDictionary<string, Thread>();
        public static SortedDictionary<string, Thread> Map_ChildTradingThreadCol = new SortedDictionary<string, Thread>();

        public static List<string> List_VWMA_Based_Tickers = new List<string>();

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

        public static object FetchTodayMACallBack(string exchange, UpdateMarketHistoryGrid FillScannerGrid)
        {
            object tmpobj = null;

            using (WebClient wc = new WebClient())
            {

                while(MapScannerDataTM.Count == 0) // This while is a fix for google block; it will keep 100 seconds to go and check once activated after 45 mins
                {
                    try
                    {
                        List<StringParsedData> ghs1 = null;
                        Daily_Reader todayReader1 = new Daily_Reader();
                        foreach (string strTick in TickerList.GetTickerList())
                        {
                            Thread.Sleep(2000);


                            
                            todayReader1.parser(exchange, strTick, 60, 1); // 600 = 10 sec, 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

                            ghs1 = todayReader1.GetGHistoryList();
                            if (ghs1 == null)
                                continue;


                            UpdateScannerGridObject UpScannerObj = new UpdateScannerGridObject();

                            lock (ghs1)
                            {

                                UpScannerObj.Ticker = strTick;


                                UpScannerObj.CurrentPrice = ghs1[ghs1.Count - 1].Close;

                                Algorithm_VolumeWeightMA objVWMA = new Algorithm_VolumeWeightMA(ghs1);
                                UpScannerObj.TVWMA = objVWMA.VWMA;
                                if (UpScannerObj.TVWMA != 0)
                                {
                                    UpScannerObj.TVWMA_PC = ((UpScannerObj.CurrentPrice - UpScannerObj.TVWMA) / UpScannerObj.TVWMA) * 100.00f;
                                }

                                Algorithm_WeightedMovingAverage owma = new Algorithm_WeightedMovingAverage(ghs1);
                                UpScannerObj.TWMA = owma.WMA;

                                Algorithm_ExpoMovingAverage objEma = new Algorithm_ExpoMovingAverage(ghs1, 90, 10);
                                UpScannerObj.TEMA = Formulas.banker_ceil(objEma.EMA);

                                UpScannerObj.THighest = todayReader1.TodayMax;
                                UpScannerObj.TLowest = todayReader1.TodayMin;

                                UpScannerObj.TVolume = ghs1[ghs1.Count - 1].Volume;

                            }


                            // 
                            MapScannerDataTM.Add(strTick, UpScannerObj);

                        }

                        if (MapScannerDataTM.Count == 0)
                        {
                            var userResult = AutoClosingMessageBox.Show("Error, because Google has blocked YOU. Wait for 45 minutes",
                                                                        "Error Reason", 3000, MessageBoxButtons.OK);
                            if (userResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                // do something
                            }

                        }

                        // Wait till history map getting filled, because we need to fill HighPrice90 fields here.
                        waitHistoryHandle.WaitOne();

                        foreach (MarketAnalysisDataumModel historyData in (SortableBindingList<MarketAnalysisDataumModel>)ls_marketData)
                        {
                            foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in MapScannerDataTM)
                            {
                                if (kvp.Key == historyData.Ticker)
                                {
                                    UpdateScannerGridObject objUp = kvp.Value;

                                    objUp.HighPrice90 = historyData.HighPrice90;
                                    objUp.LowPrice90 = historyData.LowPrice90;
                                    objUp.HighVolume90 = historyData.HighVolume90;
                                    objUp.LowVolume90 = historyData.LowVolume90;

                                    MapScannerDataTM[kvp.Key] = objUp;

                                    historyData.TEMA = kvp.Value.TEMA;
                                    historyData.THighest = kvp.Value.THighest;
                                    historyData.TLowest = kvp.Value.TLowest;
                                    historyData.TVWMA = kvp.Value.TVWMA;
                                    historyData.TVWMA_PC = kvp.Value.TVWMA_PC;

                                    break;
                                }
                            }


                        }


                        FillScannerGrid(MapScannerDataTM);

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
                        return null;
                    }


                    if( MapScannerDataTM.Count == 0)
                        Thread.Sleep(100000);

                }

            } //end of using block

            return tmpobj;
        }


        public static void MarketAnalysisThreadCallBack(IProgress<int> progress, string strExchange, UpdateMarketHistoryGrid FillHistoryGrid)
        {
            ls_marketData = MarketAnalysis.Start_MarketAnalysis(progress, strExchange);

            waitHistoryHandle.Set();
            FillHistoryGrid(ls_marketData);

        }
        

        /// <summary>
        /// Following will generate the filter VWMA based 
        /// </summary>
        private static List<string> Generate_list_PriorityVWMA( object list )
        {
            SortableBindingList <MarketAnalysisDataumModel> tempList = (SortableBindingList<MarketAnalysisDataumModel>)list;
            List<string> tempStringList = new List<string>();
            foreach ( MarketAnalysisDataumModel data in tempList)
            {
                if (data.VWMA < data.CurrentPrice)
                    tempStringList.Add(data.Ticker);
            }
            return tempStringList;
        }



        public static void TerminateAllScannerThread()
        {
            foreach (KeyValuePair<string, Thread> kvp in Map_ChildScannerThreadCol)
            {
                kvp.Value.Abort();
            }

            
            //abort parent thread
            if (null != AlgorithmThreadHandle)
            {
                foreach (Thread th in HandleToPlacePurchaseOrderThread)
                {
                    if (null != th)
                        th.Abort();
                }
                AlgorithmThreadHandle.Abort();
                AlgorithmThreadHandle = null;
            }
                

            //abort parent price polling and grid rows updating thread
            if (null != PollthreadHandle)
                PollthreadHandle.Abort();

            if( null != AlgorithmSaleThreadHandle)
            {
                foreach (Thread th in HandleToSaleOrderThread)
                {
                    if (null != th)
                    {
                        th.Abort();
                    }
                        
                }

                AlgorithmSaleThreadHandle.Abort();
                AlgorithmSaleThreadHandle = null;
            }
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
            
            Trending_chart_threads.Start();
            return Trending_chart_threads;
        }


        public static void LaunchMarketAnalysisThread_Progress(IProgress<int> progress, string exch, UpdateMarketHistoryGrid FillHistoryGrid, UpdateMarketHistoryGrid ScannerGridUpdater)
        {

            MarketAnalysis_Todaythread = new Thread(() => { FetchTodayMACallBack(exch, ScannerGridUpdater); });

            MarketAnalysis_Todaythread.Start();

            MarketAnalysis_threads = new Thread(() => {  MarketAnalysisThreadCallBack(progress, exch, FillHistoryGrid); });


            MarketAnalysis_threads.Start();


            //MarketAnalysis_threads.Join();

            //Now we need a delegate that could fill List_EnqueueOrders for scanner grid
            //List_VWMA_Based_Tickers = Generate_list_PriorityVWMA(ls_marketData);


            return ;
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


        private static void PricePollingThread(object obj, OnlyPriceUpdater UpdatePolledPriceData, MAFiledUpdater UpdateGridStatistics )
        {
            //Dictionary<string, float> sharedActiveStockList = (Dictionary<string, float>)obj;
            List<string> sharedActiveStockList = MapScannerDataTM.Keys.ToList();
            string exchange = "NSE";

            using (WebClient wc = new WebClient())
            {
                //Dictionary<string, UpdateScannerGridObject> tempDic = new Dictionary<string, UpdateScannerGridObject>(); //sharedActiveStockList;
                while (true) // cannot stuck at forever; after this count over we will sale it @ 1% loss
                {
                    String jSonStr = string.Empty;

                    // do any background work
                    try
                    {
                        //Bug , if below list not filled then it will be hang, hence replace it with Generate_list_PriorityVWMA(ls_marketData);
                        //sharedActiveStockList = List_VWMA_Based_Tickers;
                        // sharedActiveStockList = Generate_list_PriorityVWMA(ls_marketData);
                        // Thread.Sleep(5000);
                        // if (sharedActiveStockList.Count == 0)
                        //     continue;

                        // lock (sharedActiveStockList)// this is wvma prioritised list.
                        // {

                        //tempDic = Google.StringTypeParser.Get_gAPI_MapLatestPrice("NSE", sharedActiveStockList);
                        //Formulas.getCurrentTradePrice(jSonStr);
                        TranslateJsonToObject JsonInfoObject = new TranslateJsonToObject();
                        Dictionary<string, UpdateScannerGridObject> tempDic = JsonInfoObject.GetMapOfTickerCurrentPrice(exchange, sharedActiveStockList);

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
                            //Immediately updating the UI grid for the price change. Other fields will take time and shall be updated in a different Delegate
                            UpdatePolledPriceData(tempDic);



                            //-------------> we already got the latest price in map here.now try to access daily reader

                            foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in tempDic)
                            {


                                Daily_Reader todayReader1 = new Daily_Reader();

                                todayReader1.parser(kvp.Value.Exchange, kvp.Key, 60, 1); // 600 = 10 sec, 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

                                List<StringParsedData> ghs1 = todayReader1.GetGHistoryList();
                                if (ghs1 == null)
                                {
                                    //UpdatePolledDate(MapScannerDataTM);
                                    continue;
                                }

                                kvp.Value.TVolume = ghs1[ghs1.Count - 1].Volume;


                                Algorithm_VolumeWeightMA objVWMA = new Algorithm_VolumeWeightMA(ghs1);
                                kvp.Value.TVWMA = objVWMA.VWMA;
                                if (kvp.Value.TVWMA != 0)
                                {
                                    kvp.Value.TVWMA_PC = ((kvp.Value.CurrentPrice - kvp.Value.TVWMA) / kvp.Value.TVWMA) * 100.00f;
                                }

                                Algorithm_WeightedMovingAverage owma = new Algorithm_WeightedMovingAverage(ghs1);
                                kvp.Value.TWMA = owma.WMA;

                                Algorithm_ExpoMovingAverage objEma = new Algorithm_ExpoMovingAverage(ghs1, 90, 10);
                                kvp.Value.TEMA = Formulas.banker_ceil(objEma.EMA);

                                kvp.Value.THighest = todayReader1.TodayMax;
                                kvp.Value.TLowest = todayReader1.TodayMin;


                                //if( )
                                //kvp.Value.HighPrice90 = kvp.Value.HighPrice90;
                                //kvp.Value.Low90 = kvp.Value.Low90;
                                // Instead reading from Grid , we have a static list for ls_marketData; which is comprehensive market history
                                //list. This data is constant and not going to change for today Analysis.
                                // It is wise to use this list then, UI updating cells( which are susceptible) of wrong information 
                                foreach (MarketAnalysisDataumModel historyData in (SortableBindingList<MarketAnalysisDataumModel>)ls_marketData)
                                {
                                    if (kvp.Key == historyData.Ticker)
                                    {
                                        kvp.Value.HighPrice90 = historyData.HighPrice90;
                                        kvp.Value.LowPrice90 = historyData.LowPrice90;
                                        kvp.Value.HighVolume90 = historyData.HighVolume90;
                                        kvp.Value.LowVolume90 = historyData.LowVolume90;

                                    }

                                }

                            } // End of MA calculations.



                        } // Found valid map, internet is on.



                    
                        // Console.WriteLine(string.Format("Fetched  {0}:{1:0.00##}", ticker, fetched_price));
                        // algo_gp.GreedyPeek_Strategy_Execute(fetched_price, 100);


                        lock (MapScannerDataTM)
                        {
                            // Filter list logic
                            List<UpdateScannerGridObject> tempUpScannerList = MapScannerDataTM.Values.ToList();

                            if (tempUpScannerList.Count == 0)//exception fix.
                                return;

                            Dictionary<string, UpdateScannerGridObject> LocalMap = new Dictionary<string, UpdateScannerGridObject>();

                            tempUpScannerList = tempUpScannerList.OrderByDescending(o => o.TVWMA_PC).Take(10).ToList();

                            LocalMap = tempUpScannerList.ToDictionary(x => x.Ticker, x => x);

                            //for (int x = 0; x < 10; ++x)
                            //{
                            //    if (tempUpScannerList[x].TVWMA_PC > 0)
                            //    {
                            //        LocalMap.Add(tempUpScannerList[x].Ticker, tempUpScannerList[x]);// (Dictionary<string, UpdateScannerGridObject>)tempUpScanner.ToDictionary(f => f.Ticker, f => f).Take(20);
                            //    }
                            //}
                            

                            UpdateGridStatistics(LocalMap);
                        }

                        

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
                                                    objScanner.LowPrice90, 
                                                    objScanner.HighPrice90,
                                                    objScanner.TVWMA );
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
                    lock (List_ActiveOrders)
                    {
                        mapObject = func1(List_ActiveOrders);
                    }
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

                    Dictionary<string, UpdateScannerGridObject> local_Filtered = new Dictionary<string, UpdateScannerGridObject>();
                    foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in mapObject)
                    {
                        kvp.Value.TVWMA_PC = (( (kvp.Value.CurrentPrice - kvp.Value.TVWMA )*100.0f )/ kvp.Value.TVWMA);
                    }


                    //List<UpdateScannerGridObject> tempUpScanner = mapObject.Values.ToList();

                    //tempUpScanner = (List<UpdateScannerGridObject>)tempUpScanner.OrderByDescending(pair => pair.TVWMA_PC).ToList();

                    //mapObject.Clear();
                    //mapObject = tempUpScanner.ToDictionary(f => f.Ticker, f => f);

                    //// local_Filtered = (Dictionary<string, UpdateScannerGridObject>)mapObject.OrderBy(x => x.Value.TVWMA_PC);
                    //mapObject = (Dictionary<string, UpdateScannerGridObject>)mapObject.OrderByDescending(pair => pair.Value.TVWMA_PC).Take(20);


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

            lock (List_CompletedOrders)
            {
                //bool isAvaialbale = List_ActiveOrders.Where(x => x.Ticker.Equals(stock.Ticker)).Any();
                //if (isAvaialbale == false)
                //{
                    if (null != so)
                    {
                        stock.SaleOrder = so;
                        List_CompletedOrders.Add(new CompletedOrders(stock));
                        
                    }
                //}
                UpdateOrderGrids(List_CompletedOrders);
            }

           
            //return;

            //throw new NotImplementedException();
        }

        public static Thread StartPricePollingThread( object ActiveStocksList, OnlyPriceUpdater func1, MAFiledUpdater func2)
        {

            PollthreadHandle = new Thread(() => PricePollingThread(ActiveStocksList, func1, func2));
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
            AlgorithmSaleThreadHandle = new Thread(() => SaleOrdersThreadCallBack(ao, co, func_updater));
            //Trending_chart_threads.Name = name;
            AlgorithmSaleThreadHandle.Start();
            return AlgorithmSaleThreadHandle;
        }


    }
}
