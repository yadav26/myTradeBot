using AlgoCollection;
using ExchangePortal;
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

        public delegate Dictionary<string, UpdateScannerGridObject> UpdateActiveOrderStatistics(List<ActiveOrder> scobj);

        public delegate List<ActiveOrder> CompleteOrdersGridUpdater(List<CompletedOrders> lsCompobj);

        public delegate void UpdateScannerGridList(List<string> ls_tickerEnqueue);

        public delegate void UpdateMarketHistoryGrid(object marketData);
        public delegate int FillScannerGrid(object marketData);

        public delegate void OnlyPriceUpdater(Dictionary<string, float> currMarketData);
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
        private static AutoResetEvent waitHigh90 = new AutoResetEvent(false);
        
        //static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";

        

        public static object ls_marketData = null;

        public List<Thread> List_ChildThreadCol = new List<Thread>();
        public static SortedDictionary<string, Thread> Map_ChildScannerThreadCol = new SortedDictionary<string, Thread>();
        public static SortedDictionary<string, Thread> Map_ChildTradingThreadCol = new SortedDictionary<string, Thread>();

        public static List<string> List_VWMA_Based_Tickers = new List<string>();


        public static Dictionary<string, UpdateScannerGridObject> MapScannerDataTM = new Dictionary<string, UpdateScannerGridObject>();
        public static Dictionary<string, UpdateScannerGridObject> gFilteredMapTop10VWMA = new Dictionary<string, UpdateScannerGridObject>();

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


            //while (true) // MOD: this new loop is the keep finding the TVWMA, TWMA, TEMA of all stockts in every 5 minutes Thread.Sleep(5*60*1000 )
            {

                try
                {
                    MapScannerDataTM.Clear();
                    List<StringParsedData> ghs1 = null;
                    Daily_Reader todayReader1 = new Daily_Reader();
                    foreach (string strTick in TickerList.GetTickerList())
                    {
                        

                        todayReader1.parser(exchange, strTick, 10, 1); // 600 = 10 sec, 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

                        

                        ghs1 = todayReader1.GetGHistoryList();
                        if (ghs1 == null || ghs1.Count == 0 )
                            continue;

                        //Current price is the last in list.
                        float cp = ghs1[ghs1.Count - 1].Close;

                        UpdateScannerGridObject UpScannerObj = new UpdateScannerGridObject();

                       // lock (ghs1)
                        {

                            UpScannerObj.Ticker = strTick;


                            UpScannerObj.CurrentPrice = cp;

                            Algorithm_VolumeWeightMA objVWMA = new Algorithm_VolumeWeightMA(ghs1);
                            UpScannerObj.TVWMA = objVWMA.VWMA;
                            if (UpScannerObj.TVWMA != 0)
                            {
                                UpScannerObj.TVWMA_PC = ((UpScannerObj.CurrentPrice - UpScannerObj.TVWMA) / UpScannerObj.TVWMA) * 100.00f;
                                if( 0.0f == UpScannerObj.TVWMA_PC )
                                {

                                }
                            }
                            else
                                continue;

                            Algorithm_WeightedMovingAverage owma = new Algorithm_WeightedMovingAverage(ghs1);
                            UpScannerObj.TWMA = owma.WMA;

                            Algorithm_ExpoMovingAverage objEma = new Algorithm_ExpoMovingAverage(ghs1, 90, 10);
                            UpScannerObj.TEMA = Formulas.banker_ceil(objEma.EMA);

                            UpScannerObj.THighest = todayReader1.TodayMax;
                            UpScannerObj.TLowest = todayReader1.TodayMin;

                            UpScannerObj.TVolume = ghs1[ghs1.Count - 1].Volume;

                        }

                        string ticker = strTick;
                        switch (ticker)
                        {
                            case "M&M":
                                ticker = "M%26M";
                                break;
                            case "L&TFH":
                                ticker = "L%26TFH";
                                break;
                            case "M&MFIN":
                                ticker = "M%26MFIN";
                                break;
                            default:
                                break;
                        }

                        int indext = ((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData).IndexOf(((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData).Where(a => a.Ticker.Equals(ticker)).FirstOrDefault());

                        if (indext == -1)
                            continue;

                        MarketAnalysisDataumModel mad =  ((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData).Where(a => a.Ticker.Equals(ticker)).FirstOrDefault();
                        //ActiveOrder aoTemp = List_ActiveOrders.Where(a => a.Ticker.Equals(ticker)).FirstOrDefault();
                        UpScannerObj.HighPrice90 = mad.HighPrice90;
                        UpScannerObj.LowPrice90 = mad.LowPrice90;

                        UpScannerObj.HighVolume90 = mad.HighVolume90;
                        UpScannerObj.LowVolume90 = mad.LowVolume90;

                        // 
                        MapScannerDataTM.Add(strTick, UpScannerObj);
                        ((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData)[indext].TVWMA = UpScannerObj.TVWMA;
                        ((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData)[indext].TVWMA_PC = UpScannerObj.TVWMA_PC;
                        ((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData)[indext].TEMA = UpScannerObj.TEMA;
                        ((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData)[indext].THighest = UpScannerObj.THighest;
                        ((SortableBindingList<MarketAnalysisDataumModel>)ls_marketData)[indext].TLowest = UpScannerObj.TLowest;


                        ghs1.Clear();
                        Thread.Sleep(5000);

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
                    return null;
                }

                // Thread.Sleep(5 * 60 * 1000); // 5minutes

            } // End of while loop where MA is analysed for all stocks.



           return tmpobj;
        }


        public static void MarketAnalysisThreadCallBack(IProgress<int> progress, string strExchange)
        {
            ls_marketData = MarketAnalysis.Start_MarketAnalysis(progress, strExchange);
            
            //Following line is completely incorrect. Intentionally not deleting and left commented. Calling delegate here is not invoking
            //****************************************
            //FillHistoryGrid(ls_marketData);
            //********************
            //=========================================

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

            if (null != MarketAnalysis_Primary)
            {
                if (null != MarketAnalysis_threads)
                {
                    MarketAnalysis_threads.Abort();
                    MarketAnalysis_threads = null;
                }
                if (null != MarketAnalysis_Todaythread)
                {
                    MarketAnalysis_Todaythread.Abort();
                    MarketAnalysis_Todaythread = null;
                }

                MarketAnalysis_Primary.Abort();
                MarketAnalysis_Primary = null;
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

        public static Thread MarketAnalysis_Primary = null;

        public static void LaunchMarketAnalysisPrimary(IProgress<int> progress, string exch, UpdateMarketHistoryGrid FillHistoryGrid, FillScannerGrid ScannerGridUpdater)
        {
            MarketAnalysis_Primary = new Thread(() => { LaunchMarketAnalysisThread_Progress(progress, exch, FillHistoryGrid, ScannerGridUpdater); });
            MarketAnalysis_Primary.Start();

        }
        public static void LaunchMarketAnalysisThread_Progress(IProgress<int> progress, string exch, UpdateMarketHistoryGrid FillHistoryGrid, FillScannerGrid ScannerGridUpdater)
        {
            MarketAnalysis_threads = new Thread(() => { MarketAnalysisThreadCallBack(progress, exch); });
            MarketAnalysis_threads.Start();
            MarketAnalysis_threads.Join();

            //This is important we have to call following delegate here, otherwise one level down it will fail to INVOKE
            FillHistoryGrid(ls_marketData);

            ///
            ///
            ///
            while (true)
            {
                MarketAnalysis_Todaythread = new Thread(() => { FetchTodayMACallBack(exch, FillHistoryGrid); });
                MarketAnalysis_Todaythread.Start();
                MarketAnalysis_Todaythread.Join();

                FillHistoryGrid(ls_marketData);

                MarketAnalysis_Todaythread.Abort();
                MarketAnalysis_Todaythread = null;

                //FOLLOWING CODE WILL READ THE LATEST ANALYSIS AND UPDATE SCANNER LIST
                {

                    // Filter list logic
                    gFilteredMapTop10VWMA = MapScannerDataTM.OrderByDescending(o => o.Value.TVWMA_PC).Take(10).ToDictionary(x => x.Key, x => x.Value); 

                    ScannerGridUpdater(gFilteredMapTop10VWMA);
                }


                Thread.Sleep(7 * 60 * 1000);
            }

            //This is important we have to call following delegate here, otherwise one level down it will fail to INVOKE
            //FillHistoryGrid(ls_marketData);

            //ScannerGridUpdater(MapScannerDataTM);


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

        public static Dictionary<string, float> quickDict = new Dictionary<string, float>();
        private static void PricePollingThread(object obj, OnlyPriceUpdater UpdatePolledPriceData, MAFiledUpdater UpdateGridStatistics )
        {
            //Dictionary<string, float> sharedActiveStockList = (Dictionary<string, float>)obj;

            string exchange = "NSE";

           // using (WebClient wc = new WebClient())
            //{
                Dictionary<string, UpdateScannerGridObject> tempDic = new Dictionary<string, UpdateScannerGridObject>(); //sharedActiveStockList;
                while (true) // cannot stuck at forever; after this count over we will sale it @ 1% loss
                {
                    String jSonStr = string.Empty;

                List<string> sharedActiveStockList = gFilteredMapTop10VWMA.Keys.ToList();

                // do any background work
                try
                    {


                            //tempDic.Clear();
                            quickDict.Clear();
                            foreach (string strTick in sharedActiveStockList)
                            {
                                Thread.Sleep(3000);

                                Daily_Reader todayReader1 = new Daily_Reader();
                                todayReader1.parser(exchange, strTick, 600, 1); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

                                List<StringParsedData> ghs1 = todayReader1.GetGHistoryList();
                                if (null == ghs1)
                                    continue;

                                float cp = ghs1[ghs1.Count - 1].Close;

                               // cp = PortalLibrary.GetPrice(cp); // fictitious

                                //UpdateScannerGridObject obj1 = new UpdateScannerGridObject();
                               // obj1.Ticker = strTick;
                              //  obj1.cp = cp;
                               // tempDic.Add(strTick, obj1);
                                quickDict.Add(strTick, cp);
                            }


                        UpdatePolledPriceData(quickDict);



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

            //} //end of using block

        }

        private static void RunAlgorithmOnRowTicker(object obj, UpdateActiveOrderStatistics UpdateAcviteOrderGrid)
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
            
            return;
        }

        
        private static void PlaceOrdersThread(object obj, UpdateActiveOrderStatistics func1)
        {

            



            while (true)
            {

                // do any background work
                try
                {
                    //lock (List_ActiveOrders)
                    //{
                    //    mapObject = func1(List_ActiveOrders);
                    //}
                    Thread.Sleep(5000);

                    Dictionary<string, UpdateScannerGridObject> mapObject = gFilteredMapTop10VWMA;//(Dictionary<string, UpdateScannerGridObject>)obj;

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

                    //Dictionary<string, UpdateScannerGridObject> local_Filtered = new Dictionary<string, UpdateScannerGridObject>();
                    //foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in mapObject)
                    //{
                    //    kvp.Value.TVWMA_PC = (( (kvp.Value.CurrentPrice - kvp.Value.TVWMA )*100.0f )/ kvp.Value.TVWMA);
                    //}


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


                    func1(List_ActiveOrders);

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

        public static Thread StartAlgorithmThread(object MapScanner, UpdateActiveOrderStatistics func1)
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


        public static void LaunchTradingThread(object ActiveStocksList, OnlyPriceUpdater func1, MAFiledUpdater func2, UpdateActiveOrderStatistics func3, CompleteOrdersGridUpdater func4 )
        {
            Thread thTradeHandle = null;

            StartPricePollingThread(gFilteredMapTop10VWMA.Values.ToList(), func1, func2);

            StartAlgorithmThread(gFilteredMapTop10VWMA, func3);

            //StartSaleOrderThreads(List_ActiveOrders, List_CompletedOrders, func4);

            return ;
        }
    }
}
