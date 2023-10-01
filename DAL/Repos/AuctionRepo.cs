using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class AuctionRepo : IAuctionRepo
    {
        private readonly AppDbContext _context;
        public AuctionRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAuction(Auction auction)
        {
            try
            {
                auction.IsDeleted = false;
                await _context.Auctions.AddAsync(auction);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAuction(int auctionId)
        {
            try
            {
                var auction = _context.Auctions
                    .Include(x => x.User)
                    .FirstOrDefault(x => x.Id == auctionId);

                if (auction != null)
                {
                    // Remove all associated users
                    _context.Users.RemoveRange(auction.User);

                    // Remove the auction itself
                    _context.Auctions.Remove(auction);

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception.
                throw ex;
            }
        }


        public async Task<IEnumerable<Auction>> GetAllAucitons()
        {
            try
            {
                var list = _context.Auctions.Where(x=>x.IsDeleted == false).ToList();
               
                if(list.Count > 0)
                {
                    return list;
                }
                else
                { 
                    return Enumerable.Empty<Auction>();
                }
              
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Auction> GetAuctionById(int Id)
        {
            try
            {
                var res = await _context.Auctions.Where(x=>x.Id == Convert.ToInt16(Id)).FirstOrDefaultAsync();
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAuction(Auction auction)
        {
            try
            {
                _context.Auctions.Update(auction);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
