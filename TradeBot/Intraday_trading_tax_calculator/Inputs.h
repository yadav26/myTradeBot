#pragma once

#define BROKERAGE_PH	((float)(0.01))
#define SECURITY_TT_PH	((float)(0.025))
#define TRANSACTION_CHARGES_PH ((float)(0.00325))
#define GST_PH ((float)(18.0))
#define PERCENT_DIVISOR ((float)(100.0))
#define STAMP_DUTY_PH	((float)(0.003))
#define SEBI_CHARGES_PH	((float)(0.0002))

// Profit --------->>>
#define PROFIT_EXPECTATION_PH	((float)(1.5))
#define PROFIT_EXPECTATION ((float)((PROFIT_EXPECTATION_PH)/(PERCENT_DIVISOR)))
// Profit <<< --------

#define BROKERAGE ((float)((BROKERAGE_PH)/(PERCENT_DIVISOR)))
#define SECURITY_TRXN_TAX  (float(((SECURITY_TT_PH)/(PERCENT_DIVISOR))))
#define TRANSACTION_CHARGES (float(((TRANSACTION_CHARGES_PH)/(PERCENT_DIVISOR))))
#define GST (float(((GST_PH)/(PERCENT_DIVISOR))))
#define STAMP_DUTY (float(((STAMP_DUTY_PH)/(PERCENT_DIVISOR))))
#define SEBI_CHARGES (float(((SEBI_CHARGES_PH)/(PERCENT_DIVISOR))))



#define X_FACTOR ((float)(((BROKERAGE)*(GST)) + ((TRANSACTION_CHARGES)*(GST)) + (TRANSACTION_CHARGES) + (SECURITY_TRXN_TAX) + (BROKERAGE) + (SEBI_CHARGES)))
#define Y_FACTOR ((float)(((BROKERAGE)*(GST)) + ((TRANSACTION_CHARGES)*(GST)) + (TRANSACTION_CHARGES) + (BROKERAGE) + (SEBI_CHARGES)))

#define TOLERANCE_PH ((float)(1.0))
#define TOLERANCE ((float)((TOLERANCE_PH)/(PERCENT_DIVISOR)))