using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User? user)
        {
            try
            {
                if(user!= null)
                {
                    user.IsDeleted = false;
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteUser(int? id)
        {
            try
            {
                var getUser = await _context.Users.FindAsync(id);
                if (getUser != null)
                {
                    _context.Remove(getUser);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return _context.Users.Where(x=>x.IsDeleted == false).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<User>> GetUserWithAuction(int auctionId)
        {
            try
            {
                var usersWithAuction = await _context.Users
                    .Where(user => user.AuctionId == auctionId && user.IsDeleted == false)
                    .Include(user => user.Auctions).Where(x=>x.IsDeleted==false) // Include the associated Auction
                    .ToListAsync();

                return usersWithAuction;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        
        public async Task<User> GetUserWithAuction(string userId)
        {
            try
            {
                var usersWithAuction = await _context.Users
                    .Where(user => user.UserId == userId && user.IsDeleted == false)
                    .Include(user => user.Auctions).Where(x => x.IsDeleted == false) // Include the associated Auction
                    .ToListAsync();

                return usersWithAuction.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<User> GetById(int? Id)
        {
            try
            {
                return await _context.Users.Where(x => x.Id == Id && x.Status == true && x.IsDeleted == false).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<User> GetUserById(string? UserId)
        {
            try
            {
                return await _context.Users.Where(x => x.UserId == UserId && x.Status == true && x.IsDeleted == false).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateUser(User? user)
        {
            try
            {
                 _context.Users.Update(user);
                 _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
