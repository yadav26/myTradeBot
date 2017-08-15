using System;
using System.Collections.Generic;
//using System.Linq;
using Trading.Model;
using Trading.Model.BusinessModel;

namespace Fetch_all_Co_data
{
    public class EntityMapperHelper
    {
        public static TradingBusinessModel GetBusinessModel(APIResultModel apiModel)
        {
            TradingBusinessModel tradingModel = null;
            if (apiModel != null)
            {
                tradingModel = new TradingBusinessModel();
                tradingModel.GoogleSecurityID = apiModel.ID;
                tradingModel.TickerKey = apiModel.T;
                tradingModel.Exchange = apiModel.E;
                tradingModel.Price = decimal.Parse(apiModel.L_Fix.Replace(",", ""));
                tradingModel.LastPrice = decimal.Parse(apiModel.L.Replace(",", ""));
                tradingModel.L_Cur = decimal.Parse(apiModel.L_Cur.Split(';')[1].Replace(",",string.Empty));
                tradingModel.S = int.Parse(apiModel.S);
                tradingModel.LastTradeTime = DateTime.Parse(apiModel.Ltt.Replace("GMT+5:30", string.Empty));
                tradingModel.LastTrdetimeformated = DateTime.Parse(apiModel.Lt.Replace("GMT+5:30", string.Empty).Replace(",", string.Empty));
                tradingModel.LastTradeDateTime = DateTime.Parse(apiModel.Lt_Dts.Replace("GMT+5:30", string.Empty).Replace(",", string.Empty));
                tradingModel.Change = decimal.Parse(apiModel.C);
                tradingModel.C_fix = decimal.Parse(apiModel.C_fix);
                tradingModel.ChangePercentage = decimal.Parse(apiModel.Cp);
                tradingModel.Cp_fix = decimal.Parse(apiModel.Cp_fix);
                tradingModel.Ccol = apiModel.Ccol;
                tradingModel.Pcls_Fix = decimal.Parse(apiModel.Pcls_Fix);
                tradingModel.Afterhourslastprice = string.IsNullOrEmpty(apiModel.el) ? decimal.Parse("0") : decimal.Parse(apiModel.el);
                tradingModel.Afterhourslasttradetimeformated = string.IsNullOrEmpty(apiModel.elt) ? DateTime.MaxValue : DateTime.Parse(apiModel.elt.Replace("GMT+5:30", string.Empty).Replace(",", string.Empty));
                tradingModel.GetActualtime = DateTime.Now;
            }
            return tradingModel;
        }
    }

    //"id": "358464", Google security ID
    //,"t" : "MSFT" Ticker
    //,"e" : "NASDAQ" Exchange
    //,"l" : "69.08" LastPrice
    //,"l_fix" : "69.08"  Price
    //,"l_cur" : "69.08"
    //,"s": "2"
    //,"ltt":"4:00pm GMT-4" LastTradeTime
    //,"lt" : "Jul 5, 4:00pm GMT-4" Lasttrdetimeformated
    //,"lt_dts" : "2017-07-05T16:00:02Z" LastTradeDateTime
    //,"c" : "+0.91" Change
    //,"c_fix" : "0.91"
    //,"cp" : "1.33" Change Percentage
    //,"cp_fix" : "1.33" Absolute price change
    //,"ccol" : "chg"
    //,"pcls_fix" : "68.17"
    //,"el": "69.08"   Afterhourslast price
    //,"el_fix": "69.08" NA
    //,"el_cur": "69.08" NA
    //,"elt" : "Jul 5, 7:54pm GMT-4" Afterhourslasttradetimeformated
    //,"ec" : "0.00"  Extenthourchange 
    //,"ec_fix" : "0.00"
    //,"ecp" : "0.00" Extenthourchange percentage
    //,"ecp_fix" : "0.00"
    //,"eccol" : "chb"
    //,"div" : "0.39" Divedent
    //,"yld" : "2.26" Divedentyeild
    //}
}
