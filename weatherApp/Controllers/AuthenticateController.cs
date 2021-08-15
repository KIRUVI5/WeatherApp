using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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

        /// <summary>
        /// This api endpoint used to authenticate the user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public ActionResult<LoginResponse> Login(UserModel userModel)
        {
            //get user to authenticate
            var validUser = _userService.GetUser(userModel);

            if (validUser != null)
            {
                //generate the JWT Token
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
