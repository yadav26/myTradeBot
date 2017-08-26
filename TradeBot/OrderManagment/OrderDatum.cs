using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Entity;
using Trading.Model;

namespace OrderManagment
{
    public class OrderDatum
    {
        public int OrderID { get; set; }
        public bool bIsOrderSold = false;
        public bool bIsOrderPurchased = false;

        private PurchaseDetails m_PurchaseDetails = null;
        private SaleDetails m_SaleDetails = null;

        public OrderDatum(UpdateScannerGridObject scobj)
        {
            if(false == bIsOrderPurchased)
            {
                m_PurchaseDetails = new PurchaseDetails();
                bIsOrderPurchased = true;

            }

            if( true == bIsOrderPurchased) // only sale when we have purchased it. but conditions ?? not here
            {
                m_SaleDetails = new SaleDetails();
                bIsOrderSold = true;
            }
        }
    }
}
