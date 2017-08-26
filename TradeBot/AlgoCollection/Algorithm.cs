using Trading.Model;

namespace AlgoCollection
{
    public interface Algorithm
    {

        int Warm_up_time(UpdateScannerGridObject StockDetails);

        ActiveOrder Execute_Strategy(UpdateScannerGridObject StockDetails, int units);
        //ActiveOrder Execute_Strategy();


    }
}
