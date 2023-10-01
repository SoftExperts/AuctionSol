using DAL.Models;

namespace DAL.Repos
{
    public interface IAuctionRepo
    {
        Task AddAuction(Auction auction);
        Task DeleteAuction(int Id);
        Task UpdateAuction(Auction auction);
        Task<Auction> GetAuctionById(int Id);
        Task<IEnumerable<Auction>> GetAllAucitons();
    }
}
