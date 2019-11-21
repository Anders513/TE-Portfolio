using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Security.BusinessLogic;
using Security.DAO;
using Security.Models;
using Security.Models.Database;
using SessionControllerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkGeekMVC.Controllers
{
    public class AuthenticationController : SessionController
    {
        protected const string USER_KEY = "UserData";
        private IUserSecurityDAO _db = null;

        public AuthenticationController(IUserSecurityDAO db, IHttpContextAccessor httpContext) : base(httpContext)
        {
            _db = db;
        }

        public IActionResult GetAuthenticatedView(string viewName, object model = null)
        {
            IActionResult result = null;

            var userMgr = GetUserManager();
            if (userMgr.IsAuthenticated)
            {
                result = View(viewName, model);
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }
            return result;
        }

        protected UserManager GetUserManager()
        {
            var user = GetSessionData<UserItem>(USER_KEY);
            UserManager userMgr = new UserManager(_db, user);
            
            return userMgr;
        }

        protected void LoginUser(string username, string password)
        {
            var userMgr = GetUserManager();
            userMgr.LoginUser(username, password);
            SetSessionData<UserItem>(USER_KEY, userMgr.User);
        }

        protected void LogoutUser()
        {
            var userMgr = GetUserManager();
            userMgr.LogoutUser();
            SetSessionData<UserItem>(USER_KEY, userMgr.User);
        }

        protected void RegisterUser(User user)
        {
            UserManager userMgr = GetUserManager();
            userMgr.RegisterUser(user);
            SetSessionData<UserItem>(USER_KEY, userMgr.User);
        }
    }
}
