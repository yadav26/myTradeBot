using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Entity
{
    public class CompletedOrders
    {
        public string Name { get; set; }
        public DateTime Purchased_Date { get; set; }
        public DateTime Sell_Date { get; set; }
        public int Units_sold { get; set; }
        public int Units_left { get; set; }

        public float Net_Profit { get; set; }
        
        public CompletedOrders(string t, float cp, int units, DateTime timeofpurchase)
        {
            Name =t;
            
        }
    }
}
