using DAL.Models;

namespace DAL.Repos
{
    public interface IUserRepo
    {
        Task AddUser(User? user);
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetUserWithAuction(int Id);
        Task<User> GetUserWithAuction(string userId);
        Task DeleteUser(int? id);
        Task<User> GetById(int? id);
        Task<User> GetUserById(string? UserId);
        Task UpdateUser(User? user);
    }
}
