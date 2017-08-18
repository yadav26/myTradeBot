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

        List<StringParsedData> History_list = null;// = new List<GHistoryDatum>();
        public void Flush_HistoryList()
        {
            Debug.Assert(History_list != null);
            History_list.Clear();
        }
        public GHistory(string exchange, string ticker, string sd, string ed, int interval, int num_of_days)
        {
            getTickerHistory(exchange, ticker, sd, ed, interval, num_of_days );
        }
        public int getHistoryCount()
        {
            return History_list == null ? 0 : History_list.Count;
        }

        public List<StringParsedData> GetGHistoryList() { return History_list;  }
        public void getTickerHistory(string exchange, string ticker, string sd, string ed, int interval, int num_of_days)
        {

            float min=99999999, max=0;

            History_list = StringTypeParser.Get_gAPI_ListData( exchange, ticker, interval, num_of_days );
            if (History_list == null)
                return;

            foreach( StringParsedData obj in History_list)
            {
                if (obj.High > max)
                    max = obj.High;

                if (obj.Low < min)
                    min = obj.Low;
            }
            this.Max = max;
            this.Min = min;
            //this.Min = min;
            //this.Max = max;
            //this.Mean = mean;
            //this.Median = median;

            return;
        }



    }
}
