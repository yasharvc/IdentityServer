using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tribit.IdentityServer.Shared.Entities;
using Tribit.IdentityServer.Shared.Extensions;

namespace Tribit.IdentityServer.App.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        public AccountController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
        private IEnumerable<ApplicationUser> logins = new List<ApplicationUser>() {
            new ApplicationUser() {
                    Id = Guid.NewGuid().ToString(),
                        Email = "a@gmail.com",
                        UserName = "Admin",
                        PasswordHash = "Admin",
                },
                new ApplicationUser() {
                    Id = Guid.NewGuid().ToString(),
                        Email = "adminakp@gmail.com",
                        UserName = "User1",
                        PasswordHash = "Admin",
                }
        };
        [HttpGet("~/test")]
        public IActionResult Test()
		{
            return Content("Test");
		}
        [HttpPost]
        public IActionResult GetToken([FromBody]ApplicationUser userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var Valid = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                if (Valid)
                {
                    var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                    Token = JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.Email,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = new Guid(user.Id),
                    }, jwtSettings);
                }
                else
                {
                    return BadRequest($"wrong password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Get List of UserAccounts
        /// </summary>
        /// <returns>List Of UserAccounts</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            return Ok(logins);
        }
    }
}
