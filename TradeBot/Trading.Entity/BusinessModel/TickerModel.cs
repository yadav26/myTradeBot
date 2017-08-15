using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Model.BusinessModel
{
    public class TickerModel
    {
        public string TickerName { get; set; }
        public string TickerSymbol { get; set; }
        public string ISIN { get; set; }
        public decimal MarketCapital { get; set; }
        public decimal PerRatio { get; set; }
        public decimal DivYield { get; set; }
        public string Status { get; set; }
        public decimal VWAP { get; set; }
        public int TickerFaceValue { get; set; }
    }
}
