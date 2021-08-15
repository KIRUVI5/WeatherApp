using weatherApp.Shared.Response;

namespace weatherApp.Service.Interface
{
    public interface IConsumeExternalAPIService
    {
        WeatherDataResponse ConsumeWetherData(string url);
    }
}
