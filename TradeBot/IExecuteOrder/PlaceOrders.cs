using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IExecuteOrder
{
    public class PlaceOrders
    {
        float total_units_transacted { get; set; }
        float trade_purchased_price { get; set; }
        float trade_sale_price { get; set; }
        float tradeStopLoss { get; set; }
        float tradeBE { get; set; }
        float tradeTarget { get; set; }
        float tradeLpet { get; set; }
        string tradeticker{ get; set; }

        public PlaceOrders(string input) { tradeticker = input; }

        public void BUY_STOCKS(float price, int units, string ticker )
        {
            total_units_transacted = units;
            trade_purchased_price = price;
            tradeticker = ticker;

            //var result = Class1.generate_statistics(price);
            //tradeStopLoss = result.Item1;
            //tradeBE = result.Item2;
            //tradeTarget = result.Item3;
            //tradeLpet = result.Item4;

            //float money_invested = price * total_units_transacted;


            //Console.WriteLine("************************************ BUY STATs.");
            //Console.WriteLine(tradeticker);
            //Console.WriteLine(string.Format("Purcased  :{0:0.00##}", trade_purchased_price));
            //Console.WriteLine(string.Format("StopLoss..:{0:0.00##}", tradeStopLoss));
            //Console.WriteLine(string.Format("BE........:{0:0.00##}", tradeBE));
            //Console.WriteLine(string.Format("Lpet......:{0:0.00##}", tradeLpet));
            //Console.WriteLine(string.Format("Target....:{0:0.00##}", tradeTarget));
            //Console.WriteLine("************************************ END.");


            return ;// Class1.banker_ceil( money_invested);
        }

        public void SALE_ALL_STOCKS(float price)
        {
            //int total_units_purchased = 100;
            float money_returned = price * total_units_transacted;
            trade_sale_price = price;
            //float zerTax = Class1.getZerodha_Deductions(trade_purchased_price, trade_sale_price, total_units_transacted);

            //float curr_trade_profit = ((trade_sale_price - trade_purchased_price) * total_units_transacted) - zerTax;

            //Console.WriteLine("\n-----------------------------------------TRADE stats.");
            //Console.WriteLine(tradeticker);
            //Console.WriteLine(string.Format("Purcased  :{0:0.00##}", trade_purchased_price));
            //Console.WriteLine(string.Format("SOLD at   :{0:0.00##}", trade_sale_price));
            //Console.WriteLine(string.Format("Tax paid  :{0:0.00##}", zerTax));
            //Console.WriteLine(string.Format("Net P/L   :{0:0.00##}", curr_trade_profit));
            //Console.WriteLine("------------------------------------------ END.\n");

        }
    }
}
