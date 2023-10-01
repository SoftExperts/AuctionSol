using DAL.Models;
using DAL.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionSol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BidderController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        public BidderController(IUserRepo auctionRepo)
        {
            _userRepo = auctionRepo;
        }

        [HttpGet("List")]
        public Task<List<User>> UserList()
        {
            try
            {
                return _userRepo.GetAllUsers();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                await _userRepo.AddUser(user);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                await _userRepo.UpdateUser(user);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] int Id)
        {
            try
            {
                await _userRepo.DeleteUser(Id);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("GetUserById")]
        public async Task<UserDto> GetUserById([FromBody] int Id)
        {
            try
            {
                var user = await _userRepo.GetById(Id);
                if (user == null)
                {
                    return new UserDto();
                }
                return await SetDto(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        

        private async Task<UserDto> SetDto(User user)
        {
            try
            {
                return new UserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    Phone = user.Phone,
                    UserId = user.UserId,
                    AuctionEndDate = user.Auctions?.EndDate,
                    AuctionName = user.Auctions?.Name,
                    AuctionStartDate = user.Auctions?.StartDate,
                    BidAmount = user.Auctions.ReservedPrice,
                    BidIncrement = user.Auctions.BidIncrement,
                    LastBid = user.Auctions.LastBid,
                    UserType = user.UserType,
                    AuctionId = user.AuctionId,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetUserWithAuction{Id}")]
        public async Task<ActionResult<List<UserDto>>> GetUserWithAuction(int Id)
        {
            try
            {
                var res = await _userRepo.GetUserWithAuction(Id);
              
                var getUserDtos = await SetDto(res);
                
                return Ok(getUserDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("GetBidder")]
        public async Task<ActionResult<UserDto>> GetBidder([FromBody]string Id)
        {
            try
            {
                var res = await _userRepo.GetUserWithAuction(Id);
                if(res != null)
                {
                    var getUserDtos = await SetDto(res);

                    return Ok(getUserDtos);
                }
                else
                {
                    return NotFound();
                }
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<UserDto>> SetDto(List<User> res)
        {
            try
            {
                List<UserDto> users = new List<UserDto>();

                foreach (var item in res) { 
                
                    UserDto userDto = new UserDto();
                    userDto.Id = item.Id;
                    userDto.UserName = item.UserName;
                    userDto.Password = item.Password;
                    userDto.Phone = item.Phone;
                    userDto.UserId = item.UserId;
                    userDto.AuctionName = item.Auctions?.Name;
                    userDto.AuctionStartDate = item.Auctions?.StartDate;
                    userDto.AuctionEndDate = item.Auctions?.EndDate;
                    userDto.BidIncrement = item.Auctions.BidIncrement;
                    userDto.BidAmount = item.Auctions.ReservedPrice;
                    userDto.LastBid = item.Auctions.LastBid;
                    users.Add(userDto);

                }

                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
