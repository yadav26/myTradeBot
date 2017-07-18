using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Entity.BusinessModel
{
    public class TradingBusinessModel
    {
        public string GoogleSecurityID { get; set; }
        public string TickerKey { get; set; }
        public string Exchange { get; set; }
        public Decimal LastPrice { get; set; }
        public Decimal Price { get; set; } // Price
        public Decimal L_Cur { get; set; }
        public int S { get; set; }
        public DateTime LastTradeTime { get; set; }
        public DateTime LastTrdetimeformated { get; set; }
        public DateTime LastTradeDateTime { get; set; }
        public Decimal Change { get; set; }
        public Decimal C_fix { get; set; } // Absolute price change
        public Decimal ChangePercentage { get; set; }
        public Decimal Cp_fix { get; set; }
        public string Ccol { get; set; }
        public Decimal Pcls_Fix { get; set; }
        public Decimal Afterhourslastprice { get; set; }
        public DateTime Afterhourslasttradetimeformated { get; set; }
        public DateTime GetActualtime { get; set; }
    }
}
