using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagerLib
{

    
    public class AccountHandler
    {
        private Object thisLock = new Object();
        private static AccountHandler gAccHandler = null;
        private Account gAccount = new Account();
        public AccountHandler ()
        {
            gAccount.AvailableFunds = 100000;
        }
        public static AccountHandler GetHandlerObject()
        {
            if (null == gAccHandler)
            {
                gAccHandler = new AccountHandler();
                return gAccHandler;
            }

            return gAccHandler;

        }
        public float GetUnitsToBet( int nPriority, float purchase_price )
        {
            lock (thisLock)
            {
                if (gAccount.AvailableFunds <= 0)
                    return 0;

                float multplier = gAccount.AvailableFunds * (nPriority * (0.01f));

                float  nNum = multplier / purchase_price;

                gAccount.AvailableFunds = gAccount.AvailableFunds - (float)(purchase_price * nNum);

                return nNum;
            }
           
        }
    }

    class Account
    {
        public float AvailableFunds { get; set; }
        
    }
}
