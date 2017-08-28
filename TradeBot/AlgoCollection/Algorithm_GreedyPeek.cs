using Google;
using Quandl_FetchInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IExecuteOrder;
using Trading.Entity;
using TaxCalculator;
using Trading.Model;

namespace AlgoCollection
{
    public class Algorithm_GreedyPeek : Algorithm
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
        double history_trading_volume;

        float today_min_since_started = 0.01f;
        float today_max_since_started = 0.01f;

        bool bIsPurchased;

        //********** recent
        //        float today_new_Min = 0.01f;
        //        float today_new_Max = 0.01f;

        //********** Analytics for sale
        float prev_fetched_price;
        public static int acceptable_fall_count = 5;
        int fall_counter = acceptable_fall_count;
        float curr_stop_loss;
        float curr_be;
        float curr_target;
        float curr_lpet = 0.1f;
        float next_stop_loss = 0.01f;
        float next_be = 0.01f;
        float next_target = 0.01f;
        float next_lpet = 0.1f;
        public static float gross_profit_made = 0.0f;

        //********** Analytics for buy
        List<StringParsedData> gt_history_list = null;

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

        public bool IsCurrentMarketSegmentUp(float recent) { return prev_fetched_price <= recent ? true : false;  } // optimist ; same fetch is up
        public bool IsCurrentTargetMet(float recent) { return next_target < recent ? true : false; }
        public bool IsCurrentLpetMet(float recent) { return next_lpet < recent ? true : false;  }

        public List<StringParsedData> GetTodayHistory( int interval_in_seconds )
        {
            return gt_history_list;
        }

        public bool SetProfitMargins(float price, out float sl, out float be, out float tar,out float lpet  )
        {
            float today_mid_line = Formulas.banker_ceil((today_max + today_min) / 2.0f);
                        
            place_orders.BUY_STOCKS(price, 100, stock_name);

            var result = Formulas.generate_statistics(price);
            sl = result.Item1;
            be = result.Item2;
            tar = result.Item3;
            lpet = result.Item4;

            //Console.WriteLine("************************************ BUY STATs.");
            ////Console.WriteLine(string.Format("Buy Window     (L):{0:0.00##}  - (U):{1:0.00##}", buy_lower_limit, buy_upper_limit));
            //Console.WriteLine(string.Format("Purcased at       :{0:0.00##}", price));
            //Console.WriteLine(string.Format("StopLoss          :{0:0.00##}", sl));
            //Console.WriteLine(string.Format("Brek even (BE)    :{0:0.00##}", be));
            //Console.WriteLine(string.Format("Least profit ex   :{0:0.00##}", lpet));
            //Console.WriteLine(string.Format("Profit Target     :{0:0.00##}", tar));
            //Console.WriteLine("************************************ END.");

            return true;
        }
        public bool ResetProfitMargins(float price)
        {
            if (IsCurrentLpetMet(price))
            {
                SetProfitMargins(price, out next_stop_loss, out next_be, out next_target, out next_lpet);
            }
            else
                return false;

            return true;
        }

        public int Warm_up_time(UpdateScannerGridObject StockDetails)
        {
            int retVal = Warm_up_time(StockDetails.Exchange, StockDetails.Ticker, "", "");
            return retVal;
        }

        public int Warm_up_time(string exch, string ticker, string sd, string ed)
        {

            this.stock_name = ticker;

            //Console.WriteLine(ticker+ " : GreedyPeek warming up STARTED .......");
            if (place_orders == null)
                place_orders = new PlaceOrders(ticker);

            Daily_Reader todayReader = new Daily_Reader();
            todayReader.parser(exch, ticker, interval, 1); // 1 day = 1d, 5days=5d, 1 month = 1m, 1 year = 1Y

            today_min = todayReader.TodayMin;
            today_max = todayReader.TodayMax;
            today_mean = todayReader.TodayMean;
            today_median = todayReader.TodayMedian;

            buy_lower_limit = Formulas.getStopLossPrice(today_median);
            buy_upper_limit = Formulas.getBreakEvenPrice(today_median);

            List<QHistoryDatum> t_history_list = new List<QHistoryDatum>();
            QHistory hsNewObj = new QHistory(exch, ticker, sd, ed);
            //Console.WriteLine("Fetched history STARTED .......");
            if (hsNewObj.getHistoryCount() == 0)
                return -1;


            history_lowest_price = Formulas.banker_ceil(hsNewObj.Min);
            history_highest_price = Formulas.banker_ceil(hsNewObj.Max);
            history_mean_closing_price = Formulas.banker_ceil(hsNewObj.Mean);
            history_trading_volume = hsNewObj.Max_Trading_Volume;

            //Console.WriteLine("\n----------------------------------------STATISTICS.");
            //Console.WriteLine(this.stock_name);
            //Console.WriteLine("Start :" + sd + ", End :" + ed);
            //Console.WriteLine(string.Format("Today Least   :{0:0.00##}", today_min));
            //Console.WriteLine(string.Format("Today Maxim   :{0:0.00##}", today_max));
            //Console.WriteLine(string.Format("Today Mean    :{0:0.00##}", today_mean));
            //Console.WriteLine(string.Format("Today Median  :{0:0.00##}", today_median));
            //Console.WriteLine(string.Format("Buying Window (L):{0:0.00##}  - (U):{1:0.00##}", buy_lower_limit, buy_upper_limit));
            //Console.WriteLine(string.Format("QHistory Least :{0:0.00##}", history_lowest_price));
            //Console.WriteLine(string.Format("QHistory Maxim :{0:0.00##}", history_highest_price));
            //Console.WriteLine(string.Format("QHistory Mean  :{0:0.00##}", history_mean_closing_price));
            //Console.WriteLine("----------------------------------------- END.\n");


            

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

        public float Execute_Strategy(Func<CurrentOrderUpdater, int> func1, CurrentOrderUpdater objCurrentStatus, float fetched_price, int units)
        {
            return GreedyPeek_Strategy_Execute(func1, objCurrentStatus, fetched_price,  units);

        }

        private float GreedyPeek_Strategy_Execute(Func<CurrentOrderUpdater, int> Status_Updater, CurrentOrderUpdater objCurrentStatus, float fetched_price, int units)
        {

            //Find day trend for this sticker; if upward purchase otherwise find other stock
            objCurrentStatus.Current_Price = fetched_price;
            objCurrentStatus.Update_Time = DateTime.Now;
            if ( bIsPurchased )
            {
                //if( CONDITION_GREEDY_SALE_1 || // price > lpet 
                //    CONDITION_GREEDY_SALE_2 || // prev_fetch < latest_fetch market is rising ; update profit_target , if reached to new level
                //    CONDITION_GREEDY_SALE_3 || // prev_fetch > latest_fetch market is down ; reduce fall counter 
                //    CONDITION_GREEDY_SALE_4 ) // if fall counter is 0 or fallen down to lpet --> do sale
                //{

                if ( IsCurrentMarketSegmentUp(fetched_price) )
                {
                    if( true == ResetProfitMargins(fetched_price) )
                    {
                        // reseting hit counter once new price is set. <Greedy aint we..>
                        fall_counter = Algorithm_GreedyPeek.acceptable_fall_count;
                    }

                    //maintain hit count to max.
                    fall_counter = fall_counter >= Algorithm_GreedyPeek.acceptable_fall_count ? Algorithm_GreedyPeek.acceptable_fall_count : ++fall_counter; // looking for continous 4 hits
                    //Console.WriteLine(string.Format("======>Fall counter increased :{0:0.00##} ", fall_counter));
                }
                else
                {
                    
                    fall_counter--; // taking a hit
                    if (fall_counter <= 0)
                    {
                        if (next_lpet < fetched_price)
                        {
                            // if we are here ;that means that we are still profitable;
                            // run another round fall
                            fall_counter = Algorithm_GreedyPeek.acceptable_fall_count;
                            Console.WriteLine(string.Format("\n****Taking hit but we are profitable lpet:{0:0.00##} , fetch:{1:0.00##}\n ", next_lpet, fetched_price));
                        }
                        else
                        {
                            ///
                            /// this means we are taking hit; sale at loss or higher set lpet, no point of peeking
                            ///

                            trade_sale_price = fetched_price;
                            place_orders.SALE_ALL_STOCKS(trade_sale_price);
                            {

                                float zerTax = Formulas.getZerodha_Deductions(trade_purchase_price, trade_sale_price, units);

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

                                objCurrentStatus.Sell_Price = trade_sale_price;
                                objCurrentStatus.Sold_Time = DateTime.Now;
                                objCurrentStatus.TaxPaid = zerTax;
                                objCurrentStatus.NetProfit = curr_trade_profit;
                                
                            }


                            bIsPurchased = false;
                            //loss_counter = 0;

                            curr_stop_loss = 0.0f;
                            curr_be = 0.0f;
                            curr_target = 0.0f;
                            curr_lpet = 0.01f;
                            fall_counter = Algorithm_GreedyPeek.acceptable_fall_count;

                        }
                    }
                    
                    // cannot take hits any more.. lets get out.
                    //Console.WriteLine(string.Format("======>Fall counter decreased :{0:0.00##} ", fall_counter));
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


                    //float today_mid_line = Class1.banker_ceil((today_max + today_min) / 2.0f);

                    trade_purchase_price = fetched_price;

                    SetProfitMargins(fetched_price, out curr_stop_loss, out curr_be, out curr_target, out curr_lpet);

                    //default initialization
                    next_stop_loss = curr_stop_loss;
                    next_be = curr_be;
                    next_target = curr_target;
                    next_lpet = curr_lpet;

                    //place_orders.BUY_STOCKS(trade_purchase_price, 100, stock_name);
                    //var result = Class1.generate_statistics(trade_purchase_price);
                    //curr_stop_loss = result.Item1;
                    //curr_be = result.Item2;
                    //curr_exit = result.Item3;
                    //curr_lpet = result.Item4;

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


                    ///    Update UI for the statistics.
                    ///
                    objCurrentStatus.Purchased_Price = trade_purchase_price;
                    objCurrentStatus.BreakEven = curr_be;
                    objCurrentStatus.LeastProfitSell = curr_lpet;
                    objCurrentStatus.Current_Target = curr_target;
                    objCurrentStatus.StopLoss = curr_stop_loss;



                    // last_best_sale_price = newLpet;
                }
                // if new higher price of median; probably need to wait ?? or buy ?
                else
                {
                    
                }

                
            } // end of purchase block

            Status_Updater( objCurrentStatus );


            today_min = today_min > fetched_price ? fetched_price : today_min;
            today_max = today_max < fetched_price ? fetched_price : today_max;

            today_min_since_started = today_min_since_started > fetched_price ? fetched_price : today_min_since_started;
            today_max_since_started = today_max_since_started > fetched_price ? fetched_price : today_max_since_started;

            prev_fetched_price = fetched_price;


            return Formulas.banker_ceil(gross_profit_made);
        }
        ///


        public ActiveOrder Execute_Strategy(UpdateScannerGridObject StockDetails, int units)
        {
            return null;
        }

        public SaleOrder Execute_Strategy(ActiveOrder StockDetails)
        {
            return null;
        }


    }
}
