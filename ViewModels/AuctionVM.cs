namespace ViewModels
{
    public class AuctionVM : BaseVM
    {
        public string? Name { get; set; }
        public string? VisibleRemarks { get; set; }
        public string? HiddenRemarks { get; set; }
        public string? FlatName { get; set; }
        public string? Floor { get; set; }
        public string? TowerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ReservedPrice { get; set; }
        public int BidIncrement { get; set; }
        public int LastBid { get; set; }
        public string? Address { get; set; }
        public string? UserId { get; set; }
    }
}
