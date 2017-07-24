using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    public class GHistory
    {

        public float Min { get; set; }
        public float Max { get; set; }
        public float Mean { get; set; }
        public float Median { get; set; }

        List<GHistoryDatum> History_list = null;// = new List<GHistoryDatum>();
        public void Flush_HistoryList()
        {
            Debug.Assert(History_list != null);
            History_list.Clear();
        }
        public GHistory(string exchange, string ticker, string sd, string ed, int interval, string num_of_days)
        {
            getTickerHistory(exchange, ticker, sd, ed, interval, num_of_days);
        }
        public int getHistoryCount()
        {
            return History_list == null ? 0 : History_list.Count;
        }

        public List<GHistoryDatum> GetGHistoryList() { return History_list;  }
        public void getTickerHistory(string exchange, string ticker, string sd, string ed, int interval, string num_of_days)
        {

            float min, max, mean, median;

            History_list = StringTypeParser.get_TickerObjectArray(
                                                  exchange, ticker, sd, ed, interval, num_of_days,
                                                 out min,
                                                 out max,
                                                 out mean,
                                                 out median
                                                );

            this.Min = min;
            this.Max = max;
            this.Mean = mean;
            this.Median = median;

            return;
        }
        private void getHistoryNSEData(string ticker, string sd, string ed, int interval)
        {

            float min, max, mean;

            History_list = StringTypeParser.get_NSE_TickerObjectArray(
                                                  ticker, sd,ed, interval,
                                                 out min,
                                                 out max,
                                                 out mean
                                                );

            this.Min = min;
            this.Max = max;
            this.Mean = mean;

        }

        private void getHistoryBSEData(string ticker, string sd, string ed)
        {
            float min, max, mean;

            History_list = StringTypeParser.get_BSE_TickerObjectArray(
                                                  ticker,sd,ed,
                                                 out min,
                                                 out max,
                                                 out mean
                                                );
            this.Min = min;
            this.Max = max;
            this.Mean = mean;


        }
    }
}
