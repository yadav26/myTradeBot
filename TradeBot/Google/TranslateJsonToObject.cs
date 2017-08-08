using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public float latest_price { get; set; }
        public float l_fix { get; set; }
        public float l_cur { get; set; }
        public float change { get; set; }
        public float cp { get; set; }
        public float cp_fix { get; set; }
        public float pcls_fix { get; set; }

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

        public List<JsonGoogleInfoObject> GetGInfoObject( string strInput )
        {
            List<JsonGoogleInfoObject> myList = new List<JsonGoogleInfoObject>();

            string[] data = strInput.Split(new[] { ",{" }, StringSplitOptions.None);
            foreach( string val in data )
            {
                JsonGoogleInfoObject obj = new JsonGoogleInfoObject(val);
                
                
                obj.ccol = ParseJsonForValue("ccol", val );
                obj.lt = ParseJsonForValue("lt", val );
                obj.ltt = ParseJsonForValue("ltt", val );
                obj.exchange = ParseJsonForValue("e", val );
                obj.lt_dts = ParseJsonForValue("lt_dts", val );
                obj.ticker = ParseJsonForValue("t", val);

                obj.id = double.Parse(ParseJsonForValue("id", val ) );
                obj.change = float.Parse(ParseJsonForValue("c\"", val ));
                obj.cp = float.Parse(ParseJsonForValue("cp\"", val ));
                obj.cp_fix = float.Parse(ParseJsonForValue("cp_fix", val));
                obj.latest_price = float.Parse(ParseJsonForValue("l", val));
                obj.l_cur = float.Parse(ParseJsonForValue("l_cur", val));
                obj.l_fix = float.Parse(ParseJsonForValue("l_fix", val));
                obj.pcls_fix = float.Parse(ParseJsonForValue("pcls_fix", val));

                myList.Add(obj);
            }
            

            return myList;
        }


    }
}
