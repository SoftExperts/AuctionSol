using DAL.Models;
using DAL.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuctionSol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private IConfiguration _config;

        public LoginController(IUserRepo userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }


        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Post([FromBody] UserDto loginRequest)
        {
            try
            {
                if (loginRequest != null)
                {
                    var userExist = await _userRepo.GetUserById(loginRequest.UserId);

                    if (userExist != null)
                    {
                        if (userExist.UserId.ToLower() == loginRequest.UserId.ToLower()
                            && userExist.Password == loginRequest.Password)
                        {
                           LoginResponse response = new LoginResponse();

                            response.Token = await GetJwtToken();
                            response.GroupType = userExist.UserType;
                            response.UserName = userExist.UserName;
                            response.UserId = userExist.UserId;

                            return Ok(response);
                        }
                        else
                        {
                            return BadRequest("Invalid UserName or Password!");
                        }
                    }
                }
                return BadRequest("Invalid Request");
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task<string?> GetJwtToken()
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  null,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                return token;
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
