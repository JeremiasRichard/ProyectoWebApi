using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Models;
using ProyectoWebApis.Services;
using System.Security.Claims;

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
        public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentialsCreate userCredentials, [FromQuery] double balance)
        {
            var result = await _userService.RegisterUser(userCredentials,balance);
            if (result != null)
            {
                return result;
            }
            else
            {
                return BadRequest("Error while registering user");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentialsCreate userCredentials)
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
            var userCredentialsCreate = new UserCredentialsCreate()
            {
                Email = email,
            };
            return await _userService.RenewToken(userCredentialsCreate);
        }

        [HttpPost("GetAcces")]
        public async Task<ActionResult> MakeAdmin(UserPermissionsCredentials userDTO)
        {
            var result = await _userService.MakeAdmin(userDTO);
            if(result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
