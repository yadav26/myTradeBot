﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Trading.Entity
{
    public class PurchaseOrderModel
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

        public DateTime Pruchase_Time { get; set; }

        public PurchaseOrderModel(int pid, float pp, int units, float be, float sl, float min_exit_price, float targetpriceset)
        {
            OrderID = pid;
            Purchased_Price = pp;
            Units = units;
            BreakEven = be;
            StopLoss = sl;
            ExitPrice = min_exit_price;
            EstimatedProfitPrice = targetpriceset;

            Pruchase_Time = DateTime.Now;
        }
    }
    public class ActiveOrderModel
    {
        public int OrderID { get; set; }
        public string Ticker { get; set; }
        public float Current_Price { get; set; }
        public float CurrentProfit { get; set; }
        public PurchaseOrderModel OrderPurchaseDetails = null;
        private static int ORDER_ID = 0;
        public float Profit { get; set; }



        public ActiveOrderModel( string ticker, float purchased_price, int units, float current_price, float breakEven, float stop_loss, float estimated_exit_price, float Proft_target )
        {
            OrderPurchaseDetails = new PurchaseOrderModel(ORDER_ID, purchased_price, units, breakEven, stop_loss, estimated_exit_price, Proft_target );
            OrderID = ORDER_ID++; // read from DB
            Ticker = ticker;
            Current_Price = current_price;
            //Burn DB
        }

        public PurchaseOrderModel GetPurchaseOrderObject()
        {
            return OrderPurchaseDetails;

        }


    }
}
