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


        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public static List<NRN_RAGE> List_NRange { get; set; }

        public float TodayRange { get; set; }
        public float TodayNHigh { get; set; }
        public float TodayNLow { get; set; }

        public bool bTodayIsNRDay { get; set; }

        public Algorithm_NRN(string exch, string ticker, int periods )
        {

            Debug.Assert(periods != 0);

            Ticker = ticker;

            Exchange = exch;


            //Calculating past N Dates format
            string sd = DateTime.Now.AddDays((periods * (-1))).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";

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

            int seconds_in_day = 60 * 60 * 24;

            List<StringParsedData> ls_gParsedData = StringTypeParser.get_TickerAllData(Exchange, Ticker, sd, ed, seconds_in_day, periods);
            if (null == ls_gParsedData)
            {
                return;
            }


            if (null == List_NRange)
                List_NRange = new List<NRN_RAGE>();

            List_NRange.Clear();

            bool bIsTodayNRNDay = false;



            float smallest_NRN = 100000000.0f;

            foreach (StringParsedData datum in ls_gParsedData)
            {
                NRN_RAGE objNR = new NRN_RAGE();
                objNR.DayDate = datum.DateDay;
                objNR.Exchange = Exchange;
                objNR.Ticker = Ticker;
                objNR.DayHigh = datum.High;
                objNR.DayLow = datum.Low;
                objNR.Range = objNR.DayHigh - objNR.DayLow;

                if (smallest_NRN > objNR.Range)
                    smallest_NRN = objNR.Range;

                objNR.NRN_Period = periods;
                List_NRange.Add(objNR);
            }


            this.TodayRange = GetTodayNRange(Exchange, Ticker, sd, ed, 60, 1);

            if (this.TodayRange < smallest_NRN)
                bIsTodayNRNDay = true;

            this.bTodayIsNRDay = bIsTodayNRNDay;
           

        } // constructor end.

        private float GetTodayNRange(string exchange, string ticker, string sd, string ed, int seconds_in_day, int periods)
        {

            float highest = -1.0f;
            float lowest = 100000.0f;

            List<StringParsedData> ls_gTodayParsedData = StringTypeParser.get_TickerAllData(Exchange, Ticker, sd, ed, seconds_in_day, periods);
            if (null == ls_gTodayParsedData)
            {
                return 0;
            }

            foreach (StringParsedData datum in ls_gTodayParsedData)
            {
                if (highest < datum.High)
                    highest = datum.High;
                if (lowest > datum.Low)
                    lowest = datum.Low;

            }

            return (highest - lowest);
            throw new NotImplementedException();
        }
    }
}
