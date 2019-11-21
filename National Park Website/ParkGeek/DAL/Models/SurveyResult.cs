using System;
using System.Collections.Generic;
using System.Text;

namespace ParkGeek.DAL.Models
{
    public class SurveyResult
    {
        public string ParkCode { get; set; }

        public string ParkName { get; set; }

        public int NumberOfReviews { get; set; }
    }
}
