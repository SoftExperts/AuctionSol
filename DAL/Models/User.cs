using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class User : BaseEntity
    {
        [ForeignKey("Auction")]
        public int? AuctionId { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? UserType { get; set; }

        public Auction? Auctions { get; set; }
    }
}
