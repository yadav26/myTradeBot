using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Entity
{
    public class APIResultModel
    {
        public string ID { get; set; }  //
        public string T { get; set; }   // Ticker
        public string E { get; set; }   // Exchange
        public string L { get; set; }   // LastPrice
        public string L_Fix { get; set; } //
        public string L_Cur { get; set; }
        public string S { get; set; }
        public string Ltt { get; set; } // LastTradeTime
        public string Lt { get; set; }  // Lattrdetimeformated
        public string Lt_Dts { get; set; } //LastTradeDateTime
        public string C { get; set; } // Change
        public string C_fix { get; set; } //
        public string Cp { get; set; } // Change Percentage
        public string Cp_fix { get; set; }
        public string Ccol { get; set; }
        public string Pcls_Fix { get; set; }

        public string el { get; set; } // Afterhourslast price
        public string elt { get; set; }


        string getactualtime;

        public string GetActualtime
        {
            get { return getactualtime; }
            set { getactualtime = DateTime.Now.ToLongTimeString(); }
        }
    }
}
//{
//"id": "358464", Google security ID
//,"t" : "MSFT" Ticker
//,"e" : "NASDAQ" Exchange
//,"l" : "69.08" LastPrice
//,"l_fix" : "69.08"  Price
//,"l_cur" : "69.08"
//,"s": "2"
//,"ltt":"4:00pm GMT-4" LastTradeTime
//,"lt" : "Jul 5, 4:00pm GMT-4" Lasttrdetimeformated
//,"lt_dts" : "2017-07-05T16:00:02Z" LastTradeDateTime
//,"c" : "+0.91" Change
//,"c_fix" : "0.91"
//,"cp" : "1.33" Change Percentage
//,"cp_fix" : "1.33" Absolute price change
//,"ccol" : "chg"
//,"pcls_fix" : "68.17"
//,"el": "69.08"   Afterhourslast price
//,"el_fix": "69.08"
//,"el_cur": "69.08"
//,"elt" : "Jul 5, 7:54pm GMT-4" Afterhourslasttradetimeformated
//,"ec" : "0.00"  Extenthourchange 
//,"ec_fix" : "0.00"
//,"ecp" : "0.00" Extenthourchange percentage
//,"ecp_fix" : "0.00"
//,"eccol" : "chb"
//,"div" : "0.39" Divedent
//,"yld" : "2.26" Divedentyeild
//}
