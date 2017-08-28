using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;
using Trading.Model;

namespace Trading.Entity
{
    public class CompletedOrders
    {
        public string Ticker { get; set; }
        public float Profit { get; set; }
        public ActiveOrder OrderDetails;
        public CompletedOrders(ActiveOrder ao)
        {
            OrderDetails = ao;
            Ticker = ao.Ticker;
            float pp = ao.GetPurchaseOrderObject().Purchased_Price;
            float sp = ao.OrderSaleDetails.Sale_price;
            int units = ao.GetPurchaseOrderObject().Units;
            float tax = Formulas.getZerodha_Deductions(pp,sp,units);
            Profit = Formulas.netProfit(pp, sp, units,tax);
        }

    }

}
