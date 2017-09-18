
using Google;
using IExecuteOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator;
using Trading.Entity;
using Trading.Model;

namespace AlgoCollection
{
    public class Sell_MinProfit : Algorithm
    {

        //********** trade instant price
        float trade_purchase_price;
        //float trade_sale_price;

        private SaleOrder so;
        //********** Analytics for purchase

        public int TIMEOUT_SALE_WAIT { get; set; }

        //int TIMEOUT_INTERVAL;// seconds or 1 min
        //float today_min;
        //float today_max;
        //float today_mean;
        //float today_median;
        //float history_lowest_price;
        //float history_highest_price;
        //float history_mean_closing_price;

        //bool bIsPurchased = false;

        //********** recent
        //float today_new_Min;
        //float today_new_Max;

        //********** Analytics for sale
        float curr_stop_loss;
        float curr_be;
        float curr_exit;
        float curr_lpet;
        int Units_to_sell;

        //PlaceOrders place_orders = null;

        public float CurrentPrice { get; set; }


        string stock_name { get; set; }
        public float exit_price { get; private set; }

        //public float getMinPrice() { return today_min; }
        //public float getMaxPrice() { return today_max; }
        //public float getMeanPrice() { return today_mean; }
        //public float getHsMinPrice() { return history_lowest_price; }
        //public float getHsMaxPrice() { return history_highest_price; }
        //public float getHsMeanPrice() { return history_mean_closing_price; }

        public Sell_MinProfit( ActiveOrder aoStock)
        {
            CurrentPrice = aoStock.Current_Price;
            trade_purchase_price = aoStock.GetPurchaseOrderObject().Purchased_Price;
            curr_be = aoStock.GetPurchaseOrderObject().BreakEven;
            curr_exit = aoStock.GetPurchaseOrderObject().ExitPrice;
            Units_to_sell = aoStock.GetPurchaseOrderObject().Units;
            curr_stop_loss = aoStock.GetPurchaseOrderObject().StopLoss;
            exit_price = aoStock.GetPurchaseOrderObject().ExitPrice;
            curr_lpet = aoStock.GetPurchaseOrderObject().LeastProfitExit;

            TIMEOUT_SALE_WAIT = 1;
        }

        public int Warm_up_time(UpdateScannerGridObject StockDetails)
        {
            return 0;
        }


        public ActiveOrder Execute_Strategy(UpdateScannerGridObject StockDetails, int units)
        {
            return null;
        }

        public SaleOrder Execute_Strategy(ActiveOrder StockDetails)
        {
            this.stock_name = StockDetails.Ticker;

            //float purchased_price = StockDetails.GetPurchaseOrderObject().Purchased_Price;
            //float be = StockDetails.GetPurchaseOrderObject().BreakEven;
            //float stop_loss = StockDetails.GetPurchaseOrderObject().StopLoss;
            //float leastProfitExit = StockDetails.GetPurchaseOrderObject().ExitPrice;
            float current_profit = StockDetails.GetPurchaseOrderObject().Current_Profit;

            //float curr_price = StockDetails.CurrentPrice;


            DateTime purchasetime = StockDetails.GetPurchaseOrderObject().Purchase_Time;
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(purchasetime);



            // if we didnt get above following map's last entry should be the latest price
            //https://www.google.com/finance/getprices?q=SBIN&x=NSE&i=600&p=1d&f=d,c
            Daily_Reader todayReader1 = new Daily_Reader();
            todayReader1.parser("NSE", stock_name, 600, 1); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y
            List<StringParsedData> ghs1 = todayReader1.GetGHistoryList();
            if (null == ghs1)
                return null;

            CurrentPrice = ghs1[ghs1.Count - 1].Close;



            if (CurrentPrice > curr_exit ||
                CurrentPrice <= curr_stop_loss ||
                (span.TotalMinutes > TIMEOUT_SALE_WAIT && CurrentPrice >= curr_be)
                ) // save yourself from wrath of ZEROs && Conservative trade
            {

                float trade_sale_price = CurrentPrice;
                if (trade_purchase_price > trade_sale_price)
                {
                    float loss = trade_sale_price - trade_purchase_price;
                }
                so = new SaleOrder(trade_purchase_price, trade_sale_price, Units_to_sell);

                //place_orders.SALE_ALL_STOCKS(trade_sale_price);
                //{
                //    ////gross_profit_made += curr_trade_profit;
                //    //Console.WriteLine("\n------------------------SOLD Exit Stats.");
                //    //Console.WriteLine(this.stock_name);
                //    //Console.WriteLine(string.Format("Purcased:{0:0.00##}", trade_purchase_price));
                //    //Console.WriteLine(string.Format("SOLD at :{0:0.00##}", fetched_price));
                //    //Console.WriteLine(string.Format("Tax paid:{0:0.00##}", zerTax));
                //    //Console.WriteLine(string.Format("Net P/L :{0:0.00##}", curr_trade_profit));
                //    ////Console.WriteLine(string.Format("====Gross P/L:{0:0.00##}", gross_profit_made));
                //    //Console.WriteLine("-------------------------------------- END.\n");
                //}
            }

            return so;
        }

    }

}
