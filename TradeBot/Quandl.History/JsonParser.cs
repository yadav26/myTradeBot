using Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quandl_FetchInterface
{
    public class JsonParser
    {

        //https://www.quandl.com/api/v3/datasets/NSE/SBIN.json?start_date=2017-01-01&end_date=2017-07-10&api_key=G5DQsRtXsqqGZ5kY8kwU
        public static string quandl_path = @"https://www.quandl.com/api/v3/datasets/";
        private static string quandl_exch_nse = @"nse";
        //private static string quandl_exch_bse = @"bse";
        private static string quandl_ext = ".json?";
        private static string apikey = "api_key=";
        private static string key = "G5DQsRtXsqqGZ5kY8kwU";
        private static string quanl_api_key = apikey + key;

        public static List<QHistoryDatum> History_list = new List<QHistoryDatum>();

        private static string parse_header(string key, string[] header_entities)
        {
            string[] retVal = { string.Empty };
            foreach (string x in header_entities)
            {
                if (x.Contains(key))
                {
                    retVal = x.Split(new[] { ":" }, StringSplitOptions.None);
                    break;
                }
            }
            return retVal[retVal.Length - 1];
        }

        private static string [] getCleanHeaderString( string jSonStr)
        {

            string[] strarray = jSonStr.Split(new[] { "\"data\"" }, StringSplitOptions.None);
            string to_chop = "{\"dataset\":{";
            string header = strarray[0].Replace(string.Format(@"\u003cbr\u003e"), string.Format(@"<br />")).Remove(0, to_chop.Length);

            string[] header_entities = header.Split(new[] { "," }, StringSplitOptions.None);
            return header_entities;
        }

        public static SortedDictionary<int, StringParsedData> Get_gAPI_MapStringObject(
                                                                        string e,
                                                                        string t,
                                                                        int i,
                                                                        int n
                                                                     )
        {
            SortedDictionary<int, StringParsedData> map = new SortedDictionary<int, StringParsedData>();

            string sd = DateTime.Now.AddDays((n * (-1))).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";


            List<StringParsedData> collection_data = Get_ListStringDataObject(e, t, sd, ed);

            foreach (StringParsedData obj in collection_data)
            {
                map.Add(obj.DateDay, obj);
            }

            return map;
        }


        public static List<StringParsedData> Get_ListStringDataObject( string e, string t, string sd, string ed )
        {
            List<StringParsedData> coll_objectStringData = new List<StringParsedData>();

            string id;
            string dataset_code;
            string database_code;
            string name;
            string description;
            string isin;
            string start_date;
            string end_date;

            float min, max, mean;
            float tv_min = 0.0f, tv_max = 0.0f;
            double tv = 0;

            switch (t)
            {
                case "M&M":
                    t = "MM";
                    break;
                case "BAJAJ-AUTO":
                    t = "BAJAJ_AUTO";
                    break;
                case "L&TFH":
                    t = "LTFH";
                    break;
                case "M&MFIN":
                    t = "MMFIN";
                    break;
                case "MCDOWELL-N":
                    t = "MCDOWELL_N";
                    break;
                default:
                    break;
            }


            


            List<QHistoryDatum> List_QHDatum = get_TickerObjectArray(
                                                  e,
                                                  t,
                                                  sd,
                                                  ed,
                                                 out id,
                                                 out dataset_code,
                                                 out database_code,
                                                 out name,
                                                 out description,
                                                 out isin,
                                                 out start_date,
                                                 out end_date,
                                                 out min,
                                                 out max,
                                                 out mean,
                                                 out tv_min,
                                                 out tv_max,
                                                 out tv
                                                );
            int counter = 0;
            foreach( QHistoryDatum qhd in List_QHDatum)
            {
                StringParsedData spd = new StringParsedData();
                spd.Close = float.Parse(qhd.Close);
                spd.Exchange = e;
                spd.Cdays = 0;// float.Parse(qhd.Date);
                spd.dateTime = DateTime.Parse(qhd.Date);
                spd.High = float.Parse(qhd.High);
                spd.Open = float.Parse(qhd.Open);
                spd.Ticker = t;
                spd.Low = float.Parse(qhd.Low);
                spd.Volume = double.Parse(qhd.Total_Trade_Quantity);
                spd.DateDay = counter;
                counter++;

                coll_objectStringData.Add( spd);

            }

            return coll_objectStringData;
        }


        public static List<QHistoryDatum> get_TickerObjectArray(
                                                 string exchange,
                                                 string ticker,
                                                 string sd,
                                                 string ed,
                                                out string id,
                                                out string dataset_code,
                                                out string database_code,
                                                out string name,
                                                out string description,
                                                out string isin,
                                                out string start_date,
                                                out string end_date,
                                                out float min,
                                                out float max,
                                                out float mean,
                                                out float tv_min,
                                                out float tv_max, 
                                                out double tv
                                                )
        {
            min = 0.0f; max = 0.0f; mean = 0.0f;
            tv_min = 0.0f; tv_max = 0.0f; tv = 0;

            //start_date=2017-01-01&end_date=2017-07-10

            string timeline = string.Empty;
            if (sd != "" && ed != "")
                timeline = "start_date=" + sd + "& end_date=" + ed + "&"; // enddate > startdate

            if ("NASDAQ" == exchange) // an exception ; it has to be wiki ["Date","Open","High","Low","Close","Volume","Ex-Dividend","Split Ratio","Adj. Open","Adj. High","Adj. Low","Adj. Close","Adj. Volume"]
                                                                        //["Date","Open","High","Low","Last","Close","Total Trade Quantity","Turnover (Lacs)"]
                exchange = "WIKI";


            string api_fetch_string = quandl_path + exchange + @"/" + ticker + quandl_ext + timeline + quanl_api_key;

            id = string.Empty;
            dataset_code = string.Empty;
            database_code = string.Empty;

            name = string.Empty;
            description = string.Empty;
            isin = string.Empty;
            start_date = string.Empty;
            end_date = string.Empty;

            try
            {


                using (WebClient wc = new WebClient())
                {
                    string jSonStr = wc.DownloadString(api_fetch_string);

                    string[] strarray = jSonStr.Split(new[] { "\"data\"" }, StringSplitOptions.None);

                    string[] header_entities = getCleanHeaderString(strarray[0]);


                    id = parse_header("id", header_entities).Replace(string.Format("\""), "");
                    dataset_code = parse_header("dataset_code", header_entities).Replace(string.Format("\""), "");
                    database_code = parse_header("database_code", header_entities).Replace(string.Format("\""), "");
                    name = parse_header("name", header_entities).Replace(string.Format("\""), "");
                    description = parse_header("description", header_entities).Replace(string.Format("\""), "");
                    isin = parse_header("ISIN", header_entities).Replace(string.Format("\""), "");
                    start_date = parse_header("start_date", header_entities).Replace(string.Format("\""), "");
                    end_date = parse_header("end_date", header_entities).Replace(string.Format("\""), "");
                    


                    string to_chop1 = ":[[";
                    strarray[1] = strarray[1].Remove(0, to_chop1.Length);
                    string[] data = strarray[1].Split(new[] { "],[" }, StringSplitOptions.None);
                    //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3
                    float closing_total = 0.0f;

                    bool IsThisOnTop = false;
                    History_list.Clear();

                    foreach (string str in data)
                    {
                        QHistoryDatum hd = new QHistoryDatum();
                        string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
                        hd.Date = entity[0].Replace(string.Format("\""), "");
                        hd.Open = entity[1];
                        hd.High = entity[2];
                        hd.Low = entity[3];
                        //hd.Last = entity[4];
                        if (exchange == "WIKI")
                            hd.Close = entity[4];
                        else
                            hd.Close = entity[5];

                        hd.Total_Trade_Quantity = entity[6];
                        hd.Turn_Over = entity[7];
                        if (min == 0.0f)
                            min = float.Parse(hd.Low);
                        if (max == 0.0f)
                            max = float.Parse(hd.High);

                        if (min > float.Parse(hd.Low))
                            min = float.Parse(hd.Low);

                        if (max < float.Parse(hd.High))
                            max = float.Parse(hd.High);

                        if (tv_min == 0 || tv_min > float.Parse(hd.Total_Trade_Quantity))
                            tv_min = float.Parse(hd.Total_Trade_Quantity);

                        if (tv_max < float.Parse(hd.Total_Trade_Quantity))
                            tv_max = float.Parse(hd.Total_Trade_Quantity);

                        if (false == IsThisOnTop)
                        {
                            tv =double.Parse( hd.Total_Trade_Quantity);
                            IsThisOnTop = true;
                        }
                            // Console.WriteLine("Low :" + hd.Low + ", High :" + hd.High +", Last :" + hd.Last  );

                            closing_total += float.Parse(hd.Close);

                        History_list.Add(hd);
                    }
                    mean = closing_total / (float)(History_list.Count);

                }

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                    {
                        //Handle it
                        Console.WriteLine("End resp.StatusCode ==>" + api_fetch_string);
                    }
                }
                //Handle it
                return null; 
            }
            return History_list;
        }


        //public static List<QHistoryDatum> get_NSE_TickerObjectArray( 
        //                                         string ticker, 
        //                                         string sd, 
        //                                         string ed,
        //                                        out string id,
        //                                        out string dataset_code,
        //                                        out string database_code,
        //                                        out string name,
        //                                        out string description,
        //                                        out string isin,
        //                                        out string start_date,
        //                                        out string end_date,
        //                                        out float min,
        //                                        out float max,
        //                                        out float mean
        //                                        )
        //{
        //    min = 0.0f; max = 0.0f; mean = 0.0f;

        //    //start_date=2017-01-01&end_date=2017-07-10

        //    string timeline = string.Empty;
        //    if ( sd != "" && ed != "" )
        //        timeline = "start_date=" + sd + "& end_date=" + ed + "&"; // enddate > startdate

        //    string api_fetch_string = quandl_path + quandl_exch_nse + @"/" + ticker + quandl_ext + timeline + quanl_api_key;

        //    id = string.Empty;
        //    dataset_code = string.Empty;
        //    database_code = string.Empty;
                                               
        //    name = string.Empty;
        //    description = string.Empty;
        //    isin = string.Empty;
        //    start_date = string.Empty;
        //    end_date = string.Empty;

        //    try
        //    {
                

        //        using (WebClient wc = new WebClient())
        //        {
        //            string jSonStr = wc.DownloadString(api_fetch_string);

        //            string[] strarray = jSonStr.Split(new[] { "\"data\"" }, StringSplitOptions.None);

        //            string[] header_entities = getCleanHeaderString(strarray[0]);


        //            id = parse_header("id", header_entities).Replace(string.Format("\""), "");
        //            dataset_code = parse_header("dataset_code", header_entities).Replace(string.Format("\""), "");
        //            database_code = parse_header("database_code", header_entities).Replace(string.Format("\""), "");
        //            name = parse_header("name", header_entities).Replace(string.Format("\""), "");
        //            description = parse_header("description", header_entities).Replace(string.Format("\""), "");
        //            isin = parse_header("ISIN", header_entities).Replace(string.Format("\""), "");
        //            start_date = parse_header("start_date", header_entities).Replace(string.Format("\""), "");
        //            end_date = parse_header("end_date", header_entities).Replace(string.Format("\""), "");



        //            string to_chop1 = ":[[";
        //            strarray[1] = strarray[1].Remove(0, to_chop1.Length);
        //            string[] data = strarray[1].Split(new[] { "],[" }, StringSplitOptions.None);
        //            //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3
        //            float closing_total = 0.0f;


        //            foreach (string str in data)
        //            {
        //                QHistoryDatum hd = new QHistoryDatum();
        //                string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
        //                hd.Date = entity[0].Replace(string.Format("\""), "");
        //                hd.Open = entity[1];
        //                hd.High = entity[2];
        //                hd.Low = entity[3];
        //                hd.Last = entity[4];
        //                hd.Close = entity[5];
        //                hd.Total_Trade_Quantity = entity[6];
        //                hd.Turn_Over = entity[7];
        //                if(min==0.0f)
        //                    min = float.Parse(hd.Low);
        //                if(max==0.0f)
        //                    max = float.Parse(hd.High);

        //                if (min > float.Parse(hd.Low))
        //                    min = float.Parse(hd.Low);

        //                if (max < float.Parse(hd.High))
        //                    max = float.Parse(hd.High);

        //               // Console.WriteLine("Low :" + hd.Low + ", High :" + hd.High +", Last :" + hd.Last  );

        //                closing_total += float.Parse(hd.Close);

        //                History_list.Add(hd);
        //            }
        //            mean = closing_total / (float)(History_list.Count);

        //        }

        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
        //        {
        //            var resp = (HttpWebResponse)ex.Response;
        //            if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
        //            {
        //                //Handle it
        //                Console.WriteLine("JSonParser : End resp.StatusCode ==>" + api_fetch_string);
        //            }
        //        }
        //        //Handle it
        //        return null;
        //    }

        //    return History_list;
        //}

        //public static List<QHistoryDatum> get_BSE_TickerObjectArray(
        //                                           string ticker,
        //                                           string sd,
        //                                           string ed,
        //                                          out string id,
        //                                          out string dataset_code,
        //                                          out string database_code,
        //                                          out string name,
        //                                          out string description,
        //                                          out string isin,
        //                                          out string start_date,
        //                                          out string end_date
        //                                          )
        //{
        //    string timeline = string.Empty;
        //    if (sd != "" && ed != "")
        //        timeline = "start_date =" + sd + "& end_date = " + ed + "&"; // enddate > startdate

        //    string api_fetch_string = quandl_path + quandl_exch_nse + @"/" + ticker + quandl_ext + timeline + quanl_api_key;


        //    id = string.Empty;
        //    dataset_code = string.Empty;
        //    database_code = string.Empty;

        //    name = string.Empty;
        //    description = string.Empty;
        //    isin = string.Empty;
        //    start_date = string.Empty;
        //    end_date = string.Empty;

        //    try
        //    {


        //        using (WebClient wc = new WebClient())
        //        {
        //            string jSonStr = wc.DownloadString(api_fetch_string);

        //            string[] strarray = jSonStr.Split(new[] { "\"data\"" }, StringSplitOptions.None);

        //            string[] header_entities = getCleanHeaderString(strarray[0]);


        //            id = parse_header("id", header_entities).Replace(string.Format("\""), "");
        //            dataset_code = parse_header("dataset_code", header_entities).Replace(string.Format("\""), "");
        //            database_code = parse_header("database_code", header_entities).Replace(string.Format("\""), "");
        //            name = parse_header("name", header_entities).Replace(string.Format("\""), "");
        //            description = parse_header("description", header_entities).Replace(string.Format("\""), "");
        //            isin = parse_header("ISIN", header_entities).Replace(string.Format("\""), "");
        //            start_date = parse_header("start_date", header_entities).Replace(string.Format("\""), "");
        //            end_date = parse_header("end_date", header_entities).Replace(string.Format("\""), "");



        //            string to_chop1 = ":[[";
        //            strarray[1] = strarray[1].Remove(0, to_chop1.Length);
        //            string[] data = strarray[1].Split(new[] { "],[" }, StringSplitOptions.None);
        //            //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3

        //            foreach (string str in data)
        //            {
        //                QHistoryDatum hd = new QHistoryDatum();
        //                string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
        //                hd.Date = entity[0].Replace(string.Format("\""), "");
        //                hd.Open = entity[1];
        //                hd.High = entity[2];
        //                hd.Low = entity[3];
        //                hd.Last = entity[4];
        //                hd.Close = entity[5];
        //                hd.Total_Trade_Quantity = entity[6];
        //                hd.Turn_Over = entity[7];

        //                History_list.Add(hd);
        //            }

        //        }

        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
        //        {
        //            var resp = (HttpWebResponse)ex.Response;
        //            if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
        //            {
        //                //Handle it
        //                Console.WriteLine("End resp.StatusCode ==>" + api_fetch_string);
        //            }
        //        }
        //        //Handle it
        //        return null;
        //    }

        //    return History_list;
        //}

    }
}
