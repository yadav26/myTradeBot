
#include "ProfitExpectation.h"
#include "stdafx.h"
#include "Inputs.h"

extern float purchasedat ;
extern int stock_numbers_purchased ;
extern float break_even_calculator(float buy);

float set_profit_expectation()
{
	float bep = break_even_calculator(purchasedat); 

	float target_sell_price = PROFIT_EXPECTATION * purchasedat + bep;

	return target_sell_price;
}

