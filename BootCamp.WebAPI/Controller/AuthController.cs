using BootCamp.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BootCamp.WebAPI.Controller
{
    [Route("v1/api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] Model.User loginParams)
        {
            var user = Dal.UserDAL.Get(loginParams.UserName, loginParams.Password);
            if(user == null)
            {
                return NotFound(new { message = "Usuario senha invalida" });
            }
            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }
    }
}
