using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    public static class StringTypeParser
    {

        //https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=600   
        //https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=60&p=5d&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218
        //https://www.google.com/finance/getprices?q="+ <ticker> + "&x=" + <ExChange> + "&i="+<interval>+"&p="+ <5d> +"&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218"


        public static string gfinance_url_path = @"https://www.google.com/finance/getprices?q=";
        private static string gExc_nse = @"NSE";
        //private static string quandl_exch_bse = @"BSE";

        private static string sep = "&i=" ;
        private static string periods = "&p=";
        private static string gfinance_api_key = "&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218";

        public static List<GHistoryDatum> History_list = new List<GHistoryDatum>();


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

        public static SortedDictionary<int, float > get_TickerClosePriceMap(
                                                            string exchange,
                                                            string ticker,
                                                            string sd,
                                                            string ed,
                                                            int interval,
                                                            int num_of_day_data

                                                        )
        {

            //https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=86400&p=90d&f=d,c
            string intstr = string.Format("{0}", interval);
            string gfinance_api_key1 = "&f=d,c";

            string api_fetch_string1 = gfinance_url_path + ticker.Trim() + "&x=" + exchange + sep + intstr + periods + String.Format("{0}d",num_of_day_data) + gfinance_api_key1;


            try
            {

                Map_ClosePrice.Clear();
                using (WebClient wc = new WebClient())
                {
                    string jWebString = wc.DownloadString(api_fetch_string1);


                    string[] strarray = jWebString.Split(new[] { "\n1," }, StringSplitOptions.None);
                    if (strarray.Length < 2)
                    {
                        Console.WriteLine("\nGHistory thread data fetch failed : url{" + api_fetch_string1 + "}\n");
                        return null;
                    }
                    string parsethis = "1," + strarray[1];
                    string[] data =  parsethis.Split(new[] { "\n" }, StringSplitOptions.None);
                    //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3
                    float closing_total = 0.0f;

                    data = data.Where(w => w != data[data.Length - 1]).ToArray(); // deleting last
                    //data = data.Where(w => w != data[0]).ToArray(); // deleting first


                    foreach (string str in data)
                    {

                        string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);



                        Map_ClosePrice.Add(int.Parse(entity[0]), float.Parse(entity[1]) );
                    }

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
                        Console.WriteLine("End resp.StatusCode ==>" + api_fetch_string1);
                    }
                }
                //Handle it
                return null;
            }

            return Map_ClosePrice;
        }


        public static List<GHistoryDatum> get_TickerObjectArray(
                                                                    string exchange,
                                                                    string ticker,
                                                                    string sd,
                                                                    string ed,
                                                                    int interval,
                                                                    string num_of_day_data,
                                                                    out float min,
                                                                    out float max,
                                                                    out float mean,
                                                                    out float median

                                                                )
        {
            min = 0.0f; max = 0.0f; mean = 0.0f; median = 0.0f;

            //start_date=2017-01-01&end_date=2017-07-10
            string intstr = string.Format("{0}", interval);

            string api_fetch_string = gfinance_url_path + ticker.Trim() + "&x=" + exchange + sep + intstr + periods + interval + gfinance_api_key;


            try
            {


                using (WebClient wc = new WebClient())
                {
                    string jWebString = wc.DownloadString(api_fetch_string);


                    string[] strarray = jWebString.Split(new[] { "TIMEZONE_OFFSET" }, StringSplitOptions.None);
                    if(strarray.Length < 2 )
                    {
                        Console.WriteLine("\nGHistory thread data fetch failed : url{"+ api_fetch_string+"}\n");
                        return null;
                    }
                    string[] data = strarray[1].Split(new[] { "\n" }, StringSplitOptions.None);
                    //:[["2017-07-13",288.9,290.0,286.55,288.35,288.75,8434324.0,24329.3
                    float closing_total = 0.0f;

                    data = data.Where(w => w != data[data.Length - 1]).ToArray(); // deleting last
                    data = data.Where(w => w != data[0]).ToArray(); // deleting first
                    

                    float[] arr_low = new float[data.Length];
                    int counter = 0;
                    foreach (string str in data)
                    {
                        
                        GHistoryDatum hd = new GHistoryDatum();
                        string[] entity = str.Split(new[] { "," }, StringSplitOptions.None);
                        hd.Date = entity[0].Replace(string.Format("\""), "");
                        int checkValue = 0;
                        if (false == int.TryParse(hd.Date, out checkValue))
                            continue;
                        hd.Close = entity[1];
                        hd.High = entity[2];
                        hd.Low = entity[3];
                        hd.Open = entity[4];
                        hd.TickerSymbol = ticker;
                        arr_low[ counter ++] =  float.Parse(hd.Low);

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

                    //arr_low = Array.Sort(arr_low, (a, b) => a.CompareTo(b));
                    //arr_low = arr_low.OrderBy(x => x);

                    arr_low = RadixSort(arr_low);
                    int rem = counter / 2;
                    if (counter % 2 != 0)
                        median = arr_low[rem + 1];
                    else
                        median = mean;

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

        public static void Flush_HistoryList()
        {
            History_list.Clear();
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

            string api_fetch_string = gfinance_url_path + ticker + "&x="+ gExc_nse + sep + intstr + gfinance_api_key;


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


            string api_fetch_string = gfinance_url_path + ticker + "&x=" + gExc_nse + gfinance_api_key;


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


    }
}
