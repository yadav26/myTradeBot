using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Model
{
    public class MarketAnalysisDataumModel
    {
        public string Ticker { get; set; }
        public double Volume { get; set; }
        [Browsable(false)]
        public bool IsNRDay { get; set; }
        public float VWMA { get; set; }
        public float WMA { get; set; }
        public float EMA { get; set; }
        public float CurrentPrice { get; set; }

        [Browsable(false)]

        public float SMA { get; set; }
        [Browsable(false)]
        public float Close { get; set; }
        [Browsable(false)]
        double Trading_vol_Min { get; set; }
        [Browsable(false)]
        int DateDay { get; set; }
        [Browsable(false)]
        public string Exchange { get; set; }
        public void SetExchange(string s) { Exchange = s; }


        //ay
        [Browsable(false)]
        public double HighVolume90 { get; set; }
        [Browsable(false)]
        public double LowVolume90 { get; set; }

        public float HighPrice90 { get; set; }
        public float LowPrice90 { get; set; }

        [Browsable(false)]
        private float tvwma_pc { get; set; }
        public float TVWMA_PC { get { return tvwma_pc; } set { tvwma_pc = value; } }
        
        public float TVWMA { get; set; }
        public float TEMA { get; set; }
        public float THighest { get; set; }
        public float TLowest { get; set; }
    }
}
