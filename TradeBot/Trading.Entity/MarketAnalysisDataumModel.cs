using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Model
{
    public class MarketAnalysisDataumModel
    {
        public string Ticker { get; set; }
        public double Volume { get; set; }
        public bool IsNRDay { get; set; }
        public float WMA { get; set; }
        public float EMA { get; set; }
        public float Current { get; set; }
        public float SMA { get; set; }
        public float Close { get; set; }
        double Trading_vol_Min { get; set; }
        int DateDay { get; set; }
        public string Exchange { get; set; }
        public void SetExchange(string s) { Exchange = s; }

    }
}
