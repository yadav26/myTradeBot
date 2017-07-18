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

        public static bool TestData()
        {
            try
            {
                using (SQLHelper helper = new SQLHelper())
                {
                    SqlCommand cmd = helper.GetStoreProcedureCommand("TestProcedure");
                    helper.AddInParameter(cmd, "@TestPrice", SqlDbType.Money, decimal.Parse("1.00"));
                    helper.AddInParameter(cmd, "@Name", System.Data.SqlDbType.NVarChar, "Arshad");
                    helper.AddInParameter(cmd, "@LatestPrice", System.Data.SqlDbType.Money, decimal.Parse("100.00"));
                    helper.AddInParameter(cmd, "@CreateDateTime", System.Data.SqlDbType.DateTime, DateTime.Now);

                    helper.ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch(Exception ex)
            { return false; }
        }
    }
}
