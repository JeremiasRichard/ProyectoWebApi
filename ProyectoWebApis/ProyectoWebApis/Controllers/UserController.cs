using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Services;

namespace ProyectoWebApis.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentials userCredentials)
        {
            var result = await _userService.RegisterUser(userCredentials);

            if (result != null)
            {
                return result;
            }
            else
            {
                return BadRequest("Error registering user");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentials userCredentials)
        {
            var result = await _userService.Login(userCredentials);

            if (result != null)
            {
                return result;
            }
            else
            {
                return BadRequest("Invalid login!");
            }
        }

        [HttpGet("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthenticationResponse>> Renew()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var userCredentials = new UserCredentials()
            {
                Email = email,
            };
            return await _userService.RenewToken(userCredentials);
        }


    }
}
