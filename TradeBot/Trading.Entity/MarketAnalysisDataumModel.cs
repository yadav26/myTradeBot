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
        public float VWMA { get; set; }
        public float WMA { get; set; }
        public float EMA { get; set; }
        public float Current { get; set; }
        public float SMA { get; set; }
        public float Close { get; set; }
        double Trading_vol_Min { get; set; }
        int DateDay { get; set; }
        public string Exchange { get; set; }
        public void SetExchange(string s) { Exchange = s; }


        //ay
        public double HighVolume90 { get; set; }
        public double LowVolume90 { get; set; }
        public float HighPrice90 { get; set; }
        public float LowPrice90 { get; set; }
    }
}
