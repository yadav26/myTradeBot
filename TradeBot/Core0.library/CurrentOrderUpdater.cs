using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    public class CurrentOrderUpdater
    {
        public int OrderID { get; set; }
        public string Ticker { get; set; }

/// <summary>
/// Buying details
/// </summary>
        public float Purchased_Price { get; set; }
        public int Units { get; set; }
        public float BreakEven { get; set; }


/// <summary>
/// Statistics At purchase
/// </summary>
        public float Current_Price { get; set; }
        public DateTime Update_Time { get; set; }
        public float StopLoss { get; set; }
        public float LeastProfitSell { get; set; }
        public float Current_Target { get; set; }

/// <summary>
/// Closing details
/// </summary>

        public float TaxPaid { get; set; }
        public float Sell_Price { get; set; }
        public float NetProfit { get; set; }
        public DateTime Sold_Time { get; set; }

        public CurrentOrderUpdater()
        {
            OrderID = -1;
            Ticker = string.Empty;
            Purchased_Price = 0;
            Units = 0;
            Current_Price = 0;
            Update_Time = new DateTime();

            BreakEven = 0;
            
            NetProfit = 0; 
        }

        public CurrentOrderUpdater(int id, string t, float pp, int units, float cp)
        {
            OrderID = id;
            Ticker = t;
            Purchased_Price = pp;
            Units = units;
            Current_Price = cp;
            Update_Time = DateTime.Now;

            BreakEven = Formulas.getBreakEvenPrice(cp);
            float total_taxes = 0;// Formulas.get_total_transaction_charges
            NetProfit = Formulas.netProfit(BreakEven, cp, units, total_taxes);

        }
    }
}
