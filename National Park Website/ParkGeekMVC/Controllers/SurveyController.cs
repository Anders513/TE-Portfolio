using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkGeek;
using ParkGeek.DAL.Models;
using ParkGeekMVC.Models;
using Security.DAO;

namespace ParkGeekMVC.Controllers
{
    public class SurveyController : AuthenticationController
    {
        private IParkGeekDAO _dao = null;

        public SurveyController(IUserSecurityDAO db, IHttpContextAccessor httpContext, IParkGeekDAO dao) : base(db, httpContext)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            SurveyViewModel vm = new SurveyViewModel();
            vm.ParkCodeList = ParkDropdown();

            return GetAuthenticatedView("Index", vm);
        }

        [HttpPost]
        public IActionResult PostSurvey(SurveyViewModel vm)
        {
            IActionResult result = null;
            if (!ModelState.IsValid)
            {
                vm.ParkCodeList = ParkDropdown();
                result = View("Index", vm);
            }
            else
            {
                Survey survey = new Survey()
                {
                    ParkCode = vm.ParkCode,
                    Email = vm.Email,
                    State = vm.State,
                    ActivityLevel = vm.ActivityLevel
                };
                _dao.AddSurvey(survey);
                result = RedirectToAction("FavoriteParks");
            }
            return result;
        }
        public IActionResult FavoriteParks()
        {
            IList<SurveyResult> surveyResults = _dao.GetSurveyResults();          

            return GetAuthenticatedView("FavoriteParks", surveyResults);
        }
        private IList<SelectListItem> ParkDropdown()
        {
            IList<SelectListItem> parkDropDown = new List<SelectListItem>();
            IList<Park> parkList = _dao.GetParkList();
            foreach (var park in parkList)
            {
                parkDropDown.Add(new SelectListItem() { Text = park.ParkName, Value = park.ParkCode });
            }
            return parkDropDown;
        }
    }
}