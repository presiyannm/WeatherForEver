namespace WeatherForEver.Models.WeatherModels
{
    public class Current
    {
        public double Temp_c { get; set; }
        public double Pressure_mb { get; set; }
        public int Humidity { get; set; }
        public Condition Condition { get; set; }
    }
}
