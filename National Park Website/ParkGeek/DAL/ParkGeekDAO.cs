using ParkGeek.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ParkGeek.DAL
{
    public class ParkGeekDAO: IParkGeekDAO
    {
        private string _connectionstring;
        
        public ParkGeekDAO(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        /// <summary>
        /// Returns a list of national parks from a database
        /// </summary>
        /// <returns></returns>
        public IList<Park> GetParkList()
        {
            IList<Park> parkList = new List<Park>();           

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM park", conn);                

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    parkList.Add(MapRowToPark(reader));
                }
            }
            return parkList;
        }

        /// <summary>
        /// Returns a park object with all the info based on user selection
        /// </summary>
        /// <param name="parkCode">Park code of the chosen park by user</param>
        /// <returns></returns>
        public Park GetPark(string parkCode)
        {
            Park parkChoice = new Park();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM park WHERE parkCode=@parkcode", conn);
                cmd.Parameters.AddWithValue("@parkCode", parkCode);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    parkChoice = MapRowToPark(reader);
                }
            }
            return parkChoice;
        }

        /// <summary>
        /// Returns a list of the weather forecast for national parks from a database
        /// </summary>
        /// <returns></returns>
        public IList<Weather> GetWeatherForecast(string parkCode)
        {
            IList<Weather> weatherForecast = new List<Weather>();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM weather WHERE parkCode = @parkCode;", conn);
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    weatherForecast.Add(MapRowToForecast(reader));
                }
            }
            return weatherForecast;
        }

        /// <summary>
        /// Returns a list of parks and number of surveys from a database
        /// </summary>
        /// <returns></returns>
        public IList<SurveyResult> GetSurveyResults()
        {
            IList<SurveyResult> surveyResultList = new List<SurveyResult>(); 

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                string sqlString = "SELECT survey_result.parkCode, park.parkName, COUNT(*) AS 'total_reviews' " +
                    "FROM survey_result JOIN park ON survey_result.parkCode = park.parkCode " +
                    "GROUP BY survey_result.parkCode, park.parkName ORDER BY 'total_reviews' DESC, park.parkName;";
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    surveyResultList.Add(MapRowToSurveyResult(reader));
                }
            }           
            return surveyResultList;
        }

        /// <summary>
        /// Adds a submitted survey to a list of surveys in a database
        /// </summary>
        /// <returns></returns>
        public void AddSurvey(Survey survey)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO survey_result(parkCode, emailAddress, state, activityLevel) " +
                    "VALUES (@parkCode, @emailAddress, @state, @activityLevel);", conn);

                cmd.Parameters.AddWithValue("@parkCode", survey.ParkCode);
                cmd.Parameters.AddWithValue("@emailAddress", survey.Email);
                cmd.Parameters.AddWithValue("@state", survey.State);
                cmd.Parameters.AddWithValue("@activityLevel", survey.ActivityLevel);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Creates a park object from reader
        /// </summary>
        /// <param name="reader">Line of sequel connection</param>
        /// <returns></returns>
        private Park MapRowToPark(SqlDataReader reader)
        {
            return new Park()
            {
                ParkCode = Convert.ToString(reader["parkCode"]),
                ParkName = Convert.ToString(reader["parkName"]),
                State = Convert.ToString(reader["state"]),
                Acreage = Convert.ToInt32(reader["acreage"]),
                ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]),
                MilesOfTrail = Convert.ToInt32(reader["milesOfTrail"]),
                NumberOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]),
                Climate = Convert.ToString(reader["climate"]),
                YearFounded = Convert.ToInt32(reader["yearFounded"]),
                AnnualVisitors = Convert.ToInt32(reader["annualVisitorCount"]),
                InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]),
                QuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]),
                ParkDescription = Convert.ToString(reader["parkDescription"]),
                EntryFee = Convert.ToDecimal(reader["entryFee"]),
                NumberOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]),
            };
        }

        /// <summary>
        /// Creates a weather object from reader
        /// </summary>
        /// <param name="reader">Line of sequel connection</param>
        /// <returns></returns>
        private Weather MapRowToForecast(SqlDataReader reader)
        {
            return new Weather()
            {
                ParkCode = Convert.ToString(reader["parkCode"]),                
                FiveDayForecastValue = Convert.ToInt32(reader["fiveDayForecastValue"]),
                LowTemp = Convert.ToInt32(reader["low"]),
                HighTemp = Convert.ToInt32(reader["high"]),
                Forecast = Convert.ToString(reader["forecast"]),
            };
        }

        /// <summary>
        /// Creates a Survey object from reader
        /// </summary>
        /// <param name="reader">Line of sequel connection</param>
        /// <returns></returns>
        private SurveyResult MapRowToSurveyResult(SqlDataReader reader)
        {
           
            return new SurveyResult()
            {
                ParkName = Convert.ToString(reader["parkName"]),
                ParkCode = Convert.ToString(reader["parkCode"]),
                NumberOfReviews = Convert.ToInt32(reader["total_reviews"]),
            };
        }
    }
}
