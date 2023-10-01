using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using ViewModels;

namespace AuctionSolFront.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClientHelper _httpClientHelper;
        public LoginController(HttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper ?? throw new ArgumentNullException(nameof(httpClientHelper));
        }

        [HttpGet]
        public IActionResult Login()
        {
            try
            {

                return View(new LoginVM());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> BidderDashboard()
        {
            string token = Request.Cookies["AuthToken"];

            string userId = Request.Cookies["userId"];
            if(token == null && userId == null)
            {
                return Unauthorized();
            }
            string Url = "https://localhost:7007/api/Bidder/GetBidder";

            string UserId = JsonConvert.SerializeObject(userId);

            var resp = await _httpClientHelper.SendRequestAsync<LoginVM>(Url, HttpMethod.Get, token, UserId);

            return View(resp);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginObj)
        {
            try
            {
                if (loginObj != null)
                {
                    // Define the API endpoint URL where you want to request the token
                    string apiEndpoint = "https://localhost:7007/api/login";

                    string jsonRequestData = JsonConvert.SerializeObject(loginObj);

                    var response = await _httpClientHelper.SendRequestAsync(apiEndpoint,jsonRequestData, HttpMethod.Post);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                         var res = JsonConvert.DeserializeObject<LoginResponse>(data);

                        Response.Cookies.Append("AuthToken", res.Token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTime.Now.AddMinutes(30) 
                        });
                        
                        Response.Cookies.Append("UserId", res.UserId, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTime.Now.AddMinutes(30) 
                        });

                        if (res.GroupType.Equals("Bidder"))
                        {
                              
                            return RedirectToAction("BidderDashboard");
                        }
                        return RedirectToAction("List", "Admin"); // Return the token as part of the response
                    }
                    else
                    {
                        // Handle error responses here
                        return Unauthorized("User is not more existing....");
                    }
                }
                else
                {
                    return Unauthorized("Invalid Request");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("AuthToken");

                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {

                throw;
            }
         
        }
        public class LoginResponse
        {
            public string? GroupType { get; set; }
            public string? Token { get; set; }
            public string? UserId { get; set; }
            public string? UserName { get; set; }
        }
    }
}
