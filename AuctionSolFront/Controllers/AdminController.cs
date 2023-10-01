using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;
using ViewModels;

namespace AuctionSolFront.Controllers
{

    public class AdminController : Controller
    {
        private readonly HttpClientHelper _httpClientHelper;
        public AdminController(HttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper ?? throw new ArgumentNullException(nameof(httpClientHelper));
        }
        public async Task<IActionResult> List()
        {
            try
            {
                string token = Request.Cookies["AuthToken"];

                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }

                string apiEndpoint = "https://localhost:7007/api/Admin/List";
                
                var list = await _httpClientHelper.SendRequestAsync<List<AuctionVM>>(apiEndpoint, HttpMethod.Get, token);
                if(list.Count == 0)
                {
                    return View(new List<AuctionVM>()); 
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    
        public async Task<IActionResult> AddAuction()
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
        public async Task<IActionResult> Save(AuctionVM auction)
        {
            try
            {
                string token = Request.Cookies["AuthToken"];


                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }
                if (auction.Id > 0)
                {
                    string apiEndpoint = "https://localhost:7007/api/Admin/UpdateAuction";

                    string jsonRequestData = JsonConvert.SerializeObject(auction);

                    await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                }
                else
                {
                    string apiEndpoint = "https://localhost:7007/api/Admin/AddAuction";

                    string jsonRequestData = JsonConvert.SerializeObject(auction);

                    await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                }

                return RedirectToAction("List");
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

                string apiEndpoint = "https://localhost:7007/api/Admin/GetAuctionById";

                string jsonRequestData = JsonConvert.SerializeObject(Id);

                var obj = await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                var result =await obj.Content.ReadAsStringAsync();
                
                var auction = JsonConvert.DeserializeObject<AuctionVM>(result);

                return View("AddAuction",auction);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> Delete (int Id)
        {
            try
            {
                string token = Request.Cookies["AuthToken"];


                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }

                string apiEndpoint = "https://localhost:7007/api/Admin/DeleteAuction";

                string jsonRequestData = JsonConvert.SerializeObject(Id);

                await _httpClientHelper.SendRequestAsync(apiEndpoint, jsonRequestData, HttpMethod.Post, token);

                return RedirectToAction("List");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                string token = Request.Cookies["AuthToken"];

                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case where the token is not present
                    return Unauthorized();
                }

                string apiEndpoint = "https://localhost:7007/api/Admin/List";

                var list = await _httpClientHelper.SendRequestAsync<List<AuctionVM>>(apiEndpoint, HttpMethod.Get, token);
                
                
                if (list.Count == 0)
                {
                    return View(new LoginVM());
                }
                else
                {
                    var getBidder = SetBidd(list.FirstOrDefault());
                    return View(getBidder);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private LoginVM SetBidd(AuctionVM? auctionVM)
        {
            try
            {
                LoginVM login = new LoginVM();

                login.AuctionStartDate = auctionVM.StartDate;
                login.AuctionEndDate = auctionVM.EndDate;
                login.UserId = auctionVM.UserId;
                login.BidIncrement = auctionVM.BidIncrement;
                login.LastBid = auctionVM.LastBid;
                login.AuctionName = auctionVM.Name;
                login.BidAmount = auctionVM.ReservedPrice;
                login.UserId = auctionVM.UserId;
                return login;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
