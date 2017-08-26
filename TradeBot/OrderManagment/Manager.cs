using AlgoCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Entity;
using Trading.Model;

namespace OrderManagment
{
    public class Manager
    {
        public List<OrderDatum> List_OrderCollection = new List<OrderDatum>();

        Manager( UpdateScannerGridObject scobj )
        {
            OrderDatum order = new OrderDatum(scobj);

            List_OrderCollection.Add(order);
        }

    }
}
