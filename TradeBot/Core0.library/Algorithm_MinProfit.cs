using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Quandl_FetchInterface;
using Google;
using IExecuteOrder;
using Trading.Entity;

namespace Core0.library
{
    // This algorithm is about to run the bot @ minimum profit 
    // i.e. 1.5% of investment above break-even cost.
    // so we will poll for the minimum threshold price and once attained, will liquidate all assests.

    // Stop loss TOLERANCE is @ 1% including BE , i.e. purchase cost * 0.99 ; 
    public class Algorithm_MinProfit : Algorithm
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


        string stock_name{ get; set; }
        public float getMinPrice() { return today_min; }
        public float getMaxPrice() { return today_max; }
        public float getMeanPrice() { return today_mean; }
        public float getHsMinPrice() { return history_lowest_price; }
        public float getHsMaxPrice() { return history_highest_price; }
        public float getHsMeanPrice() { return history_mean_closing_price; }



        public int Warm_up_time(UpdateScannerGridObject StockDetails )
        {

            this.stock_name = StockDetails .Ticker;

            CurrentPrice = StockDetails.CurrentPrice;

            //List<StringParsedData> gt_history_list = new List<StringParsedData>();

            //Console.WriteLine( ticker + ": MinProfit WARMIN UP STARTED .......");
            //if(place_orders == null)
            //    place_orders = new PlaceOrders(ticker);

            //Daily_Reader todayReader = new Daily_Reader();
            //todayReader.parser(exch, ticker, interval, 1);

            today_min = StockDetails.TLowest;
            today_max = StockDetails.THighest;
            //today_mean = StockDetails.;
            //today_median = todayReader.TodayMedian;

            
            //List<QHistoryDatum> t_history_list = new List<QHistoryDatum>();
            //QHistory hsNewObj = new QHistory(exch, ticker, sd, ed);
            //Console.WriteLine("Fetched history STARTED .......");
            //if (hsNewObj.getHistoryCount() == 0)
                //return -1;


            history_lowest_price = StockDetails.Low90; 
            history_highest_price = StockDetails.High90; 
            //history_mean_closing_price = Formulas.banker_ceil(hsNewObj.Mean);


            //Console.WriteLine("\n----------------------------------------STATISTICS.");
            //Console.WriteLine(this.stock_name);
            //Console.WriteLine("Start :" + sd + ", End :" + ed);
            //Console.WriteLine(string.Format("Today Least:{0:0.00##}", today_min));
            //Console.WriteLine(string.Format("Today Maxim:{0:0.00##}", today_max));
            //Console.WriteLine(string.Format("Today Mean :{0:0.00##}", today_mean));
            //Console.WriteLine(string.Format("Today Median :{0:0.00##}", today_median));
            //Console.WriteLine(string.Format("QHistory Least:{0:0.00##}", history_lowest_price));
            //Console.WriteLine(string.Format("QHistory Maxim:{0:0.00##}", history_highest_price));
            //Console.WriteLine(string.Format("QHistory Mean :{0:0.00##}", history_mean_closing_price));
            //Console.WriteLine("----------------------------------------- END.\n");


            return 0;

        }

        
        public ActiveOrder Execute_Strategy(UpdateScannerGridObject obj, int units)
        {
            return MinProfit_Strategy_Execute(obj, units);

        }


        private ActiveOrder MinProfit_Strategy_Execute(UpdateScannerGridObject StockDetails, int units)
        {
            //float gross_profit_made = 0.0f;
            //Find day trend for this sticker; if upward purchase otherwise find other stock
            //if( find_day_trend() == UPWARDS ){}
            this.stock_name = StockDetails.Ticker;

            float fetched_price = StockDetails.CurrentPrice;
            CurrentPrice = fetched_price;

            today_min = StockDetails.TLowest;
            today_max = StockDetails.THighest;

            history_lowest_price = StockDetails.Low90;
            history_highest_price = StockDetails.High90;

            ActiveOrder activeOrderDetails = null;

            if (bIsPurchased)
            {

                if (fetched_price > curr_lpet && curr_lpet > 0) // save yourself from wrath of ZEROs && Conservative trade
                {
                    trade_sale_price = fetched_price;
                    place_orders.SALE_ALL_STOCKS(trade_sale_price);
                    {

                        float zerTax = Formulas.getZerodha_Deductions(trade_purchase_price, trade_sale_price, units);

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

            }
            else
            {

                if (fetched_price < history_lowest_price) // Ultra risky lesser than historical price ; definitly good deal to buy ; 
                                                          //but it may go up in intra day trading, for longer run could go up and profitable
                {
                    history_lowest_price = fetched_price;

                    //return activeOrderDetails;
                }
                else if (fetched_price > history_highest_price)
                {
                    history_highest_price = fetched_price;
                    //wait for price to come down
                }
                else
                {
                    /// here we buy ; 
                    /// have to decide the price ?? 
                    /// Ideally it should be 
                    /// lowest > day_open > day_lowest > price >day_highest >highest
                    /// OR
                    /// We should purchase at the MEDIAN -> sort all and find the mid point
                    float today_mid_line = Formulas.banker_ceil((today_max + today_min) / 2.0f);

                    if (fetched_price < history_mean_closing_price ||
                        fetched_price <= today_median
                        )
                    {

                        int units_to_buy = 100;
                        trade_purchase_price = fetched_price;
                        place_orders.BUY_STOCKS(trade_purchase_price, units_to_buy, stock_name);

                        var result = Formulas.generate_statistics(trade_purchase_price);
                        curr_stop_loss = result.Item1;
                        curr_be = result.Item2;
                        curr_target = result.Item3;
                        curr_lpet = result.Item4;

                        //Console.WriteLine("************************************ BUY STATs.");
                        //Console.WriteLine(string.Format("Purcased:{0:0.00##}", trade_purchase_price));
                        //Console.WriteLine(string.Format("StopLoss:{0:0.00##}", curr_stop_loss));
                        //Console.WriteLine(string.Format("BE:{0:0.00##}", curr_be));
                        //Console.WriteLine(string.Format("Lpet:{0:0.00##}", curr_lpet));
                        //Console.WriteLine(string.Format("Target:{0:0.00##}", curr_target));
                        //Console.WriteLine("************************************ END.");

                        //recent_purchased_price = fetched_price;
                        bIsPurchased = true;
                        // last_best_sale_price = newLpet;

                       activeOrderDetails = new ActiveOrder( stock_name, trade_purchase_price, units_to_buy, fetched_price, curr_be, curr_stop_loss, curr_lpet, curr_target);

                    }
                }
            }


            return activeOrderDetails;
        }



    }
}
