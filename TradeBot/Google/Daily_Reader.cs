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
        GHistory hs = null;
        public float TodayMin { get; set; }
        public float TodayMax { get; set; }
        public float TodayMean { get; set; }
        public float TodayMedian { get; set; }
        public List<StringParsedData> GetGHistoryList() { return hs == null ?  null: hs.GetGHistoryList(); }
        public void parser(string exchange , string ticker, int interval, int num_of_days )
        {
            hs = new GHistory(exchange, ticker, "", "", interval, num_of_days);
            int count = hs.getHistoryCount();
            this.TodayMin = hs.Min;
            this.TodayMax = hs.Max;
            this.TodayMean = hs.Mean;
            this.TodayMedian = hs.Median;
        }
        public void Flush_HistoryList()
        {
            hs.Flush_HistoryList();
            
        }
    }
}
