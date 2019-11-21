using System;
using System.Collections.Generic;
using System.Text;

namespace ParkGeek.DAL.Models
{
    public class Weather
    {
        public string ParkCode { get; set; }

        public int FiveDayForecastValue { get; set; }

        public int LowTemp { get; set; }

        public int HighTemp { get; set; }

        public string Forecast { get; set; }
    }
}
