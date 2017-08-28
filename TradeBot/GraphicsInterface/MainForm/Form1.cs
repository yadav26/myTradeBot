using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core0.library;
using System.IO;
using mshtml;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using LiveCharts.Defaults;
using System.Threading;
using System.Diagnostics;
using Trading.Entity;
using Trading.Model;
using Trading.Model.BusinessModel;
using Trading.DAL;
using TaxCalculator;

namespace MainForm
{
    public partial class Form1 : Form
    {
        public static WebBrowser wbBrowser = null;
        public static int active_order_count_id = 0;
        public static int scan_count_id = 0;
        string Stylefile = string.Empty;
        System.Data.DataTable gDataTable = null;

        private System.Data.DataSet dataSet;
        //public static List<MarketAnalysisDataum> List_RenderMarketData = new List<MarketAnalysisDataum>();
        public static SortableBindingList<MarketAnalysisDataumModel> List_RenderMarketData = new SortableBindingList<MarketAnalysisDataumModel>();

        //public static List<Scanner> List_StocksUnderScanner = new List<Scanner>();
        public static List<CompletedOrders> List_CompletedOrders = new List<CompletedOrders>();
        public static List<ActiveOrder> List_ActiveOrders = new List<ActiveOrder>();
        //public static List<string> List_EnqueueStocks = new List<string>();
        Dictionary<string, UpdateScannerGridObject> DList_EnqueueOrders = new Dictionary<string, UpdateScannerGridObject>();
        List<string> List_EnqueueOrders = new List<string>();

        Dictionary<string, UpdateScannerGridObject> mapScanner = new Dictionary<string, UpdateScannerGridObject>();


        int newSortColumn;
        ListSortDirection newColumnDirection = ListSortDirection.Ascending;

        public Progress<int> progress = null;

        public List<TickerModel> lstofTicker = CommonDAL.GetTickerDetails();

        public Form1()
        {
            InitializeComponent();

            progressBar_MarketAnalysis.Maximum = 200;
            progressBar_MarketAnalysis.Step = 1;

            dataGridView_MarketAnalysis.AutoGenerateColumns = true;
            var source = new BindingSource();
            source.DataSource = List_RenderMarketData;
            dataGridView_MarketAnalysis.DataSource = source;


            //dataGridView_ActiveOrderList.Columns.
            splitContainer1.Orientation = Orientation.Vertical;
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel1.Controls.Add(dataGridView_ActiveOrderList);
            DataTable fooTable = new DataTable("fooTable");

            // number and content of rows are different from table to table.
            fooTable.Columns.Add("ColFoo");
            fooTable.Columns.Add("ColBar");
            fooTable.Columns.Add("ColSomethingElse");

            //AddRow(fooTable, "ValueA", "ValueB", "ValueC");
            //AddRow(fooTable, "ValueD", "ValueE", "ValueF");
            // ... more rows ...

            //return fooTable;

            wbBrowser = new WebBrowser();
            splitContainer1.Orientation = Orientation.Vertical;
            splitContainer1.Panel2Collapsed = false;


            splitContainer1.Panel2.Controls.Add(dataGridView1);

            splitContainer2.Orientation = Orientation.Vertical;
            splitContainer2.Panel1Collapsed = false;

            splitContainer_MarketAnalysis.Panel1.Controls.Add(dataGridView_MarketAnalysis);


            //Scanner
            splitContainer_Scanner.Panel1Collapsed = false;
            //var scanner_source = new BindingSource();
            //scanner_source.DataSource = List_StocksUnderScanner;
            //dataGridView_Scanner.DataSource = scanner_source;

            splitContainer_Scanner.Panel1.Controls.Add(dataGridView_Scanner);
            int count = dataGridView_Scanner.Rows.Count;
            //Completed Orders
            splitContainer_CompletedOrders.Panel1Collapsed = false;
            var source_co = new BindingSource();
            source_co.DataSource = List_CompletedOrders;
            dataGridView_CompletedOrders.DataSource = source_co;

            splitContainer_CompletedOrders.Panel1.Controls.Add(dataGridView_CompletedOrders);

            
            ThreadManager.StartPricePollingThread(List_EnqueueOrders, UpdateGridStatistics);


            //Launch Scanner Algorithm thread for placing Active orders.
            ThreadManager.StartAlgorithmThread( mapScanner, UpdateActiveOrderStatistics);

            //Launch sale threads for all Active orders 
            ThreadManager.StartSaleOrderThreads(List_ActiveOrders, List_CompletedOrders, UpdatePurchaseSaleGrids);

            //Progress bar
            progressBar_MarketAnalysis.Maximum = 100;
            progressBar_MarketAnalysis.Step = 1;

            progress = new Progress<int>(v =>
            {
                AutoClosingMessageBox.Show("hi", "title", 2000);
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                progressBar_MarketAnalysis.Value = v;
            });

            
        }


        private void MakeDataRelation()
        {

            DataColumn parentColumn =
                dataSet.Tables["ParentTable"].Columns["id"];

        }

        private void BindToDataGrid()
        {
            //DataGrid dataGrid1 = new DataGrid();
            // Instruct the DataGrid to bind to the DataSet, with the 
            // ParentTable as the topmost DataTable.
            //dataGrid1.SetDataBinding(dataSet, "ParentTable");
            dataGridView1.DataSource = dataSet;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataMember = "ParentTable"; // table name you need to show

        }

        private void MakeParentTable()
        {
            // Create a new DataTable.
            gDataTable = new DataTable("ParentTable");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Label";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            gDataTable.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Value";
            column.AutoIncrement = false;
            column.Caption = "ParentItem";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the column to the table.
            gDataTable.Columns.Add(column);

            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = gDataTable.Columns["Label"];
            gDataTable.PrimaryKey = PrimaryKeyColumns;

            // Instantiate the DataSet variable.
            dataSet = new DataSet();
            // Add the new DataTable to the DataSet.
            dataSet.Tables.Add(gDataTable);

            // Create three new DataRow objects and add 
            // them to the DataTable
            /*
                        ----------------------------------------STATISTICS.
            ITC
            Start: 2017 - 4 - 21, End: 2017 - 7 - 20
            Today Least   :290.25
            Today Maxim   :294.65
            Today Mean    :292.305
            Today Median  :292.305
            Buying Window (L):289.39 - (U):292.48
            History Least :271.01
            History Maxim :367.71
            History Mean  :303.22
            ---------------------------------------- - END.

            Fetched  ITC: 291.80
             * ***********************************BUY STATs.
             Buy Window     (L):289.39 - (U):292.48
            Purcased at       :291.80
            StopLoss: 288.89
            Brek even (BE)    :291.97
            Least profit ex: 292.00
            Profit Target     :296.35
            * ***********************************END.
            */
            row = gDataTable.NewRow();
            row["Label"] = "CodeName";
            row["Value"] = "AMD";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Today Least";
            row["Value"] = "290.25";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Today Maxim";
            row["Value"] = "294.65";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Today Mean";
            row["Value"] = "292.305";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Buying Window (L)";
            row["Value"] = "289.39";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Buying Window (U)";
            row["Value"] = "292.48";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "History Least";
            row["Value"] = "284.39";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "History Max";
            row["Value"] = "310.01";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "History Mean";
            row["Value"] = "284.39";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Purcased at";
            row["Value"] = "284.39";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Strategy";
            row["Value"] = "Greedy";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Quantity";
            row["Value"] = "100";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "StopLoss";
            row["Value"] = "284.39";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Brek even (BE) ";
            row["Value"] = "284.39";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "LPET";
            row["Value"] = "284.39";
            gDataTable.Rows.Add(row);

            row = gDataTable.NewRow();
            row["Label"] = "Profit Target";
            row["Value"] = "284.39";
            gDataTable.Rows.Add(row);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Generate_CartesianChart()
        {
            if (null == DataForChartRender.Series1)
                return;

            //splitContainer2.Panel1.Controls.Add(cartesianChart2);
            splitContainer2.Panel1.Refresh();

            cartesianChart2.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> ( DataForChartRender.Series1),
                },
                //new LineSeries
                //{
                //    Title = "Series 2",
                //    Values = new ChartValues<double>( DataForChartRender.Series2),
                //    PointGeometry = null
                //}
                /*,
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {5, 2, 8, 3},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }*/
            };

            cartesianChart2.AxisX.Add(new Axis
            {
                Title = "Time Interval",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });

            cartesianChart2.AxisY.Add(new Axis
            {
                Title = "Sales",
                LabelFormatter = value => value.ToString("C")
            });

            cartesianChart2.LegendLocation = LegendLocation.Right;

            //modifying the series collection will animate and update the chart
            //cartesianChart2.Series.Add(new LineSeries
            //{
            //    Values = new ChartValues<double> { 5, 3, 2, 4, 5 },
            //    LineSmoothness = 0, //straight lines, 1 really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = System.Windows.Media.Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            //cartesianChart2.Series[2].Values.Add(5d);


            //   cartesianChart2.DataClick += cartesianChart2OnDataClick;

            // Use the Text property for the button text for all cells rather
            // than using each cell's value as the text for its own button.
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //fill algorithm selection combo box
            comboBox_Algorithms.SelectedIndex = 0;
            
        }

        private void button_start_Scanner(object sender, EventArgs e)
        {
            string ticker = textBox_ticker.Text.Trim();
            //Launch the new thread
            if (ticker == "")
            {
                // textBox_ticker.BackColor = Color.Red;
                return;
            }


            if (textBox_stock_num.Text.Trim() == "")
            {
                // textBox_stock_num.BackColor = Color.Tomato;

                return;
            }
            try
            {
                if (List_RenderMarketData.Count == 0)
                {
                    MessageBox.Show("Market Trend is not available. Please Analyse Market Trend first.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    //return;
                }

                int stock_count = int.Parse(textBox_stock_num.Text);
                string exchange = "NSE";

                if (radioButton_NSE.Checked)
                    exchange = "NSE";

                if (radioButton_BSE.Checked)
                {
                    exchange = "BSE";
                    return; // not yet supported
                }

                //ThreadManager.LaunchScannerThread(ticker, 0, scan_count_id, UpdatePrice, exchange );
                //scan_count_id++;
            }
            catch (FormatException fe)
            {

                return;
            }

        }

        int UpdateCurrentPrice(Dictionary<string, float> scobj)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new MethodInvoker(() => { UpdateCurrentPrice(scobj); }));
                }
                catch (System.ArgumentException e)
                {

                }

            }
            else
            {

                foreach (KeyValuePair<string, float> kvp in scobj)
                {
                    if (dataGridView_ActiveOrderList.RowCount > 1)
                    {
                        foreach (DataGridViewRow row in dataGridView_ActiveOrderList.Rows)
                        {
                            if (row.Cells["Ticker"].Value.ToString() == kvp.Key)
                            {
                                int cellid = row.Index;
                                dataGridView_ActiveOrderList.Rows[cellid].Cells["Current_Price"].Value = kvp.Value;

                                // following will be read from database....
                                ///

                                //string purchse_price = dataGridView_ActiveOrderList.Rows[cellid].Cells["Purchased_Price"].Value.ToString();
                                //string units = dataGridView_ActiveOrderList.Rows[cellid].Cells["Units"].Value.ToString();
                                //ActiveOrder activeOrder = new ActiveOrder(cellid, kvp.Key, float.Parse(purchse_price), int.Parse(units), kvp.Value), 0,0;

                                //dataGridView_ActiveOrderList.Rows[cellid].Cells["Profit"].Value = Formulas.banker_ceil(activeOrder.Profit);

                            }

                        }
                    }

                    if(dataGridView_Scanner.RowCount > 0 )
                    {
                        foreach (DataGridViewRow row in dataGridView_Scanner.Rows)
                        {
                            if (row.Cells["Ticker"].Value.ToString() == kvp.Key)
                            {
                                int cellid = row.Index;
                                dataGridView_Scanner.Rows[cellid].Cells["Current_Price"].Value = kvp.Value;
                            }

                        } // updated Scanner gridview

                    }

                } //endof dictionary

            }
            return 0;
        }


        List<ActiveOrder> UpdatePurchaseSaleGrids(List<CompletedOrders> scobj)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new MethodInvoker(() => { UpdatePurchaseSaleGrids(scobj); }));
                }
                catch (System.ArgumentException e)
                {
                    MessageBox.Show("UpdatePurchaseSaleGrids - Invoke crashed before.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);

                }

            }
            else
            {
                lock (List_CompletedOrders)
                {
                    List<CompletedOrders> tmpListAO = (List<CompletedOrders>)scobj;

                    if (tmpListAO.Count <= 0)
                        return List_ActiveOrders;

                    foreach( CompletedOrders co in tmpListAO)
                    {
                        string ticker = co.Ticker;
                        ActiveOrder tbdel = List_ActiveOrders.Where(a => a.Ticker.Equals(ticker)).FirstOrDefault();
                        lock (List_ActiveOrders)
                        {
                            List_ActiveOrders.RemoveAll(x => x.Ticker.Equals(ticker));
                        }
                    }

                    dataGridView_ActiveOrderList.DataSource = null;
                    dataGridView_ActiveOrderList.DataSource = List_ActiveOrders;


                    List_CompletedOrders = tmpListAO;

                    //1. Update UI
                    dataGridView_CompletedOrders.DataSource = null;
                    dataGridView_CompletedOrders.DataSource = List_CompletedOrders;

                    //2. Update DB
                    //UpdateDB_ActiveOrderTable();
                    //UpdateDB_CompletedOrderTable();

                }
            }

            return List_ActiveOrders;
        }


        Dictionary<string, UpdateScannerGridObject> UpdateActiveOrderStatistics( List<ActiveOrder> scobj)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new MethodInvoker(() => { UpdateActiveOrderStatistics(scobj); }));
                }
                catch (System.ArgumentException e)
                {
                    MessageBox.Show("UpdateGridStatistics - Invoke crashed before.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);

                }

            }
            else
            {
                lock (scobj)
                {
                    dataGridView_ActiveOrderList.DataSource = null;
                    dataGridView_ActiveOrderList.DataSource = scobj;
                    List_ActiveOrders = scobj;
                }
            }
            
            return mapScanner;
        }

        int UpdateGridStatistics(Dictionary<string, UpdateScannerGridObject> scobj)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new MethodInvoker(() => { UpdateGridStatistics(scobj); }));
                }
                catch (System.ArgumentException e)
                {
                    MessageBox.Show("UpdateGridStatistics - Invoke crashed before.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);

                }

            }
            else
            {
                //Update the UI local map with the latest Updated map from polling thread
                //mapScanner = scobj;
                /// ---------------- Important

                foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in scobj)
                {
                    if( dataGridView_ActiveOrderList.RowCount > 0 )
                    {
                        foreach (DataGridViewRow row in dataGridView_ActiveOrderList.Rows)
                        {
                            //var cell = row.Cells.Count;
                            //if (row.Cells["Ticker"].Value == null || row.Cells["Ticker"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["Ticker"].Value.ToString() ))
                            if (row.Cells.Count > 1) // Default presence is 
                            {
                                if (row.Cells["Ticker"].Value.ToString() == kvp.Key)
                                {
                                    int cellid = row.Index;
                                    float cp = kvp.Value.CurrentPrice;
                                    dataGridView_ActiveOrderList.Rows[cellid].Cells["Current_Price"].Value = kvp.Value.CurrentPrice;

                                    #region Removing of Scanner object from GRID
                                    mapScanner.Remove(row.Cells["Ticker"].Value.ToString());
                                    dataGridView_Scanner.DataSource = null;
                                    var scanner_source = new BindingSource();
                                    scanner_source.DataSource = mapScanner.Values;
                                    dataGridView_Scanner.DataSource = scanner_source;
                                    #endregion

                                    #region This Commented code has to be replaced by DB objects and BreakEven will be decided from the PURCHASED RATE not from CURRENT PRICE
                                    //string spurchse_price = dataGridView_ActiveOrderList.Rows[cellid].Cells["Purchased_Price"].Value.ToString();
                                    //float purchse_price = float.Parse(spurchse_price);
                                    //string sunits = dataGridView_ActiveOrderList.Rows[cellid].Cells["Units"].Value.ToString();
                                    //int units = int.Parse(sunits);


                                    //float be = Formulas.getBreakEvenPrice(cp);
                                    //float tt = Formulas.getZerodha_Deductions(purchse_price, cp, units);
                                    //float profit = Formulas.netProfit(be, cp, units, tt);

                                    ////ActiveOrder activeOrder = new ActiveOrder(cellid, kvp.Key, purchse_price, units, kvp.Value.CurrentPrice, be, profit);

                                    //dataGridView_ActiveOrderList.Rows[cellid].Cells["Profit"].Value = Formulas.banker_ceil(profit);
                                    #endregion

                                }

                            }

                        }

                    }

                    if(dataGridView_Scanner.RowCount > 0 )
                    {
                        foreach (DataGridViewRow row in dataGridView_Scanner.Rows)
                        {
                            if (row.Cells.Count > 1)// || row.Cells["Ticker"].Value != null)
                            {
                                if (row.Cells["Ticker"].Value.ToString().Equals(kvp.Key))
                                {
                                    int cellid = row.Index;

                                    row.Cells["CurrentPrice"].Value = kvp.Value.CurrentPrice;
                                    row.Cells["TVWMA"].Value = kvp.Value.TVWMA;
                                    row.Cells["TWMA"].Value = kvp.Value.TWMA;
                                    row.Cells["TEMA"].Value = kvp.Value.TEMA;
                                    row.Cells["TSMA"].Value = kvp.Value.TSMA;
                                    row.Cells["THighest"].Value = kvp.Value.THighest;
                                    row.Cells["TLowest"].Value = kvp.Value.TLowest;
                                    row.Cells["TVolume"].Value = kvp.Value.TVolume;


                                }


                            }
                        } // updated Scanner gridview

                    }

                } //endof dictionary

            }
            return 0;
        }


        int UpdateProgress(int data)
        {
            if (false)
            {
                this.Invoke(new MethodInvoker(() => { UpdateProgress(data); }));
            }
            else
            {
                var progress = new Progress<int>(v =>
                {
                    // This lambda is executed in context of UI thread,
                    // so it can safely update form controls
                    progressBar_MarketAnalysis.Value = v;
                });
            }
            return 0;

        }

        private void dataGridView_tradeLists_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ThreadManager.TerminateAllScannerThread();

            ThreadManager.TerminateAllTradingThread();

            ThreadManager.ExitPollingThread();
        }


        private void button_AnalyseMarket_Click(object sender, EventArgs e)
        {
            string exchange = "NSE";

            if (radioButton_NSE.Checked)
                exchange = "NSE";

            if (radioButton_BSE.Checked)
            {
                exchange = "BSE";
                return; // not yet supported
            }


            //Thread th = null;
            // Run operation in another thread
            //await Task.Run( () => { th = ThreadManager.LaunchMarketAnalysisThread_Progress(progress, exchange); } ).ConfigureAwait(true);

            

            Thread th = ThreadManager.LaunchMarketAnalysisThread_Progress(progress, exchange);

            th.Join();

            List_RenderMarketData = ThreadManager.ls_marketData;
            dataGridView_MarketAnalysis.DataSource = List_RenderMarketData;
            #region Commented code
            //List<MarketAnalysisDataum> List_Market_Data = ThreadManager.ls_marketData;

            //foreach( MarketAnalysisDataum mad in List_Market_Data )
            //{
            //    if (mad.NRDay == true)
            //    {
            //        dataGridView_MarketAnalysis.Rows.Add(mad.Ticker, (-1 * mad.Trading_vol_Max), mad.LastClose, mad.TodayEMA, mad.TodaySMA, "YES");
            //    }
            //    else
            //        dataGridView_MarketAnalysis.Rows.Add(mad.Ticker, (-1 * mad.Trading_vol_Max), mad.LastClose, mad.TodayEMA, mad.TodaySMA, "NO");
            //}

            ////Color the rows
            //foreach (DataGridViewRow row in dataGridView_MarketAnalysis.Rows)
            //{
            //    if (row.Cells[5].Value == "YES")
            //    {
            //        row.DefaultCellStyle.BackColor = System.Drawing.Color.Lavender;
            //    }

            //}
            #endregion
        }

        private void dataGridView_Scanner_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {

                int row = e.RowIndex;

                //TODO - Button Clicked - Execute Code Here
                DataGridViewRow rowObj = dataGridView_Scanner.Rows[row];

                string name = Convert.ToString(rowObj.DataGridView.Rows[e.RowIndex].Cells["Ticker"].Value);

                float cp = float.Parse(rowObj.Cells["CurrentPrice"].Value.ToString());

                int stock_count = 100;

                string exchange = "NSE";

                if (radioButton_NSE.Checked)
                    exchange = "NSE";

                if (radioButton_BSE.Checked)
                {
                    exchange = "BSE";
                    return; // not yet supported
                }


                float be = Formulas.getBreakEvenPrice(cp);
                float tt = Formulas.getZerodha_Deductions(cp, cp, stock_count);
                float profit = Formulas.netProfit(be, cp, stock_count, tt);

                //ActiveOrder activeOrder = new ActiveOrder(active_order_count_id, name, cp, stock_count, cp, be, profit );

                //List_ActiveOrders.Add(activeOrder);

                //ThreadManager.LaunchTradingThread(name, stock_count, active_order_count_id, UpdateActiveOrderCurrentPrice, exchange);
                //dataGridView_ActiveOrderList.AutoGenerateColumns = false;
                //dataGridView_ActiveOrderList.DataSource = null;

                //dataGridView_ActiveOrderList.DataSource = List_ActiveOrders;
                //dataGridView_ActiveOrderList.AutoGenerateColumns = true;
                //active_order_count_id++;
            }

        }


        private void dataGridView_CompletedOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_MarketAnalysis_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                int rowid = e.RowIndex;
                if (rowid < 0)
                    return;

                //List<MarketAnalysisDataum> lsTemp = (List<MarketAnalysisDataum>)dataGridView_MarketAnalysis.DataSource;
                //var lsTemp = dataGridView_MarketAnalysis.DataSource;
                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                rows.Add(dataGridView_MarketAnalysis.Rows[rowid]);

                //DataGridViewRowCollection rows = new DataGridViewRowCollection( dataGridView_MarketAnalysis);

                float wma = float.Parse(rows[0].Cells["WMA"].Value.ToString());
                float ema = float.Parse(rows[0].Cells["EMA"].Value.ToString());
                float sma = float.Parse(rows[0].Cells["SMA"].Value.ToString());
                float close = float.Parse(rows[0].Cells["Close"].Value.ToString());
                float cp = float.Parse(rows[0].Cells["Current"].Value.ToString());

                bool bIsNR = bool.Parse(rows[0].Cells["IsNRDay"].Value.ToString());
                string ticker = rows[0].Cells["Ticker"].Value.ToString();
                double vol = double.Parse(rows[0].Cells["Volume"].Value.ToString());

                float high90 = float.Parse(rows[0].Cells["HighPrice90"].Value.ToString());
                float Low90 = float.Parse(rows[0].Cells["LowPrice90"].Value.ToString());

                //Scanner obscan = new Scanner(map.Count, ticker, bIsNR, wma, ema, sma, close, vol, 0, false);
                //UpdateScannerGridObject obscan = new UpdateScannerGridObject(mapScanner.Count, ticker, bIsNR, wma, ema, sma, close, vol, high90, Low90);
                //int default_algoID = Convert.ToInt32(AlgorithmType.BuyMedianPrice);
                UpdateScannerGridObject obscan = new UpdateScannerGridObject(mapScanner.Count, ticker, AlgorithmType.BuyMedianPrice, bIsNR, wma, ema, sma, close, vol, high90, Low90);

                //List_StocksUnderScanner.Add(obscan);
                try
                {
                    lock (mapScanner)
                    {
                        
                        ActiveOrder aoTemp = List_ActiveOrders.Where(a => a.Ticker.Equals(ticker)).FirstOrDefault();
                        if(null == aoTemp )
                        {
                            dataGridView_Scanner.DataSource = null;
                            mapScanner.Add(ticker, obscan);
                            var scanner_source = new BindingSource();
                            scanner_source.DataSource = mapScanner.Values;
                            dataGridView_Scanner.DataSource = scanner_source;

                            //Adding price polling only when it is in map. May be its a redundant.
                            List_EnqueueOrders.Clear();

                            //Creating a new list from map, because we are constantly updating map, a new Enqueue List.
                            foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in mapScanner)
                            {
                                string strTemp = List_EnqueueOrders.Where(a => a.Equals(kvp.Value.Ticker)).FirstOrDefault();
                                if (null == strTemp)
                                    List_EnqueueOrders.Add(kvp.Value.Ticker);
                            }
                            if (List_ActiveOrders.Count > 0)
                            {
                                foreach (ActiveOrder ob in List_ActiveOrders)
                                {
                                    string strTemp = List_EnqueueOrders.Where(a => a.Equals(ob.Ticker)).FirstOrDefault();
                                    if (null == strTemp)
                                        List_EnqueueOrders.Add(ob.Ticker);
                                }
                            }
                        }
                        else
                        {
                            var userResult = AutoClosingMessageBox.Show("Failed Condition: Multiple orders for same stock once ACTIVE cannot be placed.",
                                                                        "Error Reason", 3000, MessageBoxButtons.OK);
                            if (userResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                // do something
                            }
                        }
                        

                    }
                    
                }
                catch (System.ArgumentException ex)
                {
                }

                
                 
            }


        }

        private void dataGridView_MarketAnalysis_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgv = dataGridView_MarketAnalysis;
            //dataGridView_MarketAnalysis.DataBindingComplete += Sort;
            try
            {
                if (dgv.Columns[e.ColumnIndex].SortMode != DataGridViewColumnSortMode.NotSortable)
                {
                    if (e.ColumnIndex == newSortColumn)
                    {
                        if (newColumnDirection == ListSortDirection.Ascending)
                            newColumnDirection = ListSortDirection.Descending;
                        else
                            newColumnDirection = ListSortDirection.Ascending;
                    }

                    newSortColumn = e.ColumnIndex;

                    switch (newColumnDirection)
                    {
                        case ListSortDirection.Ascending:
                            dgv.Sort(dgv.Columns[newSortColumn], ListSortDirection.Ascending);
                            break;
                        case ListSortDirection.Descending:
                            dgv.Sort(dgv.Columns[newSortColumn], ListSortDirection.Descending);
                            break;
                    }
                }

            }
            catch (System.InvalidOperationException exc)
            {
                MessageBox.Show("List is not the source. Need to bind it first.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Stop);
            }
        }

        private void dataGridView_ActiveOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            { 
            var senderGrid = (DataGridView)sender;

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    int rowId = e.RowIndex;
                    //TODO - Button Clicked - Execute Code Here
                    int orderID = int.Parse(dataGridView_ActiveOrderList.Rows[rowId].Cells["OrderID"].Value.ToString());
                    //string purchase_time_for_orderID = dataGridView_ActiveOrderList.Rows[rowId].Cells["Buy_Time"].Value.ToString();
                    string ticker = dataGridView_ActiveOrderList.Rows[rowId].Cells["Ticker"].Value.ToString();
                    //var source = dataGridView_ActiveOrderList.DataSource;
                    //List<ActiveOrder> orderList = (List<ActiveOrder>)source;
                    //ActiveOrder tbdel = orderList.Where(a => a.OrderID.Equals(orderID)).FirstOrDefault();
                    ////orderList = orderList.Where(w => w != orderList[w].OrderID ).ToArray(); // deleting last
                    //orderList.Remove(orderList.Single(s => s.OrderID == orderID));
                    ActiveOrder tbdel = List_ActiveOrders.Where(a => a.Ticker.Equals(ticker)).FirstOrDefault();
                    lock (List_ActiveOrders)
                    {
                        List_ActiveOrders.RemoveAll(x => x.OrderID.Equals(orderID));
                    }

                    dataGridView_ActiveOrderList.DataSource = null;
                    dataGridView_ActiveOrderList.DataSource = List_ActiveOrders;


                    List_CompletedOrders.Add(new CompletedOrders(tbdel));

                    //1. Update UI
                    dataGridView_CompletedOrders.DataSource = null;
                    dataGridView_CompletedOrders.DataSource = List_CompletedOrders;

                    //2. Update DB
                    //UpdateDB_ActiveOrderTable();
                    //UpdateDB_CompletedOrderTable();
                }
            }
            catch(Exception ex)
            { }
        }

        private void button_ApplyFilter_Click(object sender, EventArgs e)
        {
            string filter = textBox_Filter.Text.Trim();

            dataGridView_MarketAnalysis.DataSource = null;

            //List<MarketAnalysisDataum> tmpList = List_RenderMarketData.Contains(List_RenderMarketData.Single(s => ((s.VWMA > s.Current )&& (s.Current > s.WMA))) );

            var tmpList = List_RenderMarketData.Where(item => ((item.VWMA > item.Current) && (item.Current > item.WMA))).ToList();
//            List < MarketAnalysisDataum > tmp = (List<MarketAnalysisDataum>)tmpList;

           // dataGridView_MarketAnalysis.DataSource = tmp;
        }

        private void button_ClearFilter_Click(object sender, EventArgs e)
        {
            dataGridView_MarketAnalysis.DataSource = null;
            dataGridView_MarketAnalysis.DataSource = List_RenderMarketData;
        }
        public static bool bIsRowAdded = false;
        private void dataGridView_Scanner_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void dataGridView_ActiveOrderList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            active_order_count_id++;

        }
    }
}

