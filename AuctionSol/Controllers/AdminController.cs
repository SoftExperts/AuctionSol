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
    public class AdminController : ControllerBase
    {
        private readonly IAuctionRepo _auctionRepo;
        private readonly IUserRepo _userRepo;
        public AdminController(IAuctionRepo auctionRepo, IUserRepo userRepo) 
        {
            _userRepo = userRepo;
            _auctionRepo = auctionRepo;
        }

        [HttpGet("List")]
        public Task<IEnumerable<Auction>> AuctionList()
        {
            try
            {
                return _auctionRepo.GetAllAucitons();
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpPost("AddAuction")]
        public async Task<IActionResult> AddAuction([FromBody] Auction auction)
        {
            try
            {
                await _auctionRepo.AddAuction(auction);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpPost("UpdateAuction")]
        public async Task<IActionResult> UpdateAuction([FromBody] Auction auction)
        {
            try
            {
                await _auctionRepo.UpdateAuction(auction);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpPost("UpdateAuctionById")]
        public async Task<IActionResult> UpdateAuctionById([FromBody] Auction auction)
        {
            try
            {
                
                var getAuction = await _auctionRepo.GetAuctionById(Convert.ToInt32((auction.Id)));
                var getUser = await _userRepo.GetUserById(auction.UserId);
                if(getUser != null)
                {
                    getAuction.LastBid = auction.LastBid;
                    getAuction.UserId = auction.UserId;
                    getAuction.ModifiedOn = DateTime.Now;

                    await _auctionRepo.UpdateAuction(getAuction);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
               
              
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        
        [HttpPost("DeleteAuction")]
        public async Task<IActionResult> DeleteAuction([FromBody] int Id)
        {
            try
            {
                await _auctionRepo.DeleteAuction(Id);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpPost("GetAuctionById")]
        public async Task<Auction> GetAuctionById([FromBody] int Id)
        {
            try
            {
                return await _auctionRepo.GetAuctionById(Id);                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
