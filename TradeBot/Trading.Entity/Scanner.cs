using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Entity
{
    public class Scanner
    {
        public int RowIndex { get; set; }
        public string Ticker { get; set; }
        public bool IsNRDay { get; set; }
        public float WMA { get; set; }
        public float EMA { get; set; }
        public float SMA { get; set; }
        public float Close { get; set; }
        public double Volume { get; set; }
        public float Current_Price { get; set; }
        public bool Forced_purchase { get; set; }

        public Scanner(int rowIndex, string t, bool nr, float wma, float ema, float sma, float close, double vol, float cp, bool fp )
        {
            RowIndex = rowIndex;
            Ticker = t;
            IsNRDay = nr;
            EMA = ema;
            SMA = sma;
            Close = close;
            Volume = vol;
            Current_Price = cp;
            Forced_purchase = fp;
            WMA = wma;
        }

    }
}
