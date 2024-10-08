using Newtonsoft.Json;
using WeatherForEver.Models.WeatherModels;

namespace WeatherForEver.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(string city)
        {
            var apiKey = "06256b06731e46bba1e82248240810";
            var url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={Uri.EscapeDataString(city)}&aqi=no";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Details: {errorContent}");

                // Handle specific status codes
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new HttpRequestException("Invalid API key. Please check your key.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException($"City '{city}' cannot be found.");
                }
                else
                {
                    // For any other unexpected status codes
                    throw new HttpRequestException($"Error: {response.StatusCode}. Details: {errorContent}");
                }
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WeatherResponse>(jsonResponse);
        }
    }
}
