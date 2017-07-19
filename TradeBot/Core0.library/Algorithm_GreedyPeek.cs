using Google;
using Quandl_FetchInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IExecuteOrder;
using Trading.DAL;

namespace Core0.library
{
    public class Algorithm_GreedyPeek
    {

        //********** trade instant price
        float trade_purchase_price;
        float trade_sale_price;

        //********** Analytics for purchase
        int interval = 60; // seconds or 1 min
        float today_min;
        float today_max;
        float today_mean;
        float today_median;
        float history_lowest_price;
        float history_highest_price;
        float history_mean_closing_price;

        float today_min_since_started = 0.01f;

        bool bIsPurchased;

        //********** recent
//        float today_new_Min = 0.01f;
//        float today_new_Max = 0.01f;

        //********** Analytics for sale
        public static int acceptable_fall_count = 4;
        float curr_stop_loss;
        float curr_be;
        float curr_target;
        float curr_lpet;
        public static float gross_profit_made = 0.0f;

        PlaceOrders place_orders = null;

        string stock_name { get; set; }
        float buy_lower_limit { get; set; }
        float buy_upper_limit { get; set;  }
        public float getMinPrice() { return today_min; }
        public float getMaxPrice() { return today_max; }
        public float getMeanPrice() { return today_mean; }
        public float getHsMinPrice() { return history_lowest_price; }
        public float getHsMaxPrice() { return history_highest_price; }
        public float getHsMeanPrice() { return history_mean_closing_price; }

        public int Warm_up_time(string exch, string ticker, string sd, string ed)
        {

            this.stock_name = ticker;

            List<GHistoryDatum> gt_history_list = new List<GHistoryDatum>();

            Console.WriteLine(ticker+ " : GreedyPeek warming up STARTED .......");
            if (place_orders == null)
                place_orders = new PlaceOrders(ticker);

            Daily_Reader todayReader = new Daily_Reader();
            todayReader.parser(exch, ticker, interval, "1d"); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

            today_min = todayReader.TodayMin;
            today_max = todayReader.TodayMax;
            today_mean = todayReader.TodayMean;
            today_median = todayReader.TodayMedian;

            buy_lower_limit = Class1.getStopLossPrice(today_median);
            buy_upper_limit = Class1.getBreakEvenPrice(today_median);



            List<HistoryDatum> t_history_list = new List<HistoryDatum>();
            History hsNewObj = new History(exch, ticker, sd, ed);
            //Console.WriteLine("Fetched history STARTED .......");
            if (hsNewObj.getHistoryCount() == 0)
                return -1;


            history_lowest_price = Class1.banker_ceil(hsNewObj.Min);
            history_highest_price = Class1.banker_ceil(hsNewObj.Max);
            history_mean_closing_price = Class1.banker_ceil(hsNewObj.Mean);

            Console.WriteLine("\n----------------------------------------STATISTICS.");
            Console.WriteLine(this.stock_name);
            Console.WriteLine("Start :" + sd + ", End :" + ed);
            Console.WriteLine(string.Format("Today Least   :{0:0.00##}", today_min));
            Console.WriteLine(string.Format("Today Maxim   :{0:0.00##}", today_max));
            Console.WriteLine(string.Format("Today Mean    :{0:0.00##}", today_mean));
            Console.WriteLine(string.Format("Today Median  :{0:0.00##}", today_median));
            Console.WriteLine(string.Format("Buying Window (L):{0:0.00##}  - (U):{1:0.00##}", buy_lower_limit, buy_upper_limit));
            Console.WriteLine(string.Format("History Least :{0:0.00##}", history_lowest_price));
            Console.WriteLine(string.Format("History Maxim :{0:0.00##}", history_highest_price));
            Console.WriteLine(string.Format("History Mean  :{0:0.00##}", history_mean_closing_price));
            Console.WriteLine("----------------------------------------- END.\n");

            #region Saving GoogleHistory Record in DB
            CommonDAL.CreateGoogleHistory(todayReader.GetGHistoryList());
            #endregion

            return 0;

        }


        /// <summary>
        /// ******************Greedy peek purchase*********************
        /// Set purchase price as the days median
        /// purchase if price hit and set sale target
        /// on market increase keep updating the higher price as new sale target
        /// on every fall increase fall counter
        /// once fall counter - 4 is over; be prepared to sell at anyprice 
        /// if price matched to lpet, sale before further fall 
        /// which is next lowest else,
        /// update target to newer higher price; untill price rises and sell on the immediate drop.
        /// </summary>


        public float GreedyPeek_Strategy_Execute(float fetched_price, int units)
        {
            
            //Find day trend for this sticker; if upward purchase otherwise find other stock
            //if( find_day_trend() == UPWARDS ){}
            if ( bIsPurchased )
            {

                if (fetched_price > curr_lpet && curr_lpet > 0) // save yourself from wrath of ZEROs && Conservative trade
                {
                    trade_sale_price = fetched_price;
                    place_orders.SALE_ALL_STOCKS(trade_sale_price);
                    {

                        float zerTax = Class1.getZerodha_Deductions(trade_purchase_price, trade_sale_price, units);

                        float curr_trade_profit = ((trade_sale_price - trade_purchase_price) * units) - zerTax;

                        gross_profit_made += curr_trade_profit;
                        Console.WriteLine("\n------------------------TRADE SELL Stats.");
                        Console.WriteLine(this.stock_name);
                        Console.WriteLine(string.Format("Purcased:{0:0.00##}", trade_purchase_price));
                        Console.WriteLine(string.Format("SOLD at :{0:0.00##}", fetched_price));
                        Console.WriteLine(string.Format("Tax paid:{0:0.00##}", zerTax));
                        Console.WriteLine(string.Format("Net P/L :{0:0.00##}", curr_trade_profit));
                        Console.WriteLine(string.Format("====Gross P/L:{0:0.00##}", gross_profit_made));
                        Console.WriteLine("-------------------------------------- END.\n");

                    }


                    bIsPurchased = false;
                    //loss_counter = 0;

                    curr_stop_loss = 0.0f;
                    curr_be = 0.0f;
                    curr_target = 0.0f;
                    curr_lpet = 0.0f;

                }

            }


            else // Purchase block
            {

                //if (fetched_price < history_lowest_price) // Ultra risky lesser than historical price ; definitly good deal to buy ; 
                //                                          //but it may go up in intra day trading, for longer run could go up and profitable
                //{
                //    history_lowest_price = fetched_price;

                //    return 0;
                //}
                //else if(fetched_price > history_highest_price)
                //{
                //    history_highest_price = fetched_price;
                //    //wait for price to come down
                //}
                //else

                if ( fetched_price > buy_lower_limit && fetched_price < buy_upper_limit ) // Anything below today max could be profitable for stable stock. buy it.
                {
                    /// here we buy ; 
                    /// have to decide the price ?? 
                    /// Ideally it should be 
                    /// lowest > day_open > day_lowest > price >day_highest >highest
                    /// 


                    float today_mid_line = Class1.banker_ceil((today_max + today_min) / 2.0f);

                    trade_purchase_price = fetched_price;
                    place_orders.BUY_STOCKS(trade_purchase_price, 100, stock_name);



                    var result = Class1.generate_statistics(trade_purchase_price);
                    curr_stop_loss = result.Item1;
                    curr_be = result.Item2;
                    curr_target = result.Item3;

                    curr_lpet = result.Item4;

                    Console.WriteLine("************************************ BUY STATs.");
                    Console.WriteLine(string.Format("Buy Window     (L):{0:0.00##}  - (U):{1:0.00##}", buy_lower_limit, buy_upper_limit));
                    Console.WriteLine(string.Format("Purcased at       :{0:0.00##}", trade_purchase_price));
                    Console.WriteLine(string.Format("StopLoss          :{0:0.00##}", curr_stop_loss));
                    Console.WriteLine(string.Format("Brek even (BE)    :{0:0.00##}", curr_be));
                    Console.WriteLine(string.Format("Least profit ex   :{0:0.00##}", curr_lpet));
                    Console.WriteLine(string.Format("Profit Target     :{0:0.00##}", curr_target));
                    Console.WriteLine("************************************ END.");

                    //recent_purchased_price = fetched_price;
                    bIsPurchased = true;
                    // last_best_sale_price = newLpet;
                }
                // if new higher price of median; probably need to wait ?? or buy ?
                else
                {
                    today_max = fetched_price;
                }

                
            } // end of purchase block


            
            return Class1.banker_ceil(gross_profit_made);
        }
        ///



    }
}
