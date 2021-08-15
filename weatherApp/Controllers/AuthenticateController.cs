using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weatherApp.Service.Interface;
using weatherApp.Shared.Exceptions;
using weatherApp.Shared.Model;
using weatherApp.Shared.Response;

namespace weatherAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private string generatedToken = null;

        public AuthenticateController(IConfiguration config, ITokenService tokenService, IUserService userService)
        {
            _config = config;
            _tokenService = tokenService;
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login(UserModel userModel)
        {
            var validUser = _userService.GetUser(userModel);

            if (validUser != null)
            {
                generatedToken = _tokenService.BuildToken(_config["JWTAuthentication:JwtKey"].ToString(), _config["JWTAuthentication:JwtIssuer"].ToString(),
                validUser);

                if (generatedToken != null)
                {
                    return new LoginResponse() { Token = generatedToken };
                }
                else
                {
                    throw new ArgumentException($"Token generated failed");
                }
            }
            else
            {
                throw new ExtendedArgumentException($"User not available.", userModel);

            }
        }
    }
}
