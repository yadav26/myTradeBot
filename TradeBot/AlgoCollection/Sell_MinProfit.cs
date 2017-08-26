
using IExecuteOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Entity;
using Trading.Model;

namespace AlgoCollection
{
    public class Sell_MinProfit : Algorithm
    {

        //********** trade instant price
        float trade_purchase_price;
        float trade_sale_price;

        //********** Analytics for purchase

        int interval = 60;// seconds or 1 min
        float today_min;
        float today_max;
        float today_mean;
        float today_median;
        float history_lowest_price;
        float history_highest_price;
        float history_mean_closing_price;

        bool bIsPurchased = false;

        //********** recent
        float today_new_Min;
        float today_new_Max;

        //********** Analytics for sale
        float curr_stop_loss;
        float curr_be;
        float curr_target;
        float curr_lpet;
        PlaceOrders place_orders = null;

        public float CurrentPrice { get; set; }


        string stock_name { get; set; }
        public float getMinPrice() { return today_min; }
        public float getMaxPrice() { return today_max; }
        public float getMeanPrice() { return today_mean; }
        public float getHsMinPrice() { return history_lowest_price; }
        public float getHsMaxPrice() { return history_highest_price; }
        public float getHsMeanPrice() { return history_mean_closing_price; }



        public int Warm_up_time(UpdateScannerGridObject StockDetails)
        {
            return 0;
        }

        public ActiveOrder Execute_Strategy(UpdateScannerGridObject StockDetails, int units)
        {
            this.stock_name = StockDetails.Ticker;

            float fetched_price = StockDetails.CurrentPrice;
            CurrentPrice = fetched_price;

            today_min = StockDetails.TLowest;
            today_max = StockDetails.THighest;

            history_lowest_price = StockDetails.Low90;
            history_highest_price = StockDetails.High90;

            ActiveOrder activeOrderDetails = null;

            if (fetched_price > curr_lpet && curr_lpet > 0) // save yourself from wrath of ZEROs && Conservative trade
            {
                trade_sale_price = fetched_price;
                place_orders.SALE_ALL_STOCKS(trade_sale_price);
                {

                    float zerTax = 0;// Formulas.getZerodha_Deductions(trade_purchase_price, trade_sale_price, units);

                    float curr_trade_profit = ((trade_sale_price - trade_purchase_price) * units) - zerTax;

                    ////gross_profit_made += curr_trade_profit;
                    //Console.WriteLine("\n------------------------SOLD Exit Stats.");
                    //Console.WriteLine(this.stock_name);
                    //Console.WriteLine(string.Format("Purcased:{0:0.00##}", trade_purchase_price));
                    //Console.WriteLine(string.Format("SOLD at :{0:0.00##}", fetched_price));
                    //Console.WriteLine(string.Format("Tax paid:{0:0.00##}", zerTax));
                    //Console.WriteLine(string.Format("Net P/L :{0:0.00##}", curr_trade_profit));
                    ////Console.WriteLine(string.Format("====Gross P/L:{0:0.00##}", gross_profit_made));
                    //Console.WriteLine("-------------------------------------- END.\n");

                }


                bIsPurchased = false;
                //loss_counter = 0;

                curr_stop_loss = 0.0f;
                curr_be = 0.0f;
                curr_target = 0.0f;
                curr_lpet = 0.0f;

            }
            return null;
        }
    }
}
