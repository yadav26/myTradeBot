using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    public class Daily_Reader
    {

        public float TodayMin { get; set; }
        public float TodayMax { get; set; }
        public float TodayMean { get; set; }
        public float TodayMedian { get; set; }

        public void parser(string exchange , string ticker, int interval, string num_of_days )
        {
            GHistory hs = new GHistory(exchange, ticker, "", "", interval, num_of_days);
            int count = hs.getHistoryCount();
            this.TodayMin = hs.Min;
            this.TodayMax = hs.Max;
            this.TodayMean = hs.Mean;
            this.TodayMedian = hs.Median;
        }
    }
}
