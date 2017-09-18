using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    public class MovingAverageData
    {
        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public float LastClose { get; set; }
        public float TodaySMA { get; set; }
        public float TodayEMA { get; set; }
        public int DateDay { get; set; }


    }

    public class StringParsedData
    {//COLUMNS=d:DATE,c:CLOSE,h:HIGH,l:LOW,o:OPEN,v:VOLUME,k:CDAYS
        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public int DateDay { get; set; }
        public float Close { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
        public double Volume { get; set; }
        public float Cdays { get; set; }

        public DateTime dateTime { get; set; }
    }

    public static class StringTypeParser
    {

        
        //https://finance.google.com/finance/getprices?q=GMRINFRA&p=1d&f=d,c,o,h,l,v,k


        public const int ALLOWED_INTERVAL = 5;//seconds.
        //public static string gfinance_url_path = @"https://www.google.com/finance/getprices?q=";
        public static string gfinance_url_path = @"https://finance.google.com/finance/getprices?q=";
        private static string gExc_nse = @"NSE";
        //private static string quandl_exch_bse = @"BSE";

        private static string sep = "&i=" ;
        private static string periods = "&p=";
        private static string gfinance_api_key = "&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218";

        public static List<StringParsedData> History_list = new List<StringParsedData>();


        public static float[] RadixSort(this float[] array)
        {
            // temporary array and the array of converted floats to ints
            int[] t = new int[array.Length];
            int[] a = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
                a[i] = BitConverter.ToInt32(BitConverter.GetBytes(array[i]), 0);

            // set the group length to 1, 2, 4, 8 or 16
            // and see which one is quicker
            int groupLength = 4;
            int bitLength = 32;

            // counting and prefix arrays
            // (dimension is 2^r, the number of possible values of a r-bit number) 
            int[] count = new int[1 << groupLength];
            int[] pref = new int[1 << groupLength];
            int groups = bitLength / groupLength;
            int mask = (1 << groupLength) - 1;
            int negatives = 0, positives = 0;

            for (int c = 0, shift = 0; c < groups; c++, shift += groupLength)
            {
                // reset count array 
                for (int j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                for (int i = 0; i < a.Length; i++)
                {
                    count[(a[i] >> shift) & mask]++;

                    // additionally count all negative 
                    // values in first round
                    if (c == 0 && a[i] < 0)
                        negatives++;
                }
                if (c == 0) positives = a.Length - negatives;

                // calculating prefixes
                pref[0] = 0;
                for (int i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                for (int i = 0; i < a.Length; i++)
                {
                    // Get the right index to sort the number in
                    int index = pref[(a[i] >> shift) & mask]++;

                    if (c == groups - 1)
                    {
                        // We're in the last (most significant) group, if the
                        // number is negative, order them inversely in front
                        // of the array, pushing positive ones back.
                        if (a[i] < 0)
                            index = positives - (index - negatives) - 1;
                        else
                            index += negatives;
                    }
                    t[index] = a[i];
                }

                // a[]=t[] and start again until the last group 
                t.CopyTo(a, 0);
            }

            // Convert back the ints to the float array
            float[] ret = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
                ret[i] = BitConverter.ToSingle(BitConverter.GetBytes(a[i]), 0);

            return ret;
        }

        static SortedDictionary<int, float> Map_ClosePrice = new SortedDictionary< int, float >();

        static List<StringParsedData> List_StringParseData = new List<StringParsedData>();

        public static SortedDictionary<int, StringParsedData> Get_gAPI_MapData(
                                                                        string e,
                                                                        string t,
                                                                        int i,
                                                                        int n
                                                                     )
        {
            SortedDictionary<int, StringParsedData> map = new SortedDictionary<int, StringParsedData>();

            List<StringParsedData> collection_data = Get_gAPI_ListData(e, t, i, n);

            foreach(StringParsedData obj in collection_data )
            {
                map.Add(obj.DateDay, obj);
            }

            return map;
        }



        //public static Dictionary<string, float> Get_gAPI_MapLatestPrice(
        //                                            string exchange, // more than one seperated by comma 
        //                                            Dictionary<string, float> mapInput

        //                                           )
        //{
        //    Dictionary<string, float> map = new Dictionary<string, float>();

        //    //
        //    //https://www.google.com/finance/info?q=NSE:Jpassociat,ICICIPRULI
        //    //

        //    string api_fetch_string1 = @"https://www.google.com/finance/info?q=" + exchange +":"; 
        //    string ticker_format_string = string.Empty;

        //    foreach (KeyValuePair<string, float> kvp in mapInput.ToArray())
        //    {
        //        string ticker = kvp.Key.Trim();
        //        string cleanedTicker = string.Empty;
        //        switch (ticker)
        //        {
        //            case "M&M":
        //                cleanedTicker = "M%26M";
        //                break;
        //            case "L&TFH":
        //                cleanedTicker = "L%26TFH";
        //                break;
        //            case "M&MFIN":
        //                cleanedTicker = "M%26MFIN";
        //                break;
        //            default:
        //                break;
        //        }

        //        cleanedTicker = ticker;

        //        ticker_format_string += cleanedTicker + ",";// intstr + periods + String.Format("{0}d", num_of_day_data) + gfinance_api_key1;

        //    }

        //    api_fetch_string1 += ticker_format_string;

        //    try
        //    {

        //        //Map_ClosePrice.Clear();
        //        using (WebClient wc = new WebClient())
        //        {


        //            string jWebString = wc.DownloadString(api_fetch_string1);
        //            jWebString = jWebString.Replace("\n", string.Empty);
        //            jWebString = jWebString.Replace(" ", string.Empty);
        //            jWebString = jWebString.Replace(",\"", ";\"");

        //            string[] data = jWebString.Split(new[] { ";" }, StringSplitOptions.None);
        //            //// [ { "id": "13564339" ,"t" : "SBIN" ,"e" : "NSE" ,"l" : "310.60" ,"l_fix" : "310.60" ,
        //            //"l_cur" : "₹310.60" ,"s": "0" ,"ltt":"3:51PM GMT+5:30" ,"lt" : "Aug 7, 3:51PM GMT+5:30" ,
        //            //"lt_dts" : "2017-08-07T15:51:09Z" ,"c" : "+5.35" ,"c_fix" : "5.35" ,"cp" : "1.75" ,"cp_fix" : "1.75" ,
        //            //"ccol" : "chg" ,"pcls_fix" : "305.25" } ]


        //            //data = data.Where(w => w != data[data.Length - 1]).ToArray(); // deleting last
        //            //data = data.Where(w => w != data[0]).ToArray(); // deleting first

        //            //List_StringParseData.Clear();


        //            string tickerName = string.Empty;
        //            float tickerPrice = 0.0f;

        //            foreach (string str in data)
        //            {

        //                string[] entity = str.Split(new[] { ":" }, StringSplitOptions.None);
        //                string strKey = entity[0].Replace(" ", string.Empty);
        //                strKey = strKey.Replace("\"", "");
        //                if (strKey == "t")
        //                    tickerName = entity[1].Replace("\"", string.Empty);

        //                if (strKey == "l")
        //                {
        //                    string strcp = entity[1].Replace("\"", string.Empty);
        //                    strcp = strcp.Replace(",", string.Empty);
        //                    tickerPrice = float.Parse(strcp);

        //                    map.Add(tickerName, tickerPrice);
        //                    tickerName = string.Empty;
        //                    tickerPrice = 0.0f;
        //                }

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
        //                Console.WriteLine("End resp.StatusCode ==>" + api_fetch_string1);
        //            }
        //        }
        //        //Handle it
        //        return null;
        //    }

        //    return map;
        //}


        public static double g_gApiCounterFetch;

        public static List<StringParsedData> Get_gAPI_ListData(
                                                    string exchange,
                                                    string ticker,
                                                    int interval,
                                                    int num_of_day_data

                                                )
        {
            //
            //https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=86400&p=90d&f=d,c
            //

            if (interval < ALLOWED_INTERVAL)
                return null;

            string intstr = string.Format("{0}", interval);
            string gfinance_api_key1 = "&f=d,c,o,h,l,v,k";


            switch (ticker)
            {
                case "M&M":
                    ticker = "M%26M";
                    break;
                case "L&TFH":
                    ticker = "L%26TFH";
                    break;
                case "M&MFIN":
                    ticker = "M%26MFIN";
                    break;
                default:
                    break;
            }
            string api_fetch_string1 = gfinance_url_path + ticker.Trim() + "&x=" + exchange + sep + intstr + periods + String.Format("{0}d", num_of_day_data) + gfinance_api_key1;

            
            try
            {

                //Map_ClosePrice.Clear();
                using (WebClient wc = new WebClient())
                {
                    //"https://finance.google.com/finance/getprices?q=ACC&x=NSE&i=10&p=1d&f=d,c,o,h,l,v,k"
                    string jWebString = wc.DownloadString(api_fetch_string1);

                    //COLUMNS = DATE,CLOSE,HIGH,LOW,OPEN,VOLUME,CDAYS
                    //DATA =
                    //TIMEZONE_OFFSET = 330
                    //a1502941800,280.75,283.6,280.65,283.6,511793,0

                    g_gApiCounterFetch++;

                    string[] strarray = jWebString.Split(new[] { "\na1" }, StringSplitOptions.None);
                    if (strarray.Length < 2)
                    {
                        Console.WriteLine("\nGHistory thread data fetch failed : url{" + api_fetch_string1 + "}\n");
                        return null;
                    }

                    string parsethis = string.Empty;
                    strarray = strarray.Where(w => w != strarray[0]).ToArray();
                    foreach ( string strPart in strarray)
                    {
                        parsethis = parsethis + "\na1" + strPart;
                    }
                    
                    string[] data = parsethis.Split(new[] { "\n" }, StringSplitOptions.None);
//

                    data = data.Where(w => w != data[data.Length - 1]).ToArray(); // deleting last
                    //data = data.Where(w => w != data[0]).ToArray(); // deleting first

                    List_StringParseData.Clear();

                    foreach (string str in data)
                    {

                        string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
                        StringParsedData sparseData = new StringParsedData();
                        int val = -1;
                        bool bRet = int.TryParse(entity[0], out val );
                        sparseData.DateDay = val;
                        sparseData.Close = float.Parse(entity[1]);
                        sparseData.High = float.Parse(entity[2]);
                        sparseData.Low = float.Parse(entity[3]);
                        sparseData.Open = float.Parse(entity[4]);
                        sparseData.Volume = float.Parse(entity[5]);
                        sparseData.Cdays = float.Parse(entity[6]);


                        sparseData.Exchange = exchange;
                        sparseData.Ticker = ticker;

                        List_StringParseData.Add(sparseData);

                    }

                }

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.ServiceUnavailable) // HTTP 404
                    {
                        //Handle it
                        Console.WriteLine("End resp.StatusCode ==>" + api_fetch_string1);
                    }
                }
                //Handle it
                return null;
            }

            return List_StringParseData;
        }




        

    }
}
