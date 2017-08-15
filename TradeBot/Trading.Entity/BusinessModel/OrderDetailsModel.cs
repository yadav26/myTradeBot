using System;
using System.Collections.Generic;

namespace Trading.Model.BusinessModel
{
    public class OrderDetailsModel
    {
        public int OrderID { get; set; }
        public string TickerSymbol { get; set; }
        public decimal LatestPrice { get; set; }
        public decimal DayGain { get; set; }
        public decimal BoughtPrice { get; set; }
        public int Units { get; set; }
        public DateTime PurchaceDate { get; set; }
        public List<OrderHistoryModel> OrderHistory { get; set; }
    }
}
