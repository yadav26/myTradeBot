using System;
using System.Collections.Generic;
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


        List<GHistoryDatum> History_list = new List<GHistoryDatum>();

        public GHistory(string exchange, string ticker, string sd, string ed, int interval)
        {
            getTickerHistory(exchange, ticker, sd, ed, interval);
        }
        public int getHistoryCount()
        {
            return History_list.Count;
        }
        public void getTickerHistory(string exchange, string ticker, string sd, string ed, int interval)
        {

            //DateTime sDate = DateTime.Parse(sd);
            //DateTime eDate = DateTime.Parse(ed);
            //if (sDate > eDate)
            //    return;
            float min, max, mean;

            History_list = StringTypeParser.get_TickerObjectArray(
                                                  exchange, ticker, sd, ed, interval,
                                                 out min,
                                                 out max,
                                                 out mean
                                                );

            this.Min = min;
            this.Max = max;
            this.Mean = mean;

            //if (exchange == "NSE")
            //    getHistoryNSEData(ticker, sd, ed, interval );
            //else if (exchange == "BSE")
            //    getHistoryBSEData(ticker, sd, ed);
            //else if ( exchange == "NASDAQ")

            //    return;

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
