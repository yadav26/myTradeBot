using Google;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    public class NRN_RAGE
    {
        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public int NRN_Period { get; set; }
        public float DayHigh { get; set; }
        public float DayLow { get; set; }
        public int DayDate { get; set; }
        public float Range { get; set; }

    }


    public class Algorithm_NRN
    {
        //How to Find NR7 day..
        //1) Get the High and Low data of last few period
        //2) Calculate the range of each day i.e.high - low) for each day
        //3) Compare the range of a today and previous 6 days range(to get NR7.To get NR4 get last 3 days range)
        //4) If today's range is smallest of all 7 days, then today is NR7 day..else not.
        //It is that simple.
        //This is one of my favorite setup.It gives u a chance to be ahead of trade follower / indicator followers 
        //who will jump in the trend after you.
        //One of the easiest way to trade this setup will be to go long above the Day's high of NR7 day with stop at the Day's Low of NR7 day.
        //Or
        //Go short below the Day's Low of NR7 day with stop at the Day's High of NR7 day.



        public float TodayRange { get; set; }
        public float TodayNHigh { get; set; }
        public float TodayNLow { get; set; }

        public bool bTodayIsNRDay { get; set; }

        public Algorithm_NRN(SortedDictionary<int, StringParsedData> map, string Exchange, string Ticker, int periods, out float cp )
        {

            Debug.Assert(periods != 0);



            //Calculating past N Dates format
           // string sd = DateTime.Now.AddDays((periods * (-1))).ToString("yyyy-M-d");
           // string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";

            ///
            /// Will get data for 90days 
            /// https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=86400&p=90d&f=d,c
            ///

            //QHistory qhsNewObj = new QHistory(Exchange, Ticker, sd, ed);
            //List<QHistoryDatum> qHsList = qhsNewObj.GetQHistoryDatumList();
            //int lastIndex = qHsList.Count - 1;
            //foreach (QHistoryDatum qdatum in qHsList)
            //{
            //   //
            //}

            //int seconds_in_day = 60 * 60 * 24;

            //List<StringParsedData> ls_gParsedData = StringTypeParser.Get_gAPI_ListData(Exchange, Ticker, sd, ed, seconds_in_day, periods);
            //if (null == ls_gParsedData)
            //{
            //    return;
            //}

            bool brange = false;

            float smallest_NRN = 100000000.0f;

            foreach (KeyValuePair<int, StringParsedData> kvp in map )
            {
                StringParsedData datum = kvp.Value;
                float range = datum.High - datum.Low;

                if (smallest_NRN > range)
                    smallest_NRN = range;

            }


            this.TodayRange = GetTodayNRange(Exchange, Ticker, 60, 1, out cp);

            if (this.TodayRange < smallest_NRN)
                brange = true;

            this.bTodayIsNRDay = brange;
           

        } // constructor end.

        private float GetTodayNRange(string exchange, string ticker, int seconds_in_day, int periods, out float cp )
        {

            float highest = -1.0f;
            float lowest = 100000.0f;

            List<StringParsedData> ls_gTodayParsedData = StringTypeParser.Get_gAPI_ListData(exchange, ticker, seconds_in_day, periods);
            if (null == ls_gTodayParsedData)
            {
                cp = 0;
                return 0;
            }

            //Assign latest close price of this ticker.
            cp = ls_gTodayParsedData[0].Close;
            lock (ls_gTodayParsedData)
            {
                foreach (StringParsedData datum in ls_gTodayParsedData)
                {
                    if (datum == null)
                        continue;

                    if (highest < datum.High)
                        highest = datum.High;
                    if (lowest > datum.Low)
                        lowest = datum.Low;

                }
            }
            

            return (highest - lowest);
            throw new NotImplementedException();
        }
    }
}
