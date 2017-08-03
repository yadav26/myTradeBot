using Google;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Entity.BusinessModel;

namespace Trading.DAL
{
    public class CommonDAL
    {
        public static bool CreateExchangeDetails(TradingBusinessModel businessModel)
        {
            try
            {
                using (SQLHelper helper = new SQLHelper())
                {
                    SqlCommand cmd = helper.GetStoreProcedureCommand("CreateExchangeDetails");
                    helper.AddInParameter(cmd, "@Google_Security_ID", System.Data.SqlDbType.NVarChar, string.IsNullOrEmpty(businessModel.GoogleSecurityID) ? string.Empty : businessModel.GoogleSecurityID);
                    helper.AddInParameter(cmd, "@Exchange", System.Data.SqlDbType.NVarChar, string.IsNullOrEmpty(businessModel.Exchange) ? string.Empty : businessModel.Exchange);
                    helper.AddInParameter(cmd, "@LastPrice", System.Data.SqlDbType.Money, businessModel.LastPrice);
                    helper.AddInParameter(cmd, "@Price", System.Data.SqlDbType.Money, businessModel.Price);
                    helper.AddInParameter(cmd, "@L_Cur", System.Data.SqlDbType.Money, businessModel.L_Cur);
                    helper.AddInParameter(cmd, "@S", System.Data.SqlDbType.Int, businessModel.S);
                    helper.AddInParameter(cmd, "@LastTradeTime", System.Data.SqlDbType.DateTime, businessModel.LastTradeTime);
                    helper.AddInParameter(cmd, "@LastTrdetimeformated", System.Data.SqlDbType.NVarChar, businessModel.LastTrdetimeformated.ToString());
                    helper.AddInParameter(cmd, "@LastTradeDateTime", System.Data.SqlDbType.NVarChar, businessModel.LastTradeDateTime.ToString());
                    helper.AddInParameter(cmd, "@Change", System.Data.SqlDbType.Money, businessModel.Change);
                    helper.AddInParameter(cmd, "@C_fix", System.Data.SqlDbType.Money, businessModel.C_fix);
                    helper.AddInParameter(cmd, "@ChangePercentage", System.Data.SqlDbType.Money, businessModel.ChangePercentage);
                    helper.AddInParameter(cmd, "@Cp_fix", System.Data.SqlDbType.Money, businessModel.Cp_fix);
                    helper.AddInParameter(cmd, "@Ccol", System.Data.SqlDbType.NVarChar, businessModel.Ccol.Replace(",", ""));
                    helper.AddInParameter(cmd, "@Pcls_Fix", System.Data.SqlDbType.Money, businessModel.Pcls_Fix);
                    helper.AddInParameter(cmd, "@Afterhourslastprice", System.Data.SqlDbType.Money, businessModel.Afterhourslastprice);
                    helper.AddInParameter(cmd, "@Afterhourslasttradetimeformated", System.Data.SqlDbType.DateTime, businessModel.Afterhourslasttradetimeformated);
                    helper.AddInParameter(cmd, "@GetActualRecordTime", System.Data.SqlDbType.DateTime, businessModel.GetActualtime);

                    helper.ExecuteNonQuery(cmd);
                
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool CreateTickerDetails(TickerModel tickerModel)
        {
            try
            {
                using (SQLHelper helper = new SQLHelper())
                {
                    SqlCommand cmd = helper.GetStoreProcedureCommand("CreateTickerDetails");
                    helper.AddInParameter(cmd, "@TICKER_NAME", System.Data.SqlDbType.NVarChar, string.IsNullOrEmpty(tickerModel.TickerName) ? string.Empty : tickerModel.TickerName);
                    helper.AddInParameter(cmd, "@TICKER_SYMBOL", System.Data.SqlDbType.NVarChar, string.IsNullOrEmpty(tickerModel.TickerSymbol) ? string.Empty : tickerModel.TickerSymbol);
                    helper.AddInParameter(cmd, "@TICKER_ISIN", System.Data.SqlDbType.NVarChar, tickerModel.ISIN);
                    helper.AddInParameter(cmd, "@TICKER_MARKETCAP", System.Data.SqlDbType.Money, tickerModel.MarketCapital);
                    helper.AddInParameter(cmd, "@TICKER_PERATIO", System.Data.SqlDbType.Money, tickerModel.PerRatio);
                    helper.AddInParameter(cmd, "@TICKER_DIVYIELD", System.Data.SqlDbType.Money, tickerModel.DivYield);
                    helper.AddInParameter(cmd, "@TICKER_STATUS", System.Data.SqlDbType.NVarChar, tickerModel.Status);
                    helper.AddInParameter(cmd, "@TICKER_VWAP", System.Data.SqlDbType.Money, tickerModel.VWAP);
                    helper.AddInParameter(cmd, "@TICKER_FACE_VALUE", System.Data.SqlDbType.Int, tickerModel.TickerFaceValue);

                    helper.ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public static bool CreateGoogleHistory(List<StringParsedData> lstGoogle)
        {
            try
            {
                foreach (var googleHistory in lstGoogle)
                {
                    using (SQLHelper helper = new SQLHelper())
                    {
                        SqlCommand cmd = helper.GetStoreProcedureCommand("CreateGoogleHistory");
                        helper.AddInParameter(cmd, "@TICKER_SYMBOL", System.Data.SqlDbType.NVarChar, string.IsNullOrEmpty(googleHistory.Ticker) ? string.Empty : googleHistory.Ticker);
                        helper.AddInParameter(cmd, "@OPEN_PRICE", System.Data.SqlDbType.Money, googleHistory.Open);
                        helper.AddInParameter(cmd, "@LOW_PRICE", System.Data.SqlDbType.Money, googleHistory.Low);
                        helper.AddInParameter(cmd, "@HIGH_PRICE", System.Data.SqlDbType.Money, googleHistory.High);
                        helper.AddInParameter(cmd, "@CLOSE_PRICE", System.Data.SqlDbType.Money, googleHistory.Close);
                        helper.AddInParameter(cmd, "@DATE_WITH_INTERVAL", System.Data.SqlDbType.Int, googleHistory.DateDay);

                        helper.ExecuteNonQuery(cmd);
                    }
                }
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
    }
}
