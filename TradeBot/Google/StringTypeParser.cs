using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    class StringTypeParser
    {

        //https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=600   
        //https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=60&p=5d&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218
        //https://www.google.com/finance/getprices?q="+ <ticker> + "&x=" + <ExChange> + "&i="+<interval>+"&p=5d&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218"


        public static string quandl_path = @"https://www.google.com/finance/getprices?q=";
        private static string quandl_exch_nse = @"NSE";
        private static string quandl_exch_bse = @"BSE";

        private static string sep = "&i=" ;
        private static string quanl_api_key = "&p=5d&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218";

        public static List<GHistoryDatum> History_list = new List<GHistoryDatum>();


        public static List<GHistoryDatum> get_TickerObjectArray(
                                                                    string exchange,
                                                                    string ticker,
                                                                    string sd,
                                                                    string ed,
                                                                    int interval,
                                                                    out float min,
                                                                    out float max,
                                                                    out float mean
                                                                )
        {
            min = 0.0f; max = 0.0f; mean = 0.0f;

            //start_date=2017-01-01&end_date=2017-07-10
            string intstr = string.Format("{0}", interval);

            string api_fetch_string = quandl_path + ticker + "&x=" + exchange + sep + intstr + quanl_api_key;


            try
            {


                using (WebClient wc = new WebClient())
                {
                    string jWebString = wc.DownloadString(api_fetch_string);


                    string[] strarray = jWebString.Split(new[] { "TIMEZONE_OFFSET" }, StringSplitOptions.None);

                    string[] data = strarray[1].Split(new[] { "\n" }, StringSplitOptions.None);
                    //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3
                    float closing_total = 0.0f;


                    data = data.Where(w => w != data[data.Length - 1]).ToArray();
                    data = data.Where(w => w != data[0]).ToArray();


                    foreach (string str in data)
                    {
                        GHistoryDatum hd = new GHistoryDatum();
                        string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
                        hd.Date = entity[0].Replace(string.Format("\""), "");
                        hd.Close = entity[1];
                        hd.High = entity[2];
                        hd.Low = entity[3];
                        hd.Open = entity[4];

                        if (min == 0.0f)
                            min = float.Parse(hd.Low);
                        if (max == 0.0f)
                            max = float.Parse(hd.High);

                        if (min > float.Parse(hd.Low))
                            min = float.Parse(hd.Low);

                        if (max < float.Parse(hd.High))
                            max = float.Parse(hd.High);

                        // Console.WriteLine("Low :" + hd.Low + ", High :" + hd.High +", Last :" + hd.Last  );

                        closing_total += float.Parse(hd.Close);

                        History_list.Add(hd);
                    }
                    mean = closing_total / (float)(History_list.Count);

                }

            }
            catch (WebException ex)
            {
                // id = string.Empty;

            }

            return History_list;
        }


        public static List<GHistoryDatum> get_NSE_TickerObjectArray(
                                                 string ticker,
                                                 string sd,
                                                 string ed,
                                                 int interval,
                                              out float min,
                                                out float max,
                                                out float mean
                                                )
        {
            min = 0.0f; max = 0.0f; mean = 0.0f;

            //start_date=2017-01-01&end_date=2017-07-10
            string intstr = string.Format("{0}", interval);

            string api_fetch_string = quandl_path + ticker + "&x="+quandl_exch_nse + sep + intstr + quanl_api_key;


            try
            {


                using (WebClient wc = new WebClient())
                {
                    string jWebString = wc.DownloadString(api_fetch_string);


                    string[] strarray = jWebString.Split(new[] { "TIMEZONE_OFFSET" }, StringSplitOptions.None);

                    string[] data = strarray[1].Split(new[] { "\n" }, StringSplitOptions.None);
                    //foreach (string str in data)
                    //{
                    //    // GHistoryDatum hd = new GHistoryDatum();
                    //    string[] entity = data.Split(new[] { "," }, StringSplitOptions.None);
                    //    this. = entity[0].Replace(string.Format("\""), "");
                    //    hd.Open = entity[1];
                    //    hd.High = entity[2];
                    //    hd.Low = entity[3];
                    //    hd.Last = entity[4];
                    //    hd.Close = entity[5];
                    //}

                    // string[] header_entities = getCleanHeaderString(strarray[0]);


                    //id = parse_header("id", header_entities).Replace(string.Format("\""), "");
                    //dataset_code = parse_header("dataset_code", header_entities).Replace(string.Format("\""), "");
                    //database_code = parse_header("database_code", header_entities).Replace(string.Format("\""), "");
                    //name = parse_header("name", header_entities).Replace(string.Format("\""), "");
                    //description = parse_header("description", header_entities).Replace(string.Format("\""), "");
                    //isin = parse_header("ISIN", header_entities).Replace(string.Format("\""), "");
                    //start_date = parse_header("start_date", header_entities).Replace(string.Format("\""), "");
                    //end_date = parse_header("end_date", header_entities).Replace(string.Format("\""), "");



                    //string to_chop1 = ":[[";
                    //strarray[1] = strarray[1].Remove(0, to_chop1.Length);
                    //string[] data = strarray[1].Split(new[] { "],[" }, StringSplitOptions.None);
                    //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3
                    float closing_total = 0.0f;

                    
                    data = data.Where(w => w != data[data.Length -1]).ToArray();
                    data = data.Where(w => w != data[0]).ToArray();


                    foreach (string str in data)
                    {
                        GHistoryDatum hd = new GHistoryDatum();
                        string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
                        hd.Date = entity[0].Replace(string.Format("\""), "");
                        hd.Close = entity[1];
                        hd.High = entity[2];
                        hd.Low = entity[3];
                        hd.Open = entity[4];

                        if (min == 0.0f)
                            min = float.Parse(hd.Low);
                        if (max == 0.0f)
                            max = float.Parse(hd.High);

                        if (min > float.Parse(hd.Low))
                            min = float.Parse(hd.Low);

                        if (max < float.Parse(hd.High))
                            max = float.Parse(hd.High);

                        // Console.WriteLine("Low :" + hd.Low + ", High :" + hd.High +", Last :" + hd.Last  );

                        closing_total += float.Parse(hd.Close);

                        History_list.Add(hd);
                    }
                    mean = closing_total / (float)(History_list.Count);

                }

            }
            catch (WebException ex)
            {
               // id = string.Empty;

            }

            return History_list;
        }



        public static List<GHistoryDatum> get_BSE_TickerObjectArray(
                                         string ticker,
                                                 string sd,
                                                 string ed,
                                           out float min,
                                        out float max,
                                        out float mean
                                        )
        {
            min = 0.0f; max = 0.0f; mean = 0.0f;

            //start_date=2017-01-01&end_date=2017-07-10


            string api_fetch_string = quandl_path + ticker + "&x=" + quandl_exch_nse + quanl_api_key;


            try
            {


                using (WebClient wc = new WebClient())
                {
                    string jWebString = wc.DownloadString(api_fetch_string);


                    string[] strarray = jWebString.Split(new[] { "TIMEZONE_OFFSET" }, StringSplitOptions.None);

                    string[] data = strarray[1].Split(new[] { "\n" }, StringSplitOptions.None);
                    //foreach (string str in data)
                    //{
                    //    // GHistoryDatum hd = new GHistoryDatum();
                    //    string[] entity = data.Split(new[] { "," }, StringSplitOptions.None);
                    //    this. = entity[0].Replace(string.Format("\""), "");
                    //    hd.Open = entity[1];
                    //    hd.High = entity[2];
                    //    hd.Low = entity[3];
                    //    hd.Last = entity[4];
                    //    hd.Close = entity[5];
                    //}

                    // string[] header_entities = getCleanHeaderString(strarray[0]);


                    //id = parse_header("id", header_entities).Replace(string.Format("\""), "");
                    //dataset_code = parse_header("dataset_code", header_entities).Replace(string.Format("\""), "");
                    //database_code = parse_header("database_code", header_entities).Replace(string.Format("\""), "");
                    //name = parse_header("name", header_entities).Replace(string.Format("\""), "");
                    //description = parse_header("description", header_entities).Replace(string.Format("\""), "");
                    //isin = parse_header("ISIN", header_entities).Replace(string.Format("\""), "");
                    //start_date = parse_header("start_date", header_entities).Replace(string.Format("\""), "");
                    //end_date = parse_header("end_date", header_entities).Replace(string.Format("\""), "");



                    //string to_chop1 = ":[[";
                    //strarray[1] = strarray[1].Remove(0, to_chop1.Length);
                    //string[] data = strarray[1].Split(new[] { "],[" }, StringSplitOptions.None);
                    //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3
                    float closing_total = 0.0f;


                    data = data.Where(w => w != data[data.Length - 1]).ToArray();
                    data = data.Where(w => w != data[0]).ToArray();


                    foreach (string str in data)
                    {
                        GHistoryDatum hd = new GHistoryDatum();
                        string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
                        hd.Date = entity[0].Replace(string.Format("\""), "");
                        hd.Close = entity[1];
                        hd.High = entity[2];
                        hd.Low = entity[3];
                        hd.Open = entity[4];

                        if (min == 0.0f)
                            min = float.Parse(hd.Low);
                        if (max == 0.0f)
                            max = float.Parse(hd.High);

                        if (min > float.Parse(hd.Low))
                            min = float.Parse(hd.Low);

                        if (max < float.Parse(hd.High))
                            max = float.Parse(hd.High);

                        // Console.WriteLine("Low :" + hd.Low + ", High :" + hd.High +", Last :" + hd.Last  );

                        closing_total += float.Parse(hd.Close);

                        History_list.Add(hd);
                    }
                    mean = closing_total / (float)(History_list.Count);

                }

            }
            catch (WebException ex)
            {
                // id = string.Empty;

            }

            return History_list;
        }


    }
}
