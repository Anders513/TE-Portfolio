using System;
using System.Collections.Generic;
using ParkGeek.DAL.Models;

namespace ParkGeek
{
    public interface IParkGeekDAO
    {
        IList<Park> GetParkList();

        Park GetPark(string parkCode);

        IList<Weather> GetWeatherForecast(string parkCode);

        IList<SurveyResult> GetSurveyResults();

        void AddSurvey(Survey survey);       
    }
}
