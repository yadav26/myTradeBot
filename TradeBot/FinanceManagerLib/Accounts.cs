using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagerLib
{

    
    public class AccountHandler
    {
        private static AccountHandler gAccHandler = null;
        Account gAccount = new Account();
        public AccountHandler ()
        {

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
        public int GetUnitsToBet( int nPriority, float purchase_price )
        {
            
            float funds = gAccount.AvailableFunds;
            int nNum = (int)(funds *(nPriority*(10/100)) / purchase_price);
            return nNum;
        }
    }

    class Account
    {
        public float AvailableFunds { get; set; }
        
    }
}
