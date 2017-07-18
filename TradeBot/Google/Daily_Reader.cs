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

        public void parser(string exchange , string ticker)
        {
            GHistory hs = new GHistory(exchange, ticker, "", "", 600);
            int count = hs.getHistoryCount();
            this.TodayMin = hs.Min;
            this.TodayMax = hs.Max;
            this.TodayMean = hs.Mean;
        }
    }
}
