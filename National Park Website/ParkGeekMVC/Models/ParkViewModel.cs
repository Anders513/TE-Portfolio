using ParkGeek.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkGeekMVC.Models
{
    public class ParkViewModel
    {             
        public Park Park { get; set; }

        public IList<Weather> Weather { get; set; }

        public string ForecastDescription(string forecast)
        {
            string result = "";

            if(forecast == "rain")
            {
                result = "Bring an umbrella!";
            }
            else if (forecast == "partly cloudy")
            {
                result = "Bring a light jacket!";
            }
            else if (forecast == "cloudy")
            {
                result = "You probably wont need to bring much!";
            }
            else if (forecast == "thunderstorms")
            {
                result = "Stay inside!";
            }
            else if (forecast == "sunny")
            {
                result = "Bring sunscreen!";
            }
            else if (forecast == "snow")
            {
                result = "Wear a coat!";
            }

            return result;
        }

        public int FahrenheitToCelsius(int temp)
        {
            return (int)((temp-32)*.556);
        }

        public DayOfWeek CurrentDay(int dayValue)
        {
            DateTime date = DateTime.UtcNow;
            var newDate = date.AddDays(dayValue - 1);
            var dayOfTheWeek = newDate.DayOfWeek;
            return dayOfTheWeek;
        }
    }
}
