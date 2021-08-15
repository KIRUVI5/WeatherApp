using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using weatherApp.Shared.Constant;
using weatherApp.Shared.Enum;
using weatherApp.Shared.Helper;
using weatherApp.Shared.Model;
using weatherApp.Shared.Response;
using weatherAppUI.Models;

namespace weatherAppUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.UserName) || string.IsNullOrEmpty(userModel.Password))
            {
                return (RedirectToAction("Error"));
            }

            var _service = new ConsumeExternalWebAPIService<JObject>();

            var loginResposne = await _service.ProcessAPIRequest(string.Empty, ExternalEndpoints.LoginUrl, HttpVerb.POST, userModel);

            if (loginResposne != null)
            {
                var loginResult = JsonConvert.DeserializeObject<LoginResponse>(loginResposne.ToString());

                HttpContext.Session.SetString("Token", loginResult.Token.ToString());
                return RedirectToAction("Index");

            }
            else
            {
                return (RedirectToAction("Error"));
            }
        }

        public IActionResult Index()
        {
            string token = HttpContext.Session.GetString("Token");
            var _service = new ConsumeExternalWebAPIService<JObject>();
            WeatherAPIResponse response = null;

            try
            {
                var apiResponse = _service.ProcessAPIRequest(token, ExternalEndpoints.GetAllWeatherData, HttpVerb.GET, null);
                if (apiResponse != null)
                {
                    response = JsonConvert.DeserializeObject<WeatherAPIResponse>(apiResponse.Result.ToString());
                }
            }
            catch (Exception ex)
            {
                return (RedirectToAction("Error"));
            }

            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WeatherData data)
        {
            string token = HttpContext.Session.GetString("Token");

            if (ModelState.IsValid)
            {
                var _service = new ConsumeExternalWebAPIService<JObject>();
                var apiResponse = await _service.ProcessAPIRequest(token, ExternalEndpoints.AddWeatherData, HttpVerb.POST, data);

                if (apiResponse != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
