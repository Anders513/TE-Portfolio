using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Capstone.DAL;
using System.Collections.Generic;
using Capstone.Models;
using System;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundDAOTests
    {
        private string _connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=npcampground;Integrated Security=True";
        
        private TransactionScope _tranactionscope;

        [TestInitialize]
        public void DAOSetup()
        {
            _tranactionscope = new TransactionScope();
            ResetDatabase();
        }

        [TestMethod]
        public void UserReservationTests()
        {
            CampgroundDAO campgroundDAO = new CampgroundDAO(_connectionString);

            //Tests park info output 
            var parkInfo = campgroundDAO.ParkInfo(_parkName);
            var testParkInfo = ($"{"Tech Elevator"}\n");
            testParkInfo += string.Format("{0,-20}{1}\n", "Location: ", "Ohio");
            testParkInfo += string.Format("{0,-20}{1}\n", "Established: ", "10/25/2019");
            testParkInfo += string.Format("{0,-20}{1}\n", "Area(KM): ", "99999" + " SQ KM");
            testParkInfo += string.Format("{0,-20}{1}\n", "Annual Visitors: ", "500000");
            testParkInfo += string.Format("{0}{1,-40}\n", "Description: \n", "Lorem ipsum dolor sit amet");
            Assert.AreEqual(parkInfo, testParkInfo);

            //Tests campground info output
            var campgroundInfo = campgroundDAO.CampgroundInfo();
            var testCampInfo = string.Format("{0,10}{1,35}{2, 20}{3, 30}\n", "Name", "Open", "Close", "Daily Fee");
            testCampInfo += string.Format("{0,-5}", _campgroundId);
            testCampInfo += string.Format("{0,-37}", "YYYbYYY");
            testCampInfo += string.Format("{0,-19}", "3");
            testCampInfo += string.Format("{0,-25}", "5");
            testCampInfo += "25.00 \n";
            Assert.AreEqual(campgroundInfo, testCampInfo);

            //Verifies user selection of a campground
            //Is true
            var campgroundChoice = campgroundDAO.VerifyCampground(_campgroundId);            
            Assert.IsTrue(campgroundChoice);
            //Is false
            campgroundChoice = campgroundDAO.VerifyCampground(20304009);        
            Assert.IsFalse(campgroundChoice);

            //Verifies user dates and campground selection have available sites
            //during that time
            DateTime arrival = new DateTime(2022, 10, 25);
            DateTime departure = new DateTime(2022, 10, 28);
            var siteAvailable = campgroundDAO.VerifySiteAvailbility(_campgroundId, arrival, departure);
            Assert.IsTrue(siteAvailable);

            //Verifies user choice as a valid site choice for campground
            //Is true
            var siteChoice = campgroundDAO.VerifySite(1);
            Assert.IsTrue(siteChoice);
            //Is false
            siteChoice = campgroundDAO.VerifySite(5);
            Assert.IsFalse(siteChoice);

            //Tests site display after verification
            var siteInfo = campgroundDAO.SiteInfo();
            var testsiteInfo = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}\n", "Site ",
                "Max Occupancy ", "Accessible ", "MaxRVLength ", "Utilities ", "Cost");
            testsiteInfo += string.Format("{0,-20}", "1");
            testsiteInfo += string.Format("{0,-20}", "5");
            testsiteInfo += string.Format("{0,-20}", "1");
            testsiteInfo += string.Format("{0,-20}", "35");
            testsiteInfo += string.Format("{0,-20}", "1");
            testsiteInfo += string.Format("{0,-20}", "75.00");
            Assert.AreEqual(siteInfo, testsiteInfo);

            //Tests adding a reservation during chosen time
            var reservation = campgroundDAO.CreateReservation(_siteId, "Tech Elevator");
            Assert.AreEqual(2, reservation);                       
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tranactionscope.Dispose();
        }

        //Member Variables
        private string _parkName = "Tech Elevator";
        private int _parkId;
        private int _campgroundId;
        private int _siteId;
        //Creates new testable database
        private void ResetDatabase()
        {
            string sql = "DELETE FROM reservation; " +
                "DELETE FROM site; " +
                "DELETE FROM campground; " +
                "DELETE FROM park; " +
                "DBCC CHECKIDENT(reservation, RESEED, 0)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
            }
            CreateNewPark();
            CreateNewCampground();
            CreateNewCampsite();
            CreateNewReservation();
        }
        private void CreateNewPark()
        {
            string sql = "INSERT park (name, location, establish_date, area, visitors, description) " +
                "VALUES(@name, @location, @established, @area, @visitors, @description)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", "Tech Elevator");
                cmd.Parameters.AddWithValue("@location", "Ohio");
                cmd.Parameters.AddWithValue("@established",  new DateTime(2019, 10, 25));
                cmd.Parameters.AddWithValue("@area", "99999");
                cmd.Parameters.AddWithValue("@visitors", "500000");
                cmd.Parameters.AddWithValue("@description", "Lorem ipsum dolor sit amet");

                SqlDataReader reader = cmd.ExecuteReader();
            }
        }
        private void CreateNewCampground()
        {
            _parkId = PullParkId();
            string sql = "INSERT campground VALUES(@parkId, @name, @openDate, @closeDate, @dailyFee)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkId", _parkId);
                cmd.Parameters.AddWithValue("@name", "YYYbYYY");
                cmd.Parameters.AddWithValue("@openDate", "3");
                cmd.Parameters.AddWithValue("@closeDate", "5");
                cmd.Parameters.AddWithValue("@dailyFee", "25.00");

                SqlDataReader reader = cmd.ExecuteReader();
            }
        }
        private void CreateNewCampsite()
        {
            _campgroundId = PullCampgroundId();
            string sql = "INSERT site VALUES(@campgroundId, @siteNumber, " +
                "@maxOccupancy, @accessible, @maxRVLength, @utilities)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@campgroundId", _campgroundId);
                cmd.Parameters.AddWithValue("@siteNumber", "1");
                cmd.Parameters.AddWithValue("@maxOccupancy", "5");
                cmd.Parameters.AddWithValue("@accessible", "1");
                cmd.Parameters.AddWithValue("@maxRVLength", "35");
                cmd.Parameters.AddWithValue("@utilities", "1");

                SqlDataReader reader = cmd.ExecuteReader();
            }
        }
        private void CreateNewReservation()
        {
            _siteId = PullSiteId();
            string sql = "INSERT reservation (site_id, name, from_date, to_date, create_date) " +
                "VALUES(@siteId, @name, @dateStart, @dateEnd, @createdDate);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@siteId", _siteId);
                cmd.Parameters.AddWithValue("@name", "Tech Elevator Family");
                cmd.Parameters.AddWithValue("@dateStart", "2019-10-23");
                cmd.Parameters.AddWithValue("@dateEnd", "2019-10-25");
                cmd.Parameters.AddWithValue("@createdDate", DateTime.UtcNow);

                SqlDataReader reader = cmd.ExecuteReader();
            }
        }
        //Pulls data from each object in database to create new tables in database
        private int PullParkId()
        {
            int result = 0;
            const string sql = "SELECT park_id FROM park WHERE name = @parkName;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkName", _parkName);

                result = (int)cmd.ExecuteScalar();
            }
            return result;
        }
        private int PullCampgroundId()
        {
            int result = 0;
            const string sql = "SELECT campground_id FROM campground WHERE park_id = @parkId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkId", _parkId);

                result = (int)cmd.ExecuteScalar();
            }
            return result;
        }
        private int PullSiteId()
        {
            int result = 0;
            const string sql = "SELECT site_id FROM site WHERE campground_id = @campgroundId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@campgroundId", _campgroundId);

                result = (int)cmd.ExecuteScalar();
            }
            return result;
        }
    }
}


