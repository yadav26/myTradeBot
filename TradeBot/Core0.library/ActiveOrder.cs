using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core0.library;

namespace Trading.DAL
{
    public class ActiveOrder
    {
        public int OrderID { get; set; }
        public string Ticker { get; set; }
        public float Purchased_Price { get; set; }
        public int Units { get; set; }
        public float BreakEven{ get; set; }
        public float Current_Price { get; set; }
        public float Profit { get; set; }
        public DateTime Buy_Time { get; set; }


        public ActiveOrder( int id, string t, float pp, int units, float cp )
        {
            OrderID = id;
            Ticker = t;
            Purchased_Price = pp;
            Units = units;
            Current_Price = cp;
            Buy_Time = DateTime.Now;

            BreakEven = Formulas.getBreakEvenPrice(cp);
            float total_taxes = 0;// Formulas.get_total_transaction_charges
            Profit = Formulas.netProfit(BreakEven, cp, units, total_taxes);

        }
    }
}
