using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Entity;

namespace Core0.library
{
    public interface Algorithm
    {

        int Warm_up_time(UpdateScannerGridObject StockDetails);

        ActiveOrder Execute_Strategy(UpdateScannerGridObject StockDetails, int units);


    }
}
