using Core0.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.DAL
{
    public class CompletedOrders
    {
        public string Name { get; set; }
        public float Sell_price { get; set; }
        public float Net_Profit { get; set; }
        public int Units_sold { get; set; }
        public int Units_Purchased { get; set; }
        public DateTime Purchased_Date { get; set; }
        public DateTime Sell_Date { get; set; }

        
        public float NetProfit(float pc, float sp, int units_bought, int units_sold)
        {
            float profit = 0.0f;

            profit = Formulas.netProfit(pc, sp, units_bought, Units_sold);
            return profit;
        }
        public CompletedOrders(string t, float sell, int units_sold, DateTime purchaseAt )
        {
            Name = t;
            Units_sold = units_sold;
            Purchased_Date = purchaseAt; //ORder READ from DB
            int Order_units_Count = 0; // order read from db, left units
            float pc = 0;
            Sell_Date = DateTime.Now;
            Sell_price = sell;
            Net_Profit = NetProfit(pc, sell, Order_units_Count, units_sold);
            Units_Purchased = Order_units_Count;

        }


    }
}
