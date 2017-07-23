using System;
using System.Windows.Forms;
using Winforms.Cartesian.BasicLine;
using Winforms.Cartesian.ConstantChanges;
using Winforms.Cartesian.Customized_Series;
using Winforms.Cartesian.DataPagination;
using Winforms.Cartesian.DateTime;
using Winforms.Cartesian.DynamicVisibility;
using Winforms.Cartesian.Events;
using Winforms.Cartesian.FullyResponsive;
using Winforms.Cartesian.Funnel_Chart;
using Winforms.Cartesian.GanttChart;
using Winforms.Cartesian.HeatSeriesExample;
using Winforms.Cartesian.Inverted_Series;
using Winforms.Cartesian.Irregular_Intervals;
using Winforms.Cartesian.Labels;
using Winforms.Cartesian.Linq;
using Winforms.Cartesian.LogarithmScale;
using Winforms.Cartesian.MissingPoints;
using Winforms.Cartesian.MultiAxes;
using Winforms.Cartesian.ScatterExample;
using Winforms.Cartesian.Sections;
using Winforms.Cartesian.StackedArea;
using Winforms.Cartesian.StepLine;
using Winforms.Cartesian.UielementsExample;
using Winforms.Cartesian.Zooming_and_Panning;
using Winforms.Gauge.AngularGauge;
using Winforms.Gauge._360;
using Winforms.Maps;
using Winforms.PieChart;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace Winforms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
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
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {5, 2, 8, 3},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
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
            //    PointForeground = Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            cartesianChart1.Series[2].Values.Add(5d);

        }

  /*      private void btnIObservable_Click(object sender, EventArgs e)
        {
            new FullyResponsive().ShowDialog();
        }

        private void btnLabels_Click(object sender, EventArgs e)
        {
            new Labels().ShowDialog();
        }

        private void btnSeries_Click(object sender, EventArgs e)
        {
            new CustomizedSeries().ShowDialog();
        }

        private void btnBasicLine_Click(object sender, EventArgs e)
        {
            new BasicLineExample().ShowDialog();
        }

        private void btnInvertedSeries_Click(object sender, EventArgs e)
        {
            new InvertedSeries().ShowDialog();
        }

        private void btnStackedArea_Click(object sender, EventArgs e)
        {
            new StackedAreaExample().ShowDialog();
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            new SectionsExample().ShowDialog();
        }

        private void btnIrregularIntervals_Click(object sender, EventArgs e)
        {
            new IrregularIntervalsExample().ShowDialog();
        }

        private void btnZoomingAndPanning_Click(object sender, EventArgs e)
        {
            new ZomingAndPanningExample().ShowDialog();
        }

        private void btnMissingPoints_Click(object sender, EventArgs e)
        {
            new MissingPoint().ShowDialog();
        }

        private void btnMultiAx_Click(object sender, EventArgs e)
        {
            new MultipleAxesExample().ShowDialog();
        }

        private void btnDateTime_Click(object sender, EventArgs e)
        {
            new DateTimeExample().ShowDialog();
        }

        private void btnLogScale_Click(object sender, EventArgs e)
        {
            new LogarithmSacale().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ConstantChanges().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new PieChartExample().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new DynamicVisibiltyExample().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new DataPaginationExample().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new HeatSeriesExample().ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Gauge360Example().ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new DoughnutExample().ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new LinqExample().ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new UielementsExample().ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new AngularGugeForm().ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            new ScatterForm().ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            new GeoMapExample().ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            new StepLineExample().ShowDialog();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            new GanttExample().ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            new FunnelExample().ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            new EventsExample().ShowDialog();
        }*/

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
