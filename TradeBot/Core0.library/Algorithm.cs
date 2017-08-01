using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    public interface Algorithm
    {

        int Warm_up_time(string exch, string ticker, string sd, string ed);

        float Execute_Strategy(Func<CurrentOrderUpdater, int> func1, CurrentOrderUpdater objCurrentStatus, float fetched_price, int units);

    }
}
