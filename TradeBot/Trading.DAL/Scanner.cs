using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.DAL
{
    public class Scanner
    {
        public string Ticker { get; set; }
        public bool IsNRDay { get; set; }
        public float EMA { get; set; }
        public float SMA { get; set; }
        public float Close { get; set; }
        public double Volume { get; set; }
        public float Current_Price { get; set; }
        public bool Forced_purchase { get; set; }

    }
}
