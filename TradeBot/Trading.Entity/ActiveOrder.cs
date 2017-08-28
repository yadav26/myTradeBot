using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;

namespace Trading.Model
{
    public class PurchaseOrder
    {
        public int OrderID { get; set; }
        public string Ticker { get; set; }
        public float Purchased_Price { get; set; }

        public float StopLoss { get; set; }
        public float ExitPrice { get; set; }
        public float EstimatedProfitPrice { get; set; }

        public int Units { get; set; }
        public float BreakEven { get; set; }
        public float Current_Price { get; set; }

        public float LeastProfitExit { get; set; }


        public DateTime Pruchase_Time { get; set; }

        //private Algorithms AlgorithmID { get; set; }

        public PurchaseOrder(float pp, int units)
        {
            
            Purchased_Price = pp;
            Units = units;
            var result = Formulas.generate_statistics(pp);
            StopLoss = result.Item1;
            BreakEven = result.Item2;
            ExitPrice = result.Item3;
            LeastProfitExit = result.Item4;
            Pruchase_Time = DateTime.Now;

            //Console.WriteLine("************************************ BUY STATs.");
            //Console.WriteLine(string.Format("Purcased:{0:0.00##}", trade_purchase_price));
            //Console.WriteLine(string.Format("StopLoss:{0:0.00##}", curr_stop_loss));
            //Console.WriteLine(string.Format("BE:{0:0.00##}", curr_be));
            //Console.WriteLine(string.Format("Lpet:{0:0.00##}", curr_lpet));
            //Console.WriteLine(string.Format("Target:{0:0.00##}", curr_target));
            //Console.WriteLine("************************************ END.");

        }

    }

    public class SaleOrder
    {
        public float Sale_price { get; set; }
        private float profit_made;

        public SaleOrder( float pp, float sp, int units )
        {

            float zerTax = Formulas.getZerodha_Deductions(pp, sp, units);

            float curr_trade_profit = ((sp - pp) * units) - zerTax;
        }
    }

    public class ActiveOrder
    {
        public int OrderID { get; set; }
        public string Ticker { get; set; }
        public float Current_Price { get; set; }
        public float CurrentProfit { get; set; }

        private PurchaseOrder OrderPurchaseDetails = null;
        public SaleOrder OrderSaleDetails = null;

        private static int ORDER_ID = 0;
        public float Profit { get; set; }

        public int id_algorithm_purchase;
        public int id_algorithm_sale;

        public ActiveOrder(string ticker, float purchased_price, int units )
        {
            if (OrderPurchaseDetails == null)
                OrderPurchaseDetails = new PurchaseOrder(purchased_price, units );

            OrderID = ORDER_ID++; // read from DB
            Ticker = ticker;
            Current_Price = purchased_price;


            //Burn DB
        }

        public PurchaseOrder GetPurchaseOrderObject()
        {
            return OrderPurchaseDetails;
        }


    }
}
