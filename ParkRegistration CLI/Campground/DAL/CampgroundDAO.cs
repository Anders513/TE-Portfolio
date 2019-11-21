using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundDAO
    {
        //private string _parkName;
        //private int _campgroundId;
        //private DateTime _arrivalTime;
        //private DateTime _departureTime;
        private string _connectionString;

        public CampgroundDAO(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        ///<summary>
        ///Pulls out the park of choice's info and displays it
        ///</summary>
        public Park GetPark(string parkName)
        {           
            var result = new Park();

            const string sql = "SELECT * FROM park WHERE name = @name";
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", parkName);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Park park = GetParkFromReader(reader);
                    result = park;
                }
            }
            return result;
        }

        ///<summary>
        ///Pulls out all the campgrounds in the given park
        ///</summary>
        public List<Campground> GetCampgrounds(string parkName)
        {
            var result = new List<Campground>();
            const string sql = "SELECT * FROM campground JOIN park ON park.park_id = campground.park_id WHERE park.name = @parkName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkName", parkName);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Campground campground = GetCampgroundFromReader(reader);
                    result.Add(campground);
                }
            }
            return result;
        }

        ///<summary>
        ///Pulls out all the sites in the given campground
        ///</summary>
        public List<Site> GetAvailableSites(int campgroundId, DateTime arrivalTime, DateTime departureTime)
        {
            var result = new List<Site>();

            const string sql = "Select Top 5 * From [site] " +
                "Join [campground] On [campground].campground_id = [site].campground_id " +
                "Where [site].site_id Not In(Select [site].site_id From [site] " +
                "Join [reservation] On [reservation].site_id = [site].site_id " +
                "Where (Not ([reservation].to_date < @arrivalTime Or [reservation].from_date > @departureTime)))" +
                "And[site].campground_id = @campgroundId " +
                "Order By[site].site_number;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                cmd.Parameters.AddWithValue("@arrivalTime", arrivalTime);
                cmd.Parameters.AddWithValue("@departureTime", departureTime);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Site site = GetSiteFromReader(reader);
                    result.Add(site);
                }
            }
            return result;
        }

        ///<summary>
        ///Adds a reservation to the database
        ///</summary>
        private int AddReservation(Reservation reservation)
        {
            int result = 0;
            const string sql = "INSERT INTO reservation (site_id, name, from_date, to_date, create_date) " +
                "VALUES (@site_id, @nameEntered, @from_date, @to_Date, @create_date) ";


            string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql+_getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@site_id", reservation.SiteID);
                cmd.Parameters.AddWithValue("@nameEntered", reservation.Name);
                cmd.Parameters.AddWithValue("@from_date", reservation.FromDate);
                cmd.Parameters.AddWithValue("@to_date", reservation.ToDate);
                cmd.Parameters.AddWithValue("@create_date", reservation.CreateADate);
                result = (int)cmd.ExecuteScalar();

            }
            return result;
        }

        ///<summary>
        ///Creates a reservation object to add to the database
        ///</summary>
        public int CreateReservation(int siteNumber, string name, ReservationInfo info, List<Site> sites)
        {
            var siteId = 0;           
            foreach(var site in sites)
            {
                if(site.SiteNumber==siteNumber)
                {
                    siteId = site.SiteID;
                }
            }

            Reservation reservation = new Reservation()
            {
                Name = name,
                SiteID = siteId,
                ToDate = info.DepartureDate,
                FromDate = info.ArrivalDate,
                CreateADate = DateTime.UtcNow
            };            
            return AddReservation(reservation);
        }

        ///<summary>
        ///Creates a new park object from the database
        ///</summary>
        private Park GetParkFromReader(SqlDataReader reader)
        {
            Park park = new Park();

            park.ParkID = Convert.ToInt32(reader["park_id"]);
            park.Name = Convert.ToString(reader["name"]);
            park.Location = Convert.ToString(reader["location"]);
            park.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
            park.Area = Convert.ToInt32(reader["area"]);
            park.Visitors = Convert.ToInt32(reader["visitors"]);
            park.Description = Convert.ToString(reader["description"]);

            return park;
        }

        ///<summary>
        ///Creates a new campground object from the database
        ///</summary>
        private Campground GetCampgroundFromReader(SqlDataReader reader)
        {
            Campground campground = new Campground();

            campground.CampgroundID= Convert.ToInt32(reader["campground_id"]);
            campground.ParkID = Convert.ToInt32(reader["park_id"]);
            campground.Name = Convert.ToString(reader["name"]);
            campground.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
            campground.CloseMonth = Convert.ToInt32(reader["open_to_mm"]);
            campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

            return campground;
        }

        ///<summary>
        ///Creates a new site object from the database
        ///</summary>
        private Site GetSiteFromReader(SqlDataReader reader)
        {
            Site site = new Site();

            site.SiteID = Convert.ToInt32(reader["site_id"]);
            site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
            site.Accessible = Convert.ToInt32(reader["accessible"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.Utilities = Convert.ToInt32(reader["utilities"]);

            return site;
        }

        ///<summary>
        ///Creates a new reservation object from the database
        ///</summary>
        private Reservation GetReservationFromReader(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationID = Convert.ToInt32(reader["reservation_id"]);
            reservation.SiteID = Convert.ToInt32(reader["site_id"]);
            reservation.Name = Convert.ToString(reader["name"]);
            //reservation.UserID = Convert.ToInt32(reader["user_id"]);
            reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
            reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
            reservation.CreateADate = Convert.ToDateTime(reader["create_date"]);

            return reservation;
        }
    }
}
