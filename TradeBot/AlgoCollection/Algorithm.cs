﻿using Trading.Model;

namespace AlgoCollection
{
    public interface Algorithm
    {

        int Warm_up_time(UpdateScannerGridObject StockDetails);

        ActiveOrder Execute_Strategy(UpdateScannerGridObject StockDetails, float units);

        SaleOrder Execute_Strategy(ActiveOrder StockDetails );
    }

}
