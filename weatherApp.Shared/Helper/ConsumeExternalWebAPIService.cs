using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using weatherApp.Shared.Enum;

namespace weatherApp.Shared.Helper
{
    public class ConsumeExternalWebAPIService<T>
    {
        private readonly HttpClient _httpClient;
        private HttpResponseMessage _httpResponse;


        /// <summary>
        /// Use this .ctor when specific things need to be done to 
        /// HttpClient object or anyother object
        /// </summary>
        public ConsumeExternalWebAPIService()
        {
            _httpClient = new HttpClient();
            _httpResponse = new HttpResponseMessage();

            //CONTENT TYPE is set to "application/json" format by default
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Process any API calls with accessToken and return back the response as JObject
        /// </summary>
        /// <param name="authParams">this could be an accessToken,User credentials,etc</param>
        /// <param name="URL"></param>
        /// <param name="httpVerb"></param>
        /// <param name="requestObject"></param>
        /// <param name="method"></param>
        /// <param name="scheme"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public async Task<T> ProcessAPIRequest(string authParams, string URL, HttpVerb httpVerb, object requestObject, string scheme = "Bearer", string mediaType = "application/json")
        {
            var result = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(authParams))
                {
                    //Set authorize access token header
                    AuthorizeForAPIs(authParams, scheme);
                }

                if (httpVerb == HttpVerb.GET || httpVerb == HttpVerb.DELETE)
                {
                    _httpResponse = ProcessHttpGetOrDelete(URL, httpVerb);
                }

                if (requestObject != null && (httpVerb == HttpVerb.POST || httpVerb == HttpVerb.PUT))
                {
                    _httpResponse = ProcessHttpPostOrPut(URL, httpVerb, requestObject, mediaType);
                }

                result = _httpResponse?.Content?.ReadAsStringAsync().Result;

                return _httpResponse.IsSuccessStatusCode ? JsonConvert.DeserializeObject<T>(result) : throw new Exception($"{_httpResponse.StatusCode} - {result}");

            }
            catch (Exception) { throw; }
            finally
            {
                var isErrorLog = false;

                var response = $"{_httpResponse?.Content?.ReadAsStringAsync().Result} => {_httpResponse}";
            }
        }

        /// <summary>
        /// Set access token in header for authorization
        /// </summary>
        /// <param name="authParams"></param>
        /// <param name="scheme"></param>
        private void AuthorizeForAPIs(string authParams, string scheme)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, authParams);
        }

        /// <summary>
        /// Process Get or Delete calls
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="httpVerb"></param>
        /// <returns></returns>
        private HttpResponseMessage ProcessHttpGetOrDelete(string URL, HttpVerb httpVerb)
        {
            try
            {
                if (httpVerb == HttpVerb.GET)
                {
                    _httpResponse = _httpClient.GetAsync(new Uri(URL)).Result;
                }

                if (httpVerb == HttpVerb.DELETE)
                {
                    _httpResponse = _httpClient.DeleteAsync(new Uri(URL)).Result;
                }

                return _httpResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Process Post or Put calls
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="httpVerb"></param>
        /// <param name="requestObject"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        private HttpResponseMessage ProcessHttpPostOrPut(string URL, HttpVerb httpVerb, object requestObject, string mediaType)
        {
            try
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, mediaType);

                if (httpVerb == HttpVerb.POST)
                {
                    _httpResponse = _httpClient.PostAsync(URL, httpContent).Result;
                }

                if (httpVerb == HttpVerb.PUT)
                {
                    _httpResponse = _httpClient.PutAsync(URL, httpContent).Result;
                }

                return _httpResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
