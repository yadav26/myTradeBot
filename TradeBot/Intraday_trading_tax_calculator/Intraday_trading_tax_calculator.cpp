// Intraday_trading_tax_calculator.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include "ProfitExpectation.h"
#include "Inputs.h"
#include "NSE_Listed_Stickers.h"

using namespace std;

float purchasedat = 100;
float soldat_perstock = 110;
int stock_numbers_purchased = 100;
float total_turn_over = (purchasedat + soldat_perstock) * stock_numbers_purchased;

float getBrokerage() 
{
	float br_on_purchase=0;
	if ((BROKERAGE * purchasedat*stock_numbers_purchased) > 20.0 )
		br_on_purchase = 20;

	float br_on_sale=0;
	if ((BROKERAGE * soldat_perstock*stock_numbers_purchased) > 20.0)
		br_on_sale = 20;

	if(br_on_purchase < 20.0)
		br_on_purchase  = BROKERAGE * purchasedat*stock_numbers_purchased;


	if (br_on_sale < 20.0)
		br_on_sale = BROKERAGE * soldat_perstock*stock_numbers_purchased;

	float total_brokerage = br_on_purchase + br_on_sale;



	//int max_brokerage = (int)actual_brokerage + 1;
	return total_brokerage;
}

float get_security_Transaction_Charges()
{
	float total_soldat = soldat_perstock * stock_numbers_purchased;

	float sec_trxn_charges = SECURITY_TRXN_TAX * total_soldat;

	float max_sec_trxn_charges = ceil(sec_trxn_charges);

	return max_sec_trxn_charges;
}

float get_total_transaction_charges()
{
	return TRANSACTION_CHARGES * total_turn_over;
}

float get_gst(float brok, float trxn)
{
	return GST * (brok + trxn);
}

float get_sebi_charges()
{
	float fixed_sebi_charges_per_crore = 15.0;
	return SEBI_CHARGES * total_turn_over;;
}

float netProfit(float tax)
{
	return ((soldat_perstock - purchasedat ) * stock_numbers_purchased) - tax;
}

float break_even_calculator( float buy )
{
	//Formula calculated on board
	// sale_price = Purchase_price * ( ( 1 + Y ) / ( 1 - X ) )
	//X = BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + SECURITY_TRXN_TAX + BROKERAGE + SEBI_CHARGES 
	//Y = BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + BROKERAGE + SEBI_CHARGES 
	float X = X_FACTOR;//BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + SECURITY_TRXN_TAX + BROKERAGE + SEBI_CHARGES;
	float Y = Y_FACTOR;// BROKERAGE* GST + TRANSACTION_CHARGES*GST + TRANSACTION_CHARGES + BROKERAGE + SEBI_CHARGES;

	float break_even_cost = buy *((1 + Y) / (1 - X));
	return break_even_cost;
}

float return_on_investment( float inv, float prof )
{
	return ((prof / inv) * 100);
}

//The 1% percent risk rule is never risking more than 1% of your account on a single trade.
float one_percent_risk_rule( ) 
{
	//Set a stop loss point --> loss of income including tax shouldnot go below 1% of total investment in per trade 
	
	//Calculated on board that BLP(tolerance loss limit)  is formula -1
	float blp = (purchasedat *((stock_numbers_purchased* TOLERANCE) - stock_numbers_purchased - Y_FACTOR) ) / (X_FACTOR - stock_numbers_purchased ) ;

	//Calculated on board that BLP(tolerance loss limit)  is formula -2
	float blp2 = (purchasedat *((stock_numbers_purchased* TOLERANCE) - stock_numbers_purchased + Y_FACTOR)) / (X_FACTOR + stock_numbers_purchased);

	float blp3 = purchasedat * ((1- Y_FACTOR - TOLERANCE) / (1+X_FACTOR));

	return blp3;

}
float getStampDuty()
{
	return STAMP_DUTY * total_turn_over;
}
float getZerodha_Deductions() 
{
	float actual_brokerage = getBrokerage();

	float sec_trxn_charges = get_security_Transaction_Charges();

	float trxn_charges = get_total_transaction_charges();

	float gst = get_gst(actual_brokerage, trxn_charges);

	float sebi_charges = get_sebi_charges();

	float stamp_duty = getStampDuty();

	float TT = gst + trxn_charges + sec_trxn_charges + actual_brokerage + sebi_charges + stamp_duty ;

	return TT;
}

int main()
{
	
	//cout.precision(2);

	float profit_before_tax = (soldat_perstock - purchasedat) * stock_numbers_purchased;
	
	float actual_brokerage = getBrokerage();
	
	float sec_trxn_charges = get_security_Transaction_Charges();

	float trxn_charges = get_total_transaction_charges();


	float gst = get_gst(actual_brokerage, trxn_charges);


		
	float sebi_charges = get_sebi_charges();



	float stamp_charges = getStampDuty();// STAMP_DUTY * total_turn_over;



	float TT = gst + trxn_charges + sec_trxn_charges + actual_brokerage + sebi_charges;
	
	//cout << endl << "Total Tax : " << TT << endl;

	float zerodha_deduc = getZerodha_Deductions();// stamp_charges + TT;

	//cout << "Zerodha Deduction : " << zerodha_deduc <<endl;

	float bep = break_even_calculator(purchasedat);

	

	//cout <<endl << "BEP : " << bep << " -> for trade [N = " << stock_numbers_purchased << "], [Bought at = " << purchasedat <<"]" << endl;
	
	float money_invested = purchasedat * stock_numbers_purchased;
	float nettprofit = netProfit(zerodha_deduc);
	
	
	cout << "Summary : \n";
	cout <<  "Purchased at = " << purchasedat 
		<<endl << "Number of Units = " << stock_numbers_purchased
		<<endl << "Money Invested = " << money_invested
		<< endl << "BEP = " << bep
		<< endl << "Sold at = " << soldat_perstock
		<< endl << "TAX/Deductions = " << zerodha_deduc;
	cout << endl << "\tBrokerage : " << actual_brokerage ;
	cout << endl << "\tSTT total : " << sec_trxn_charges ;
	cout << endl << "\tTotal Txn charge : " << trxn_charges ;
	cout << endl << "\tGST : " << gst ;
	cout << endl << "\tSEBI charges : " << sebi_charges ;
	cout << endl << "\tStamp_charges : " << stamp_charges 


		<< endl << "Profit = " << nettprofit
		<<endl << "ROI = " << return_on_investment(money_invested, nettprofit) << "%"
		<< endl << "Loss Tolerance ("<< TOLERANCE_PH <<"%) price = " << one_percent_risk_rule()

	<<endl << "Profit expectation ("<< PROFIT_EXPECTATION_PH <<"%) price : " << set_profit_expectation ()<< endl;

	cout << endl << endl;


	//cout << "list all NSE ---->\n";
	//int ser = 0;
	//for (int x= 0;  x< sizeof(list_of_nse)/sizeof(list_of_nse[0]) ; x++ )
	//	cout << ++ser << ".\t" <<list_of_nse[x] << "\t" << list_of_nse[x++] << endl;
	return 0;
}

