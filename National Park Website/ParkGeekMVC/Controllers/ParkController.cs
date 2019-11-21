using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkGeek;
using ParkGeek.DAL.Models;
using ParkGeekMVC.Models;
using Security.DAO;

namespace ParkGeekMVC.Controllers
{
    public class ParkController : AuthenticationController
    {
        private IParkGeekDAO _dao = null;

        public ParkController(IUserSecurityDAO db, IHttpContextAccessor httpContext, IParkGeekDAO dao) : base(db, httpContext)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            IList<Park>ListOfParks = _dao.GetParkList();

            return GetAuthenticatedView("Index", ListOfParks);
        }
        public IActionResult Detail(string code)
        {
            ParkViewModel parks = new ParkViewModel();
            parks.Park= _dao.GetPark(code);
            parks.Weather = _dao.GetWeatherForecast(code);

            return GetAuthenticatedView("Detail", parks);
        }
   


    }
}
