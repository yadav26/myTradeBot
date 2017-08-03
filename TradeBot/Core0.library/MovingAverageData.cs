using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    public class MovingAverageData
    {
        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public float LastClose { get; set; }
        public float TodaySMA { get; set; }
        public float TodayEMA { get; set; }
        public float TodayWMA { get; set; }
        public int DateDay { get; set; }


    }

}
