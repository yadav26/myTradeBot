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

namespace MainForm
{
    public partial class Form1 : Form
    {
        public static WebBrowser wbBrowser = null;
        public static int place_orders_count = 0;
        string Stylefile =string.Empty;
        System.Data.DataTable gDataTable = null;

        private System.Data.DataSet dataSet;

        public Form1()
        {
            InitializeComponent();
            splitContainer1.Orientation = Orientation.Vertical;
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel1.Controls.Add(dataGridView_tradeLists);
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
            splitContainer2.Panel1.Controls.Add(cartesianChart1);


            //char separator = Path.DirectorySeparatorChar;
            //string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            //string[] pathItems = startupPath.Split(separator);
            //string projectPath = string.Join(separator.ToString(),
            //    pathItems.Take(pathItems.Length - 3));


            //Stylefile = projectPath + Path.Combine(projectPath, "\\Content\\Style\\bootstrap.min.css");
            //string jquery_script = projectPath + Path.Combine(projectPath, "\\Content\\Script\\jquery.min.js");
            //string bootstrap_script = projectPath+ Path.Combine(projectPath, "\\Content\\Script\\bootstrap.min.js");


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

        private void Form1_Load(object sender, EventArgs e)
        {
            //fill algorithm selection combo box

            comboBox_Algorithms.SelectedIndex = 0;

            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> {4, 6, 5, 2, 7}
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {6, 7, 3, 4, 6},
                    PointGeometry = null
                }
                /*,
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {5, 2, 8, 3},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }*/
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Month",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Sales",
                LabelFormatter = value => value.ToString("C")
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;

            //modifying the series collection will animate and update the chart
            //cartesianChart1.Series.Add(new LineSeries
            //{
            //    Values = new ChartValues<double> { 5, 3, 2, 4, 5 },
            //    LineSmoothness = 0, //straight lines, 1 really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = System.Windows.Media.Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            //cartesianChart1.Series[2].Values.Add(5d);


            //   cartesianChart1.DataClick += CartesianChart1OnDataClick;

            // Use the Text property for the button text for all cells rather
            // than using each cell's value as the text for its own button.

        }

        private void button_start_trade_Click(object sender, EventArgs e)
        {
            //Launch the new thread
            if(textBox_ticker.Text.Trim() == "" )
            {
               // textBox_ticker.BackColor = Color.Red;

                return;
            }


            if (textBox_stock_num.Text.Trim() == ""  )
            {
               // textBox_stock_num.BackColor = Color.Tomato;

                return;
            }
            try
            {
                int stock_count = int.Parse(textBox_stock_num.Text);
                ThreadManager.LaunchTradingThread(textBox_ticker.Text, stock_count, place_orders_count );
                //Add the entry in data grid

                place_orders_count++;

                this.dataGridView_tradeLists.Rows.Add("",place_orders_count, textBox_ticker.Text, "35021.76", stock_count, DateTime.Now.ToString() );
                
            }
            catch( FormatException fe)
            {
                //textBox_stock_num.BackColor = Color.YellowGreen;
                return;
            }

          
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView_tradeLists_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && dataGridView_tradeLists.Columns[e.ColumnIndex].Name == "Exit")
            {
                int rowIndex = Convert.ToInt32(e.RowIndex); // Get the current row
                int bigStore = Convert.ToInt32(dataGridView_tradeLists.Rows[rowIndex].Cells[1].Value);

                ThreadManager.ExitTradingThread(rowIndex);
              

                dataGridView_tradeLists.Rows[rowIndex].ReadOnly = true;

            }
            else
            {

                /*

                StringBuilder htmlTable = new StringBuilder();
                htmlTable.Append("<table border='1'>");
                htmlTable.Append("<tr style='background-color:green; color: White;'><th>Customer ID.</th><th>Name</th><th>Address</th><th>Contact No</th></tr>");

                //if (!object.Equals(ds.Tables[0], null))
                {
                    //if (ds.Tables[0].Rows.Count > 0)
                    {

                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            htmlTable.Append("<tr style='color: White;'>");
                            htmlTable.Append("<td>" + "MyCustID" + "</td>");
                            htmlTable.Append("<td>" + "MyName" + "</td>");
                            htmlTable.Append("<td>" + "MyhomeAddress" + "</td>");
                            htmlTable.Append("<td>" + "MyContactNo" + "</td>");
                            htmlTable.Append("</tr>");
                        }
                        htmlTable.Append("</table>");
                        //DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
                    }
                    //else
                    //{
                    //    htmlTable.Append("<tr>");
                    //    htmlTable.Append("<td align='center' colspan='4'>There is no Record.</td>");
                    //    htmlTable.Append("</tr>");
                    //}
                }


                //wbBrowser.Document.GetElementById("first").OuterText = htmlTable.ToString();

                //Draw Graph
                var htmlstring = @"<!DOCTYPE html>
                                <html lang='en'>
                                <head>
                                  <title>Bootstrap Example</title>
                                  <meta charset='utf-8'>
                                  <meta name='viewport' content='width=device-width, initial-scale=1'>
                                  <link rel='stylesheet' href='" + Stylefile +
                                      @"'><script src='
                                      '></script> <script src=''></script>
                                </head>
                                <body>

                                <div class='container'>
                                   
                                </div>
                                    <form id='form1' runat='server'>  
                                        <table style='width: 50%; text-align: center; background-color: skyblue;'>  
                                            <tr>  
                                                <td align='center'>  " + htmlTable.ToString() + @"
                                                    <div id='first' runat='server'></div>  
                                                </td>  
                                            </tr>  
                                        </table>  
                                    </form>  
                                </body>
                                </html>";
                wbBrowser.DocumentText = htmlstring;
                wbBrowser.Refresh();
                */
                MakeParentTable();
                //MakeChildTable();
                MakeDataRelation();
                BindToDataGrid();

            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ThreadManager.CleanUpAllThreads();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
