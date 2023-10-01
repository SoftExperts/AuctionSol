using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Security.Policy;
using ViewModels;

namespace AuctionSolFront.Controllers
{
    public class BidderController : Controller
    {
        private readonly HttpClientHelper _httpClientHelper;
        public BidderController(HttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper ?? throw new ArgumentNullException(nameof(httpClientHelper));
        }
        public async Task<IActionResult> List(int? AuctionId)
        {
            try
            {
                string token = Request.Cookies["AuthToken"];

                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }

                string apiEndpoint = $"https://localhost:7007/api/Bidder/List{AuctionId}";

                var obj = await _httpClientHelper.SendRequestAsync<LoginVM>(apiEndpoint, HttpMethod.Get, token);
                if (obj != null)
                {
                    return View("AddBidder", new LoginVM());
                }
                return View("AddBidder", obj);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IActionResult> AddBidder()
        {
            try
            {
                string token = Request.Cookies["AuthToken"];


                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(LoginVM loginVm)
        {
            try
            {
                string token = Request.Cookies["AuthToken"];


                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }
                if (loginVm.Id > 0)
                {
                    string apiEndpoint = "https://localhost:7007/api/Bidder/UpdateUser";

                    loginVm.ModifiedOn = DateTime.Now;

                    string jsonRequestData = JsonConvert.SerializeObject(loginVm);

                    await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                }
                else
                {
                    string apiEndpoint = "https://localhost:7007/api/Bidder/AddUser";
                    
                    loginVm.UserType = "Bidder";
                    loginVm.Status = true;
                    loginVm.CreatedOn = DateTime.Now;
                    loginVm.IsDeleted = false;

                    string jsonRequestData = JsonConvert.SerializeObject(loginVm);

                    await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                    // Get Auction list with Bidders.
                    string GetUserWithAuciton = $"https://localhost:7007/api/Bidder/GetUserWithAuction{loginVm.AuctionId}";

                    string jsonRequestDataq = JsonConvert.SerializeObject(loginVm);

                    var obj = await _httpClientHelper.SendRequestAsync<List<LoginVM>>(GetUserWithAuciton, HttpMethod.Get, token);
                    // Return a partial view containing the updated table
                    return PartialView("_BidPartial", obj);
                }
                return Unauthorized();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult>GetBidders(LoginVM loginVm)
        {
            try
            {
                string token = Request.Cookies["AuthToken"];

                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }
                // Get Auction list with Bidders.
                string GetUserWithAuciton = $"https://localhost:7007/api/Bidder/GetUserWithAuction{loginVm.AuctionId}";

                string jsonRequestDataq = JsonConvert.SerializeObject(loginVm);

                var obj = await _httpClientHelper.SendRequestAsync<List<LoginVM>>(GetUserWithAuciton, HttpMethod.Get, token);
                // Return a partial view containing the updated table
                return PartialView("_BidPartial", obj);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                string token = Request.Cookies["AuthToken"];


                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }

                string apiEndpoint = "https://localhost:7007/api/Bidder/GetUserById";

                string jsonRequestData = JsonConvert.SerializeObject(Id);

                var obj = await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                var result = await obj.Content.ReadAsStringAsync();

                var auction = JsonConvert.DeserializeObject<LoginVM>(result);

                return View("AddBidder", auction);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                string token = Request.Cookies["AuthToken"];


                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }

                string apiEndpoint = "https://localhost:7007/api/Bidder/DeleteUser";

                string jsonRequestData = JsonConvert.SerializeObject(Id);

                await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                return RedirectToAction("List","Admin");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBid(int auctionId, int newBidAmount, string UserId)
        {
            string token = Request.Cookies["AuthToken"];

            AuctionVM auction = new AuctionVM();

            auction.Id = auctionId;
            auction.LastBid = newBidAmount;
            auction.UserId = UserId;

            string apiEndpoint = "https://localhost:7007/api/Admin/UpdateAuctionById";

            string jsonRequestData = JsonConvert.SerializeObject(auction);

            var res = await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);
            if(res.IsSuccessStatusCode)
            {
                var response = new
                {
                    success = true,
                    message = "Bid updated successfully",
                    newBidAmount = newBidAmount
                };

                return Json(response);
            }
            else
            {
                //return RedirectToAction("Login","Login");
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestBidInfo()
        {
            string token = Request.Cookies["AuthToken"];

            string userId = Request.Cookies["userId"];
            if (token == null && userId == null)
            {
                return Unauthorized();
            }
            string Url = "https://localhost:7007/api/Bidder/GetBidder";

            string UserId = JsonConvert.SerializeObject(userId);

            var resp = await _httpClientHelper.SendRequestAsync<LoginVM>(Url, HttpMethod.Get, token, UserId);

            return Json(new { data = resp});
        }
    }
}
