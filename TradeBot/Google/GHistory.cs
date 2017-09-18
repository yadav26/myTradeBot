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

        private  List<StringParsedData> history_list = null;

        public List<StringParsedData> History_list { get { return history_list; } set { history_list = value; } }

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
            List<StringParsedData> tmp = new List<StringParsedData>();
            tmp = StringTypeParser.Get_gAPI_ListData(exchange, ticker, interval, num_of_days);

            History_list = tmp;

            if (History_list == null)
                return;
            //lock (History_list)
            {
          
                //lock(tmp)
                {
                    foreach (StringParsedData obj in tmp)
                    {
                        if (obj == null)
                            continue;

                        if (obj.High > max)
                            max = obj.High;

                        if (obj.Low < min)
                            min = obj.Low;
                    }
                }


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
