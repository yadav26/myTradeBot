using System;
using System.Data;
using System.Data.SqlClient;
using Trading.Entity.BusinessModel;

namespace Trading.DAL
{
    public class OrderDetailsDAL
    {
        public static bool CreateOrderMasterDetails(OrderDetailsModel orderDetials, out int orderID)
        {
            try
            {
                using (SQLHelper helper = new SQLHelper())
                {
                    SqlCommand cmd = helper.GetStoreProcedureCommand("CreateOrderMaster");
                    helper.AddInParameter(cmd, "@TICKER_SYMBOL", SqlDbType.NVarChar, string.IsNullOrEmpty(orderDetials.TickerSymbol) ? string.Empty : orderDetials.TickerSymbol);
                    helper.AddInParameter(cmd, "@LATEST_PRICE", SqlDbType.Money, orderDetials.LatestPrice);
                    helper.AddInParameter(cmd, "@DAY_GAIN", SqlDbType.Money, orderDetials.DayGain);
                    helper.AddInParameter(cmd, "@BOUGHT_PRICE", SqlDbType.Money, orderDetials.BoughtPrice);
                    helper.AddInParameter(cmd, "@UNITS", SqlDbType.Int, orderDetials.Units);
                    helper.AddInParameter(cmd, "@PURCHACE_DATE", SqlDbType.DateTime, orderDetials.PurchaceDate);
                    helper.AddOutParameter(cmd, "@ORDER_MASTER_ID", System.Data.SqlDbType.DateTime, orderDetials.OrderID);
                    helper.ExecuteNonQuery(cmd);
                    orderID = Convert.ToInt32(helper.GetParameterValue(cmd, "@ORDER_MASTER_ID"));
                }
                return true;
            }
            catch (Exception ex)
            {
                orderID = 0;
                return false;
            }
        }

        public static bool CreateOrderHistory(OrderHistoryModel orderHistory)
        {
            try
            {
                using (SQLHelper helper = new SQLHelper())
                {
                    SqlCommand cmd = helper.GetStoreProcedureCommand("CreateHistoryMaster");
                    helper.AddInParameter(cmd, "@ORDER_MASTER_ID", SqlDbType.Int, orderHistory.OrderID);
                    helper.AddInParameter(cmd, "@ORDER_CODE_NAME", SqlDbType.NVarChar, string.IsNullOrEmpty(orderHistory.CodeName) ? string.Empty : orderHistory.CodeName);
                    helper.AddInParameter(cmd, "@ORDER_TODAY_LEAST", SqlDbType.Money, orderHistory.TodayLeast);
                    helper.AddInParameter(cmd, "@ORDER_TODAY_MAXIM", SqlDbType.Money, orderHistory.TodayMaxim);
                    helper.AddInParameter(cmd, "@ORDER_TODAY_MEAN", SqlDbType.Money, orderHistory.TodayMean);
                    helper.AddInParameter(cmd, "@ORDER_BUYING_WINDOW_L", SqlDbType.Money, orderHistory.BuyingWindowL);
                    helper.AddInParameter(cmd, "@ORDER_BUYING_WINDOW_U", SqlDbType.Money, orderHistory.BuyingWindowU);
                    helper.AddInParameter(cmd, "@ORDER_HISTORY_LEAST", SqlDbType.Money, orderHistory.HistoryLeast);
                    helper.AddInParameter(cmd, "@ORDER_HISTORY_MAX", SqlDbType.Money, orderHistory.HistoryMax);
                    helper.AddInParameter(cmd, "@ORDER_HISTORY_MEAN", SqlDbType.Money, orderHistory.HistoryMean);
                    helper.AddInParameter(cmd, "@ORDER_PURCHASED_AT", SqlDbType.Money, orderHistory.PurcasedAt);
                    helper.AddInParameter(cmd, "@ORDER_STRATERGY", SqlDbType.Money, orderHistory.Stratergy);
                    helper.AddInParameter(cmd, "@ORDER_QUANTITY", SqlDbType.Int, orderHistory.Quantity);
                    helper.AddInParameter(cmd, "@ORDER_STOP_LOSS", SqlDbType.Money, orderHistory.StopLoss);
                    helper.AddInParameter(cmd, "@ORDER_BREAK_EVEN", SqlDbType.Money, orderHistory.BreakEven);
                    helper.AddInParameter(cmd, "@ORDER_LPET", SqlDbType.Money, orderHistory.LPET);
                    helper.AddInParameter(cmd, "@ORDER_PROFIT_TARGET", SqlDbType.Money, orderHistory.ProfitTarget);

                    helper.ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public static OrderDetailsModel GetOrderDetails(int orderID)
        //{
        //    try
        //    {
        //        using (SQLHelper helper = new SQLHelper())
        //        {
        //            SqlCommand cmd = helper.GetStoreProcedureCommand("GetOrderDetails");
        //            helper.AddInParameter(cmd, "@PURCHACE_DATE", SqlDbType.DateTime, orderID);
        //            helper.ExecuteNonQuery(cmd);
        //            orderID = Convert.ToInt32(helper.GetParameterValue(cmd, "@ORDER_MASTER_ID"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        orderID = 0;
        //        return false;
        //    }
        //}
    }
}
