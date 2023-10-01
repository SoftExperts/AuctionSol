using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UserDto
    {
        public int? AuctionId { get; set; }
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? UserType { get; set; }
        public string? AuctionName { get; set; }
        public DateTime? AuctionStartDate { get; set; }
        public DateTime? AuctionEndDate { get; set; }
        public int BidIncrement { get; set; }
        public int BidAmount { get; set; }
        public int LastBid { get; set; }
    }
}
