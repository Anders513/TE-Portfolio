using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class ReservationInfo
    {
        public int CampgroundId { get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }
    }
}
