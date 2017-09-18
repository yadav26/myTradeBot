using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Trading.Entity;
using Trading.Model;

namespace Google
{
    public class JsonGoogleInfoObject
    {
        public string ticker { get; set; }
        public string exchange { get; set; }
        public string ltt { get; set; }
        public string lt { get; set; }
        public string lt_dts { get; set; }
        public string ccol { get; set; }
        

        public double id { get; set; }
        public float latestprice { get; set; }
        public float l_fix { get; set; }
        public float l_cur { get; set; }
        public float change { get; set; }
        public float cp { get; set; }
        public float cp_fix { get; set; }
        public float pcls_fix { get; set; }


        // how about adding details of VWMA, EMA, WMA, THIGH, TLOW
        public float TVWMA { get; set; }
        public float TEMA { get; set; }
        public float TWMA { get; set; }
        public float THIGH { get; set; }
        public float TLOW { get; set; }


        private string string_to_parse = string.Empty;
        public JsonGoogleInfoObject(string input)
        {
            string_to_parse = input;
        }
    }

    public class TranslateJsonToObject
    {
        /*
        // [ 
        { "id": "6864582" ,"t" : "JPASSOCIAT" ,"e" : "NSE" ,"l" : "26.95" ,"l_fix" : "26.95" ,"l_cur" : "₹26.95" ,"s": "0" ,
        "ltt":"10:46AM GMT+5:30" ,"lt" : "Aug 8, 10:46AM GMT+5:30" ,"lt_dts" : "2017-08-08T10:46:58Z" ,"c" : "-2.00" ,
        "c_fix" : "-2.00" ,"cp" : "-6.91" ,"cp_fix" : "-6.91" ,"ccol" : "chr" ,"pcls_fix" : "28.95" } ,
        
        { "id": "994804355412903" ,"t" : "ICICIPRULI" ,"e" : "NSE" ,"l" : "428.85" ,"l_fix" : "428.85" ,"l_cur" : "₹428.85" ,"s": "0" ,
        "ltt":"10:46AM GMT+5:30" ,"lt" : "Aug 8, 10:46AM GMT+5:30" ,"lt_dts" : "2017-08-08T10:46:57Z" ,"c" : "-8.55" ,"c_fix" : "-8.55" ,
        "cp" : "-1.95" ,"cp_fix" : "-1.95" ,"ccol" : "chr" ,"pcls_fix" : "437.4" } 
        ]
        */
        public string ParseJsonForValue( string search, string strInput )
        {
            string retVal = string.Empty;

            string[] data = strInput.Split(new[] { string.Format(("\"{0}\""), search) }, StringSplitOptions.None);

           // string[] value = data[1].Split(new[] { "," }, StringSplitOptions.None);

            string modInput = Regex.Replace(strInput, @"\t|\n|\r|//|\[|\]|\ ", "").Trim();

            string[] strarray = modInput.Split(new[] { "\",\"" }, StringSplitOptions.None);

            //string[] strarray = jSonStr.Split(',');
            //int x = 0;

            string quot_val = string.Format(("{0}"), search);
            var results = Array.FindAll(strarray, s => s.Contains(quot_val));
            //"l":"106.30"
            string price = Regex.Replace(results[0], @"\,|\""|&#8377;|}", ""); // INR symbol hex code &#8377;

            string price_st = price.Substring(price.IndexOf(":") + 1, price.Length - price.IndexOf(":") - 1);

            //return banker_ceil(float.Parse(price_st));
            return price_st;
        }

        public Dictionary<string, UpdateScannerGridObject> GetMapOfTickerCurrentPrice(string exchange, // more than one seperated by comma 
                                                    List<string> InputList)
        {
            Dictionary<string, UpdateScannerGridObject> map = new Dictionary<string, UpdateScannerGridObject>();
            int chunkSize = 99;

            List<string> InputList_noDuplicates = InputList.Distinct().ToList();

            //REmove duplicate entries
            List <List<string> >TempList = InputList_noDuplicates.Select((x, i) => new { Index = i, Value = x })
                                        .GroupBy(x => x.Index / chunkSize)
                                        .Select(x => x.Select(v => v.Value).ToList())
                                        .ToList();
            Dictionary<string, UpdateScannerGridObject> tmp = new Dictionary<string, UpdateScannerGridObject>();

            foreach (var choppedList in TempList)
            {
                //tmp.Clear();
                tmp = GetGInfoMapOfTickerCurrentPrice(exchange, choppedList);
                if (tmp == null)
                    continue;

                map = map.Concat(tmp).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
            }
            
            return map;

        }



        public Dictionary<string, UpdateScannerGridObject> GetGInfoMapOfTickerCurrentPrice(string exchange, // more than one seperated by comma 
                                                    List<string> InputList)
        {
            Dictionary<string, UpdateScannerGridObject> map = new Dictionary<string, UpdateScannerGridObject>();

            //
            //https://www.google.com/finance/info?q=NSE:Jpassociat,ICICIPRULI
            //

            string local_api_info_string = @"https://www.google.com/finance/info?q=" + exchange + ":";
            string ticker_format_string = string.Empty;

            //foreach (KeyValuePair<string, UpdateScannerGridObject> kvp in mapInput.ToArray())
            List<string> InputListNoDup = InputList.Distinct().ToList();

            foreach ( string strTicker in InputListNoDup)
            {
                string ticker = strTicker;// kvp.Key.Trim();
                string cleanedTicker = ticker;
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

                //ticker = cleanedTicker;

                ticker_format_string += ticker + ",";// intstr + periods + String.Format("{0}d", num_of_day_data) + gfinance_api_key1;

            }

            local_api_info_string += ticker_format_string;

            try
            {

                //Map_ClosePrice.Clear();
                using (WebClient wc = new WebClient())
                {
                    //https://www.google.com/finance/info?q=NSE:ADANIENT,ADANIPORTS,ADANIPOWER,AJANTPHARM,ALBK,AMARAJABAT,AMBUJACEM
                    //https://www.google.com/finance/info?q=NSE:ADANIENT,ADANIPORTS,ADANIPOWER,AJANTPHARM,ALBK,AMARAJABAT,AMBUJACEM
                    string jWebString = wc.DownloadString(local_api_info_string);

                    List<UpdateScannerGridObject> list_object = GetGInfoObject(jWebString);

                    foreach (UpdateScannerGridObject obj in list_object )
                    {
                        map.Add(obj.Ticker, obj);
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
                        Console.WriteLine("End resp.StatusCode ==>api fetch failed.");
                    }
                }
                //Handle it
                return null;
            }

            return map;
       }





        public List<UpdateScannerGridObject> GetGInfoObject( string strInput )
        {
            List<UpdateScannerGridObject> myList = new List<UpdateScannerGridObject>();

            string[] data = strInput.Split(new[] { ",{" }, StringSplitOptions.None);

            foreach( string val in data )
            {
                UpdateScannerGridObject obj = new UpdateScannerGridObject();


                obj.id = double.Parse(ParseJsonForValue("id", val ) );
                float c = 0.0f;
                bool bResult = float.TryParse(ParseJsonForValue("c\"", val), out c);
                float cp = 0.0f, cp_fix, l, l_cur, l_fix, pcls_fix;

                float retVal = 0.0f;

                obj.ccol = ParseJsonForValue("ccol", val);
                obj.Exchange = ParseJsonForValue("e", val);
                string tTemp = ParseJsonForValue("t", val);

                obj.Ticker = tTemp;
                if (tTemp.Contains("\\u0026"))
                    obj.Ticker = tTemp.Replace("\\u0026", "&");

                bResult = float.TryParse(ParseJsonForValue("lt", val), out retVal);
                obj.lt = retVal;

                bResult = float.TryParse(ParseJsonForValue("ltt", val), out retVal);
                obj.ltt = retVal;

                bResult = float.TryParse(ParseJsonForValue("lt_dts", val), out retVal);
                obj.lt_dts = retVal;

                bResult = float.TryParse(ParseJsonForValue("cp\"", val), out cp);
                bResult = float.TryParse(ParseJsonForValue("cp_fix", val), out cp_fix);
                bResult = float.TryParse(ParseJsonForValue("l", val), out l);
                bResult = float.TryParse(ParseJsonForValue("l_cur", val), out l_cur);
                bResult = float.TryParse(ParseJsonForValue("l_fix", val), out l_fix);
                bResult = float.TryParse(ParseJsonForValue("pcls_fix", val), out pcls_fix);


                obj.change = c;
                obj.cp =cp;
                obj.cp_fix = cp_fix;
                obj.CurrentPrice = l;
                obj.l_cur = l_cur;
                obj.l_fix = l_fix;
                obj.pcls_fix = pcls_fix;

                myList.Add(obj);
            }
            

            return myList;
        }


        public List<UpdateScannerGridObject> GetPriceObject(string strInput) // GetPrice api using
        {
            List<UpdateScannerGridObject> myList = new List<UpdateScannerGridObject>();

            string[] data = strInput.Split(new[] { ",{" }, StringSplitOptions.None);
            foreach (string val in data)
            {
                UpdateScannerGridObject obj = new UpdateScannerGridObject( );
                obj.Exchange = ParseJsonForValue("e", val);
                obj.Ticker = ParseJsonForValue("t", val);

                Daily_Reader dailyReader = new Daily_Reader();
                dailyReader.parser(obj.Exchange, obj.Ticker, 60, 1);
                List<StringParsedData> list = dailyReader.GetGHistoryList();

                
                //foreach(List)
                myList.Add(obj);
            }


            return myList;
        }



    }
}
