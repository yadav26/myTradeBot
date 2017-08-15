using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Model;

namespace Trading.DAL
{
    public class MarketAnalysisDataumDAL
    {
        public static bool CreateMarketAnalysisData(List<MarketAnalysisDataumModel> lstMarketAnalysisData)
        {
            try
            {
                //using (SQLHelper helper = new SQLHelper())
                //{
                //    SqlCommand cmd = helper.GetStoreProcedureCommand("CreateMarketAnalysisDataum");
                //    helper.AddInParameter(cmd, "@TICKER_SYMBOL", System.Data.SqlDbType.NVarChar, string.IsNullOrEmpty(model.Ticker) ? string.Empty : model.Ticker);
                //    helper.AddInParameter(cmd, "@TICKER_ISIN", System.Data.SqlDbType.NVarChar, tickerModel.ISIN);
                //    helper.AddInParameter(cmd, "@TICKER_MARKETCAP", System.Data.SqlDbType.Money, tickerModel.MarketCapital);
                //    helper.AddInParameter(cmd, "@TICKER_PERATIO", System.Data.SqlDbType.Money, tickerModel.PerRatio);
                //    helper.AddInParameter(cmd, "@TICKER_DIVYIELD", System.Data.SqlDbType.Money, tickerModel.DivYield);
                //    helper.AddInParameter(cmd, "@TICKER_STATUS", System.Data.SqlDbType.NVarChar, tickerModel.Status);
                //    helper.AddInParameter(cmd, "@TICKER_VWAP", System.Data.SqlDbType.Money, tickerModel.VWAP);
                //    helper.AddInParameter(cmd, "@TICKER_FACE_VALUE", System.Data.SqlDbType.Int, tickerModel.TickerFaceValue);

                //    helper.ExecuteNonQuery(cmd);
                //}
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
    }
}
