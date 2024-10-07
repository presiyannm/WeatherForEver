using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace WeatherForEver.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string Name { get; set; }    
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile AccountPhoto { get; set; }

    }
}
