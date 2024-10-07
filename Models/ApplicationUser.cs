using Microsoft.AspNetCore.Identity;

namespace WeatherForEver.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string AccountPhoto { get; set; }
    }
}
