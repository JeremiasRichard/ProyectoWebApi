﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoWebApis.DataBase;
using ProyectoWebApis.DTOs;
using ProyectoWebApis.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProyectoWebApis.Services
{
    public class UserService
    {

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly IDataProtector _dataProtector;
       
        public UserService(UserManager<User> userManager, IConfiguration configuration, SignInManager<User> signInManager, IDataProtectionProvider dataProtectionProvider)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _dataProtector = dataProtectionProvider.CreateProtector("testingProtectorProvider");
           
        }

        public async Task<AuthenticationResponse> RegisterUser(UserCredentialsCreate userCredentials, double balance)
        {
            var user = new User { UserName = userCredentials.Email, Email = userCredentials.Email, Status = true, Balance = balance};

            var result = await _userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {

                return await BuildToken(userCredentials);
            }
            else
            {
                return null;
            }
        }

        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentialsCreate userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email,
               userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(userCredentials);
            }
            else
            {
                return null;
            }
        }

        public async Task<AuthenticationResponse> RenewToken(UserCredentialsCreate userCredentials)
        {
            return await BuildToken(userCredentials);
        }

        private async Task<AuthenticationResponse> BuildToken(UserCredentialsCreate userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",userCredentials.Email),
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);

            var claimsDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyJwt"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(20);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims
                , expires: expiration, signingCredentials: credentials);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration,
                UserId = user.Id
            };
        }

        public async Task<bool> MakeAdmin(UserPermissionsCredentials userDTO)
        {

            if(userDTO.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(userDTO.Email);
                await _userManager.AddClaimAsync(user, new Claim("isAdmin", "1"));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
