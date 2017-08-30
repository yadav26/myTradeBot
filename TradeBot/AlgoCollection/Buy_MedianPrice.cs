
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
    public class Buy_MedianPrice : Algorithm
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

        private float today_vwma;

        public float Today_VWMA { get { return today_vwma; } set {today_vwma = value; } }

        public float CurrentPrice { get; set; }


        string stock_name { get; set; }
        public float getMinPrice() { return today_min; }
        public float getMaxPrice() { return today_max; }
        public float getMeanPrice() { return today_mean; }
        public float getHsMinPrice() { return history_lowest_price; }
        public float getHsMaxPrice() { return history_highest_price; }
        public float getHsMeanPrice() { return history_mean_closing_price; }

        public Buy_MedianPrice( string t, float cp, float tlowest, float thighest, float l90, float h90, float vwma)
        {
            stock_name = t;
            CurrentPrice = cp;
            today_min = tlowest;
            today_max = thighest;
            history_lowest_price = l90;
            history_highest_price = h90;
            Today_VWMA = vwma;
        }

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

            history_lowest_price = StockDetails.LowPrice90;
            history_highest_price = StockDetails.HighPrice90;

            ActiveOrder activeOrderDetails = null;
            

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
                float today_mid_line =(today_max + today_min) / 2.0f;

                //if (fetched_price < history_mean_closing_price ||
                //    fetched_price <= today_median
                //    )
                if( fetched_price > Today_VWMA )
                {

                    int units_to_buy = 100;
                    trade_purchase_price = fetched_price;
                    //place_orders.BUY_STOCKS(trade_purchase_price, units_to_buy, stock_name);


                    //recent_purchased_price = fetched_price;
                    bIsPurchased = true;
                    // last_best_sale_price = newLpet;

                    activeOrderDetails = new ActiveOrder(stock_name, trade_purchase_price, units_to_buy );

                }
            }
            
            return activeOrderDetails;

        } // end of buy Execute_Strategy

        // Empty implementation.
        public SaleOrder Execute_Strategy(ActiveOrder StockDetails)
        {
            return null;
        }

    }


}
