using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkGeek;
using ParkGeek.DAL;
using ParkGeek.DAL.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace ParkGeekTests
{
    [TestClass]
    public class IntegrationTests
    {
        private string _connectionstring = "Data Source=localhost\\sqlexpress;Initial Catalog=NPGeek;Integrated Security=True";
        private TransactionScope _tran = null;
        private IParkGeekDAO _dao = null;

        [TestInitialize]
        public void Initialize()
        {
            _dao = new ParkGeekDAO(_connectionstring);
            _tran = new TransactionScope();
        }

        /// <summary>
        /// Tests the pulling of park information from a list and individual park data
        /// and their 5 day forecast
        /// </summary>
        [TestMethod]
        public void ParkInformationTest()
        {
            //Adds and verifies new park has been added to list
            int parkCount = _dao.GetParkList().Count;
            AddPark();
            int newParkCount = _dao.GetParkList().Count;
            Assert.AreEqual(parkCount + 1, newParkCount);

            //Tests that the park information was pulled correctly for
            //the chosen park
            string parkCode = "TE";
            var testPark = _dao.GetPark(parkCode);
            Assert.AreEqual("Tech Elevator", testPark.ParkName);
            Assert.AreEqual("Ohio", testPark.State);
            Assert.AreEqual(1, testPark.Acreage);
            Assert.AreEqual(30, testPark.ElevationInFeet);
            Assert.AreEqual(1, testPark.MilesOfTrail);
            Assert.AreEqual(2, testPark.NumberOfCampsites);
            Assert.AreEqual(2019, testPark.YearFounded);
            Assert.AreEqual(30, testPark.AnnualVisitors);
            Assert.AreEqual("Do what you want", testPark.InspirationalQuote);
            Assert.AreEqual("Chris Rupp", testPark.QuoteSource);
            Assert.AreEqual("Elevate yourself", testPark.ParkDescription);
            Assert.AreEqual(15500, testPark.EntryFee);
            Assert.AreEqual(1, testPark.NumberOfAnimalSpecies);

            //Adds fake weather to the park and tests how many days
            //of forecast were returned
            AddWeatherForecast();
            Assert.AreEqual(1, _dao.GetWeatherForecast(parkCode).Count);
        }

        [TestMethod]
        public void SubmittingSurveyTest()
        {
            Survey survey = new Survey()
            {
                ParkCode = "TE",
                Email = "Bob@TE.com",
                State = "Ohio",
                ActivityLevel = "Xtreme",
            };

            //Adds park to avoid conflict with FOREIGN KEY constraint
            AddPark();
            //Adds custom survey to the list of current surveys
            _dao.AddSurvey(survey);

            //Creates list of all surveys with new survey added
            var surveyResults = _dao.GetSurveyResults();
            IList <SurveyResult> testResultList= new List<SurveyResult>();
            foreach(var result in surveyResults)
            {
                if(result.ParkCode=="TE")
                {
                    testResultList.Add(result);
                }
            }

            Assert.AreEqual(1, testResultList[0].NumberOfReviews);
            Assert.AreEqual("Tech Elevator", testResultList[0].ParkName);
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }
        private void AddPark()
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                string sqlString = "INSERT INTO park(parkCode, parkName, state, acreage, elevationInFeet, " +
                    "milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, " +
                    "inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) " +
                    "VALUES (@parkCode, @parkName, @state, @acreage, @elevationInFeet, @milesOfTrail, @numberOfCampsites, " +
                    "@climate, @yearFounded, @annualVisitorCount, @inspirationalQuote, @inspirationalQuoteSource, @parkDescription, " +
                    "@entryFee, @numberOfAnimalSpecies);";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                cmd.Parameters.AddWithValue("@parkCode", "TE");
                cmd.Parameters.AddWithValue("@parkName", "Tech Elevator");
                cmd.Parameters.AddWithValue("@state", "Ohio");
                cmd.Parameters.AddWithValue("@acreage", 1);
                cmd.Parameters.AddWithValue("@elevationInFeet", 30);
                cmd.Parameters.AddWithValue("@milesOfTrail", 1);
                cmd.Parameters.AddWithValue("@numberOfCampsites", 2);
                cmd.Parameters.AddWithValue("@climate", "Cold");
                cmd.Parameters.AddWithValue("@yearFounded", 2019);
                cmd.Parameters.AddWithValue("@annualVisitorCount", 30);
                cmd.Parameters.AddWithValue("@inspirationalQuote", "Do what you want");
                cmd.Parameters.AddWithValue("@inspirationalQuoteSource", "Chris Rupp");
                cmd.Parameters.AddWithValue("@parkDescription", "Elevate yourself");
                cmd.Parameters.AddWithValue("@entryFee", 15500);
                cmd.Parameters.AddWithValue("@numberOfAnimalSpecies", 1);
                cmd.ExecuteNonQuery();
            }
        }
        private void AddWeatherForecast()
        {
            Weather weather = new Weather()
            {
            ParkCode = "TE",
            FiveDayForecastValue = 1,
            LowTemp = 55,
            HighTemp =72,
            Forecast = "sunny",
            };
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                string sqlString = "INSERT INTO weather(parkCode, fiveDayForecastValue, low, high, forecast) " +
                "VALUES (@parkCode, @fiveDayForecastValue, @low, @high, @forecast);";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                cmd.Parameters.AddWithValue("@parkCode", weather.ParkCode);
                cmd.Parameters.AddWithValue("@fiveDayForecastValue", weather.FiveDayForecastValue);
                cmd.Parameters.AddWithValue("@low", weather.LowTemp);
                cmd.Parameters.AddWithValue("@high", weather.HighTemp);
                cmd.Parameters.AddWithValue("@forecast", weather.Forecast);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
