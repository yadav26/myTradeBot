﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Model
{
    public class UpdateScannerGridObject
    {
        [Browsable(false)]
        
        public float ltt { get; set; }
        [Browsable(false)]
        public float lt { get; set; }
        [Browsable(false)]
        public float lt_dts { get; set; }
        [Browsable(false)]
        public string ccol { get; set; }
        [Browsable(false)]
        public double id { get; set; }
        [Browsable(false)]
        //public float latestprice { get; set; }
        public float l_fix { get; set; }
        [Browsable(false)]
        public float l_cur { get; set; }
        [Browsable(false)]
        public float change { get; set; }
        [Browsable(false)]
        public float cp { get; set; }
        [Browsable(false)]
        public float cp_fix { get; set; }
        [Browsable(false)]
        public float pcls_fix { get; set; }

        public string Ticker { get; set; }

        [Browsable(false)]
        public string Exchange { get; set; }

        [Browsable(false)]
        private float tvwma_pc{ get; set; }

        public float TVWMA_PC { get { return tvwma_pc; } set { tvwma_pc = value; } }

        //public float LastClose { get; set; }
        public float CurrentPrice { get; set; }
        public float TVWMA { get; set; }
        public float TEMA { get; set; }
        //        public float Close { get; set; }
        public float THighest { get; set; }
        public float TLowest { get; set; }
        public float HighPrice90 { get; set; }
        public float LowPrice90 { get; set; }
        public double HighVolume90 { get; set; }
        public double LowVolume90 { get; set; }

        //      public float Open { get; set; }
        public double TVolume { get; set; }
        public float TWMA { get; set; }
        public float TSMA { get; set; }
        public int DateDay { get; set; }

        public AlgorithmType Id { get; set; }
        [DisplayName("Purchased")]
        public bool IsPurchased { get; set; }
        [DisplayName("SOLD")]
        public bool IsSold { get; set; }

        //private Algorithm m_Algorithm = null;

        public UpdateScannerGridObject( int id, string t, AlgorithmType algoID, bool bNr, float wma, float ema, float sma, float close, double vol, float h90, float l90 )
        {
            Ticker = t;
            Exchange = "NSE";
            HighPrice90 = h90;
            LowPrice90 = l90;
            Id = algoID;
            TWMA = wma;
            TEMA = ema;
            TSMA = sma;
            IsPurchased = false;
            IsSold = false;
        }

        public UpdateScannerGridObject()
        {
            ltt = 0;
            lt = 0;
            lt_dts = 0;

        }
        
    }
    public enum AlgorithmType
    {
        BuyMedianPrice = 0,
        GreedyPeek,
        SellMinProfit
    }

}
