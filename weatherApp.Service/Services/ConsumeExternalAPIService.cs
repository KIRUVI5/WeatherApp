using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using weatherApp.Service.Interface;
using weatherApp.Shared.Enum;
using weatherApp.Shared.Helper;
using weatherApp.Shared.Response;

namespace weatherApp.Service.Services
{
    public class ConsumeExternalAPIService : IConsumeExternalAPIService
    {

        public WeatherDataResponse ConsumeWetherData(string url)
        {
            var _service = new ConsumeExternalWebAPIService<JObject>();

            try
            {
                var apiResponse = _service.ProcessAPIRequest(string.Empty, url, HttpVerb.GET, null);

                var response = JsonConvert.DeserializeObject<WeatherDataResponse>(apiResponse.Result.ToString());

                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
        }

    }
}
