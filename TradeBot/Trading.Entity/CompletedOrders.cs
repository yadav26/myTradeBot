using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Model;

namespace Trading.Entity
{
    public class CompletedOrders
    {
        public string Ticker { get; set; }

        public ActiveOrder OrderDetails;
        public CompletedOrders(ActiveOrder ao)
        {
            OrderDetails = ao;
            Ticker = ao.Ticker;
        }

    }

}
