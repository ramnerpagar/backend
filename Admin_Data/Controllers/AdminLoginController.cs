using Admin_Data.DataContext;
using Admin_Data.Models;
using Admin_Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Pump_Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ILogger = NLog.ILogger;

namespace Admin_Data.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AdminLoginController : Controller
    {
        AdminLoginService adminLoginService;

        private readonly AdminDBContext _dbContext;

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly JwtSettings _jwtSettings;

        public AdminLoginController(AdminDBContext dbContext, IOptions<JwtSettings> options)
        {
            this._dbContext = dbContext;
            this._jwtSettings = options.Value;
            adminLoginService = new AdminLoginService(dbContext);
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AdminLogin adminLogin)
        {
            //var user = await this._dbContext.tblSecure.FirstOrDefaultAsync(item => item.tblUsername == adminLogin.tblUsername && item.tblPassword == adminLogin.tblPassword);
            JsonResponse jsonResponse = new JsonResponse();
            bool status = adminLoginService.AdminLogin(adminLogin);
            if (status)
            {
                jsonResponse.Message = TokenManager.GenerateToken("admin");
            }
            

            //if (user == null)
            //{

            //    return Unauthorized();

            //}
            //var tokenhandler = new JwtSecurityTokenHandler();
            //var tokenkey = Encoding.UTF8.GetBytes(this._jwtSettings.securitykey);
            //var tokendesc = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(
            //        new Claim[] { new Claim(ClaimTypes.Name, user.tblUsername) }),
            //    Expires = DateTime.Now.AddSeconds(20),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            //};
            //var token = tokenhandler.CreateToken(tokendesc);
            //string finaltoken = tokenhandler.WriteToken(token);

     

            return Ok("success");
        }

          
        [HttpPost]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLogin adminLogin)
        {
            try
            {
                var user = await this._dbContext.tblSecure.FirstOrDefaultAsync(item => item.tblUsername == adminLogin.tblUsername && item.tblPassword == adminLogin.tblPassword);
                bool status = adminLoginService.AdminLogin(adminLogin);
                JsonResponse jsonResponse = new JsonResponse(); 
                if (status)
                {
                    jsonResponse.Result = true;
                    //jsonResponse.Message = "Login Success";
                    jsonResponse.Message = TokenManager.GenerateToken("admin");

                }
                else
                {
                    jsonResponse.Result = false;
                    jsonResponse.Message = "Login failed";
                }
                logger.Info("sucess");
                return Ok(jsonResponse);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

    }
}
