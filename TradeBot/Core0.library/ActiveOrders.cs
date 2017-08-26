using Core0.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Entity;

namespace Core0.library
{
    public class ActiveOrders
    {
        private ActiveOrderModel OrderInfo = null;
        private Algorithm AlgoUsed = null;

        public ActiveOrders(ActiveOrderModel data, Algorithm algo )
        {
            if (null == OrderInfo)
                OrderInfo = new ActiveOrderModel(data.Ticker, 
                    data.OrderPurchaseDetails.Purchased_Price, 
                    data.OrderPurchaseDetails.Units,
                    OrderInfo.Current_Price, 
                    OrderInfo.OrderPurchaseDetails.BreakEven, 
                    OrderInfo.OrderPurchaseDetails.StopLoss, 
                    OrderInfo.OrderPurchaseDetails.ExitPrice, 
                    OrderInfo.OrderPurchaseDetails.EstimatedProfitPrice);

            AlgoUsed = algo;

        }

    }
}
