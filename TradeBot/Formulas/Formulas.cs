using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaxCalculator
{
    public class Formulas
    {
        private static float BROKERAGE_PH = 0.01f;
        
        private static float SECURITY_TT_PH	= 0.025f;
        private static float TRANSACTION_CHARGES_PH = 0.00325f;
        private static float GST_PH = 18.0f;
        private static float PERCENT_DIVISOR = 100.0f;
        private static float STAMP_DUTY_PH	= 0.003f;
        private static float SEBI_CHARGES_PH	= 0.0002f;
        private static float TOLERANCE_PH = 1.0f;
        // Profit --------->>>
        private static float PROFIT_EXPECTATION_PH	= 1.5f;
        public static float PROFIT_EXPECTATION = (PROFIT_EXPECTATION_PH) / (PERCENT_DIVISOR);
        //        // Profit <<< --------
        
        public static float BROKERAGE = (BROKERAGE_PH)/(PERCENT_DIVISOR);
        public static float SECURITY_TRXN_TAX = ((SECURITY_TT_PH) / (PERCENT_DIVISOR));
        public static float TRANSACTION_CHARGES = ((TRANSACTION_CHARGES_PH) / (PERCENT_DIVISOR));
        public static float GST = ((GST_PH) / (PERCENT_DIVISOR));
        public static float STAMP_DUTY = ((STAMP_DUTY_PH) / (PERCENT_DIVISOR));
        public static float SEBI_CHARGES = ((SEBI_CHARGES_PH) / (PERCENT_DIVISOR));

        public static float X_FACTOR = ((BROKERAGE) * (GST)) + ((TRANSACTION_CHARGES) * (GST)) + (TRANSACTION_CHARGES) + (SECURITY_TRXN_TAX) + (BROKERAGE) + (SEBI_CHARGES);
        public static float Y_FACTOR = ((BROKERAGE) * (GST)) + ((TRANSACTION_CHARGES) * (GST)) + (TRANSACTION_CHARGES) + (BROKERAGE) + (SEBI_CHARGES);

        public static float TOLERANCE = (TOLERANCE_PH) / (PERCENT_DIVISOR);

        public static float banker_ceil(float ind)
        {
            int rem = (int)(ind * 100) + 1;

            return (float)((float)(rem) / 100.0f);
        }

        public static float getCurrentTradePrice(string jSonStr)
        {
            //float ft = 0.0f;
            string[] strarray = jSonStr.Split(new[] { "\",\"" }, StringSplitOptions.None);

            //string[] strarray = jSonStr.Split(',');
            //int x = 0;

            string value = "l\":";
            var results = Array.FindAll(strarray, s => s.Contains(value));
            //"l":"106.30"
            string price = Regex.Replace(results[0], @"\,|\""", "");

            string price_st = price.Substring(price.IndexOf(":") + 1, price.Length - price.IndexOf(":") - 1);

            return banker_ceil( float.Parse(price_st) );
        }
        public static float getBrokerage(  float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased )
        {
            float br_on_purchase = 0;
            if ((BROKERAGE * purchasedat * stock_numbers_purchased) > 20.0)
                br_on_purchase = 20;

            float br_on_sale = 0;
            if ((BROKERAGE * soldat_perstock * stock_numbers_purchased) > 20.0)
                br_on_sale = 20;

            if (br_on_purchase < 20.0)
                br_on_purchase = BROKERAGE * purchasedat * stock_numbers_purchased;


            if (br_on_sale < 20.0)
                br_on_sale = BROKERAGE * soldat_perstock * stock_numbers_purchased;

            float total_brokerage = br_on_purchase + br_on_sale;

            //int max_brokerage = (int)actual_brokerage + 1;
            return total_brokerage;
        }


        public static float get_security_Transaction_Charges(float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased)
        {
            float total_soldat = soldat_perstock * stock_numbers_purchased;

            float sec_trxn_charges = SECURITY_TRXN_TAX * total_soldat;
            
            //float max_sec_trxn_charges = decimal.Round(sec_trxn_charges);
            //fix needed need to return max_sec

            return sec_trxn_charges;
        }

        public static float get_total_transaction_charges( float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased )
        {
            return TRANSACTION_CHARGES *( (soldat_perstock + purchasedat)* stock_numbers_purchased );
        }

        public static float get_gst(float brok, float trxn)
        {
            return GST * (brok + trxn);
        }

        public static float get_sebi_charges(float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased)
        {
            //float fixed_sebi_charges_per_crore = 15.0f;

            return SEBI_CHARGES * ((soldat_perstock + purchasedat) * stock_numbers_purchased);
        }

        public static float netProfit(float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased,
                                    float tax )
        {
            return banker_ceil(((soldat_perstock - purchasedat) * stock_numbers_purchased) - tax);
        }

        public static float getBreakEvenPrice(float buy)
        {
            //Formula calculated on board
            // sale_price = Purchase_price * ( ( 1 + Y ) / ( 1 - X ) )
            //X = BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + SECURITY_TRXN_TAX + BROKERAGE + SEBI_CHARGES 
            //Y = BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + BROKERAGE + SEBI_CHARGES 
            float X = X_FACTOR;//BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + SECURITY_TRXN_TAX + BROKERAGE + SEBI_CHARGES;
            float Y = Y_FACTOR;// BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + BROKERAGE + SEBI_CHARGES;

            float break_even_cost = buy * ((1 + Y) / (1 - X));
            return banker_ceil(break_even_cost);
        }

        public static float return_on_investment(float inv, float prof)
        {
            return ((prof / inv) * 100);
        }

        //The 1% percent risk rule is never risking more than 1% of your account on a single trade.
        public static float one_percent_risk_rule(float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased)
        {
            //Set a stop loss point --> loss of income including tax shouldnot go below 1% of total investment in per trade 

            //Calculated on whiteboard that BLP(tolerance loss limit)  is formula -1
            float blp = (purchasedat * ((stock_numbers_purchased * TOLERANCE) - stock_numbers_purchased - Y_FACTOR)) / (X_FACTOR - stock_numbers_purchased);

            //Calculated on whiteboard that BLP(tolerance loss limit)  is formula -2
            float blp2 = (purchasedat * ((stock_numbers_purchased * TOLERANCE) - stock_numbers_purchased + Y_FACTOR)) / (X_FACTOR + stock_numbers_purchased);

            float blp3 = purchasedat * ((1 - Y_FACTOR - TOLERANCE) / (1 + X_FACTOR));

            return blp3;

        }


        public static float get_loss_tolerance_1_pc( float price )
        {
            return price * 0.01f;
        }


        public static float getStampDuty(float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased)
        {
            

            return STAMP_DUTY * ((soldat_perstock + purchasedat) * stock_numbers_purchased);
        }

        public static float getZerodha_Deductions(float purchasedat,
                                    float soldat_perstock,
                                    float stock_numbers_purchased)
        {
            float actual_brokerage = getBrokerage(purchasedat, soldat_perstock, stock_numbers_purchased);

            float sec_trxn_charges = get_security_Transaction_Charges(purchasedat, soldat_perstock, stock_numbers_purchased);

            float trxn_charges = get_total_transaction_charges(purchasedat, soldat_perstock, stock_numbers_purchased);

            float gst = get_gst(actual_brokerage, trxn_charges);

            float sebi_charges = get_sebi_charges(purchasedat, soldat_perstock, stock_numbers_purchased);

            float stamp_duty = getStampDuty(purchasedat, soldat_perstock, stock_numbers_purchased);

            float TT = gst + trxn_charges + sec_trxn_charges + actual_brokerage + sebi_charges + stamp_duty;

            return banker_ceil(TT);
        }

        public static float getStopLossPrice( float price )
        {
            return banker_ceil(price - (price * 0.01f));
        }

        //-------
        public static Tuple<float, float, float, float>generate_statistics(float price)
        {
            //**** already fixed and calculations in Warm_up_time******

            float stop_loss = getStopLossPrice(price);
            float be = getBreakEvenPrice(price);
            float target = be + (price * 0.015f);
            float lpet = be * 1.0001f;

            return Tuple.Create(stop_loss, be, banker_ceil(target), banker_ceil(lpet) );
        }
    }
}
