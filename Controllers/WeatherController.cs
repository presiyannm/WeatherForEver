using Microsoft.AspNetCore.Mvc;
using WeatherForEver.Services;
using WeatherForEver.Models.WeatherModels;

public class WeatherController : Controller
{
    private readonly WeatherService _weatherService;

    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<IActionResult> Index(string city)
    {
        if (string.IsNullOrEmpty(city))
        {
            ViewBag.ErrorMessage = "Please enter a city.";
            return View(null);
        }

        try
        {
            var weatherData = await _weatherService.GetWeatherAsync(city);

            if (weatherData == null)
            {
                ViewBag.ErrorMessage = $"City '{city}' not found.";
                return View(null);
            }

            return View(weatherData);
        }
        catch (HttpRequestException ex)
        {
            // Log the exception and pass the message to the view
            Console.WriteLine(ex.Message);
            ViewBag.ErrorMessage = ex.Message;
            return View(null);
        }
    }
}
