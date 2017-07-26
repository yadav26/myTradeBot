using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandl_FetchInterface
{
    public class QHistoryDatum
    {

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
