using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandl_FetchInterface
{
    public class QHistoryDatum
    {
        //https://www.quandl.com/api/v3/datasets/NSE/SBIN.json?start_date=2017-01-01&end_date=2017-07-10&api_key=G5DQsRtXsqqGZ5kY8kwU
        //["Date","Open","High","Low","Last","Close","Total Trade Quantity","Turnover (Lacs)"]
        //["2017-07-12",284.7,288.4,283.2,287.8,287.65,7524835.0,21505.32]
        public string Date { get; set; }
        public string Open { get; set; }
        public string Close { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Last{ get; set; }
        public string Total_Trade_Quantity { get; set; }
        public string Turn_Over{ get; set; }



    }
}
