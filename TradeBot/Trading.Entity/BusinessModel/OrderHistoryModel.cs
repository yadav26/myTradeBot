
namespace Trading.Entity.BusinessModel
{
    public class OrderHistoryModel
    {
        public int OrderID { get; set; }
        public string CodeName { get; set; }
        public decimal TodayLeast { get; set; }
        public decimal TodayMaxim { get; set; }
        public decimal TodayMean { get; set; }
        public decimal BuyingWindowL { get; set; }
        public decimal BuyingWindowU { get; set; }
        public decimal HistoryLeast { get; set; }
        public decimal HistoryMax { get; set; }
        public decimal HistoryMean { get; set; }
        public decimal PurcasedAt { get; set; }
        public string Stratergy { get; set; }
        public int Quantity { get; set; }
        public decimal StopLoss { get; set; }
        public decimal BreakEven { get; set; }
        public decimal LPET { get; set; }
        public decimal ProfitTarget { get; set; }
    }
}
