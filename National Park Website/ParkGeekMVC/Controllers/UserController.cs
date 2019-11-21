using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkGeekMVC.Models;
using Security.BusinessLogic;
using Security.DAO;
using Security.Exceptions;
using Security.Models;
using Security.Models.Database;
using SessionControllerData;

namespace ParkGeekMVC.Controllers
{
    public class UserController : AuthenticationController
    {
        private const string LOGIN_ERROR = "The username or password are invalid.";
        
        public UserController(IUserSecurityDAO db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            LogoutUser();

            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel vm)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                try
                {
                    LoginUser(vm.Username, vm.Password);

                    result = RedirectToAction("Index", "Park");
                }
                catch (UserDoesNotExistException)
                {
                    ModelState.AddModelError("invalid-user", LOGIN_ERROR);
                }
                catch (PasswordMatchException)
                {
                    ModelState.AddModelError("invalid-user", LOGIN_ERROR);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid-user", ex.Message);
                }
            }

            return result;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel vm)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User();
                    user.FirstName = vm.FirstName;
                    user.LastName = vm.LastName;
                    user.Email = vm.Email;
                    user.Password = vm.Password;
                    user.ConfirmPassword = vm.ConfirmPassword;
                    user.Username = vm.Username;

                    RegisterUser(user);

                    result = RedirectToAction("Index", "Park");
                }
                catch (UserExistsException)
                {
                    ModelState.AddModelError("invalid-user", "The username is already taken.");
                }
                catch (PasswordMatchException)
                {
                    ModelState.AddModelError("invalid-user", "The passwords do not match.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid-user", ex.Message);
                }
            }

            return result;
        }



    }
}