using Admin_Data.DataContext;
using Admin_Data.Models;
using Admin_Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Pump_Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ILogger = NLog.ILogger;

namespace Admin_Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        
        private readonly AdminDBContext _dbContext;

        private readonly JwtSettings _jwtSettings;

        public UserController(AdminDBContext dbContext,IOptions<JwtSettings> options)
        {
            this._dbContext = dbContext;
            this._jwtSettings = options.Value;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AdminLogin adminLogin)
        {
            var user= await this._dbContext.tblSecure.FirstOrDefaultAsync(item => item.tblUsername == adminLogin.tblUsername && item.tblPassword == adminLogin.tblPassword);
            if (user == null){

                return Unauthorized();

            }
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(this._jwtSettings.securitykey);
            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Name, user.tblUsername) }),
                Expires = DateTime.Now.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokendesc);
            string finaltoken= tokenhandler.WriteToken(token);

            return Ok(finaltoken);
 

       
        }
    }
}

      