using System.Collections.Generic;

namespace Kurs
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get(int numberOfResults, int minTemp, int maxTemp);
    }
}