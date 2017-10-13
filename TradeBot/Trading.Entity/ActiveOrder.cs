using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;

namespace Trading.Model
{
    public class PurchaseOrder
    {

        private float units;
        public float Units { get { return units; } set { units = value; } }

        private float purchased_price;
        
        public float Purchased_Price { get { return purchased_price; } set { purchased_price = value; } }

        private float stoploss;
        public float StopLoss{ get { return stoploss; } set { stoploss = value; } }

        private float exitprice;
        public float ExitPrice { get { return exitprice; } set { exitprice = value; } }

        private float current_profit;
        public float Current_Profit{ get { return current_profit; } set { current_profit = value; } }

        private float breakeven;
        public float BreakEven { get { return breakeven; } set { breakeven = value; } }


        private float leastprofitexit;
        public float LeastProfitExit { get {return leastprofitexit; } set {leastprofitexit=value; } }

        private DateTime purchase_time;
        public DateTime Purchase_Time { get { return purchase_time; } set { purchase_time = value; } }


        public PurchaseOrder()
        {

        }

        public PurchaseOrder(float pp, float units)
        {
            
            Purchased_Price = pp;
            Units = units;
            var result = Formulas.generate_statistics(pp);
            StopLoss = result.Item1;
            BreakEven = result.Item2;
            ExitPrice = result.Item3;
            LeastProfitExit = result.Item4;
            Purchase_Time = DateTime.Now;

        }

    }

    public class SaleOrder
    {
        public float Sale_price { get; set; }
        private float profit_made;

        public SaleOrder()
        {

        }
        public SaleOrder( float pp, float sp, float units )
        {
            Sale_price = sp;
            float zerTax = Formulas.getZerodha_Deductions(pp, sp, units);

            float curr_trade_profit = ((sp - pp) * units) - zerTax;
            profit_made = curr_trade_profit;
        }
    }

    public class ActiveOrder
    {


        public int OrderID { get; set; }

        private string ticker;
        public string Ticker { get { return ticker; } set { ticker = value; } }

        private float current_price;
        public float Current_Price { get { return current_price; } set { current_price = value; } }


        private float current_profit;
        public float Current_Profit { get { return current_profit; } set { current_profit = value; } }


        private PurchaseOrder OrderPurchaseDetails = null;
        public PurchaseOrder PurchaseOrder { get { return OrderPurchaseDetails; } set { OrderPurchaseDetails = value; } }


        private SaleOrder OrderSaleDetails = null;
        public SaleOrder SaleOrder { get { return OrderSaleDetails; } set { OrderSaleDetails = value; } }



        private static int ORDER_ID = 0;


        public int id_algorithm_purchase;
        public int id_algorithm_sale;

        public ActiveOrder(string ticker, float purchased_price, float units )
        {
            if (OrderPurchaseDetails == null)
                OrderPurchaseDetails = new PurchaseOrder(purchased_price, units );

            OrderID = ORDER_ID++; // read from DB
            Ticker = ticker;
            Current_Price = purchased_price;
            float tax = Formulas.getZerodha_Deductions(purchased_price, Current_Price, units);
            Current_Profit = Formulas.netProfit(purchased_price, Current_Price, units, tax);

            //Burn DB
        }

        public PurchaseOrder GetPurchaseOrderObject()
        {
            return OrderPurchaseDetails;
        }


    }
}
