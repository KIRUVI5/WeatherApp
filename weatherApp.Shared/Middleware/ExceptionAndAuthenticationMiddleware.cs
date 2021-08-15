using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using weatherApp.Shared.Classes;
using weatherApp.Shared.Exceptions;
using weatherApp.Shared.Extensions;

namespace weatherApp.Shared.Middleware
{
    public class ExceptionAndAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;
        // private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionAndAuthenticationMiddleware(RequestDelegate next,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //var sessionToken = context.Session.GetString("Token");
                // var sessionToken = _session.;
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                    AttachUserToContext(context, token);

                await _next(context);
            }
            catch (Exception exception)
            {
                //_logger.LogError(exception, "");
                await HandleErrorsAsync(
                    context,
                    exception,
                    GetStatusCodeFromException(exception),
                    _hostingEnvironment.IsLive() ? null : $"{exception.TargetSite.ReflectedType.FullName}->{exception.TargetSite.Name} ({new StackTrace(exception, true).GetFrame(0)?.GetFileLineNumber()})"//https://stackoverflow.com/a/3329072/1677973
                );
            }
        }

        /// <summary>
        /// Creates the JSON to send back
        /// </summary>
        /// <remarks>
        /// Can also end up here for example with a 404 where no exception is provided
        /// </remarks>
        public static async Task HandleErrorsAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string target = null)
        {
            // stop any caching of the errors
            context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
            context.Response.Headers[HeaderNames.Pragma] = "no-cache";
            context.Response.Headers[HeaderNames.Expires] = "0";
            context.Response.Headers.Remove(HeaderNames.ETag);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var apiError = new APIError()
            {
                Message = string.IsNullOrEmpty(exception?.Message) ? ReasonPhrases.GetReasonPhrase((int)statusCode) : exception?.Message,
                ExtraInfo = (exception as ExtendedArgumentException)?.ExtraInfo,
                HTTPCode = statusCode,
                ExceptionName = exception?.GetType().Name,
            };

            await context.Response.WriteAsync(
                apiError.ToString()
            );
        }

        /// <summary>
        /// Set HTTP status code depending upon exception type
        /// </summary>
        // 403, 404 and 409 are custom exceptions -> https://stackoverflow.com/questions/3297048/403-forbidden-vs-401-unauthorized-http-responses, https://leastprivilege.com/2014/10/02/401-vs-403/
        public static HttpStatusCode GetStatusCodeFromException(Exception exception)
        {
            if (exception is ArgumentException ||
                exception is ExtendedArgumentException)
            {
                return HttpStatusCode.BadRequest;               // 400
            }
            else if (exception is UnauthorizedAccessException ||
                exception is SecurityTokenException ||
                exception is SecurityTokenExpiredException ||
                exception is SecurityTokenNotYetValidException ||
                exception is SecurityTokenReplayDetectedException ||
                exception is SecurityTokenValidationException)
            {
                return HttpStatusCode.Unauthorized;             // 401 - not authenticated
            }
            else if (exception is ForbiddenException)
            {
                return HttpStatusCode.Forbidden;                // 403 - not authorised
            }
            else if (exception is NotFoundException)
            {
                return HttpStatusCode.NotFound;                 // 404
            }
            else if (exception is NotSupportedException)
            {
                return HttpStatusCode.MethodNotAllowed;         // 405
            }
            else if (exception is AlreadyExistsException)
            {
                return HttpStatusCode.Conflict;                 // 409
            }
            else if (exception is NotImplementedException)
            {
                return HttpStatusCode.NotImplemented;           // 501
            }
            else
            {
                return HttpStatusCode.InternalServerError;      // 500 - such as InvalidOperationException
            }
        }


        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                //get JWTAuthentication section from appsetting.json
                var JWTCongig = _configuration.GetSection("JWTAuthentication");

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTCongig["JwtKey"]));

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = securityKey,
                    ValidateAudience = true,
                    ValidIssuer = JWTCongig["JwtIssuer"],
                    ValidAudience = JWTCongig["JwtAudience"],
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
