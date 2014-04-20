using AutoMapper;
using System.Linq;
using System.Web.Mvc;
using testmvc.App_LocalResources;
using testmvc.Filters;
using testmvc.Models;
using WebMatrix.WebData;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using testmvc.Repository;
using System;
using log4net;

namespace testmvc.Controllers
{
    [InitializeSimpleMembership]
    public class AccountController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountController));

        [HttpGet]
        [ActionName("login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (WebSecurity.Login(user.LoginName, user.Password, persistCookie: user.RememberMe))
            {
                return RedirectToAction("index", "home");
            }

            ModelState.AddModelError(string.Empty, Strings.NoUserOrWrongPassword);

            return View(user);
        }

        [ActionName("register")]
        public ActionResult Register()
        {
            RegisterUserViewModel user = new RegisterUserViewModel();

            if (usersRepository.Count() == 0)
            {
                user.DataBaseIsEmpty = true;
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // First user is admin
            bool isAdmin = usersRepository.Count() == 0;

            try
            {
                // TODO transaction?
                WebSecurity.CreateUserAndAccount(
                    user.LoginName,
                    user.Password,
                    propertyValues: new
                    {
                        Email = user.Email,
                        IsAdmin = isAdmin
                    });

                WebSecurity.Login(user.LoginName, user.Password);
            }
            catch(InvalidOperationException ex)
            {
                log.Error(ex);
            }

            return RedirectToAction("index", "home");
        }

        [ActionName("logoff")]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("index", "home");
        }


        [HttpGet]
        [Authorize]
        public ActionResult Manage(int? id)
        {
            UserModel currentUser = GetCurrentUser();

            int _id = id.HasValue ? id.Value : currentUser.UserId;

            if (!currentUser.IsAdmin && currentUser.UserId != _id)
            {
                return RedirectToAction("index", "home");
            }

            UserModel userById = usersRepository.GetByID(_id);

            EditUserViewModel user = Mapper.Map<EditUserViewModel>(userById);

            return View(user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Manage(EditUserViewModel user)
        {
            UserModel currentUser = GetCurrentUser();

            if (!currentUser.IsAdmin && currentUser.UserId != user.UserId)
            {
                return RedirectToAction("index", "home");
            }

            if (!ModelState.IsValid)
            {
                return Manage(user.UserId);
            }

            SaveUser(user);

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditUserViewModel user)
        {
            ValidationContext validationContext = new ValidationContext(user, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(user, validationContext, validationResults, true))
            {
                var results = validationResults.Select(r => new { Field = r.MemberNames.First(), Message = r.ErrorMessage }).ToArray();

                Response.StatusCode = 400;

                return Json(results);
            }

            SaveUser(user);
            
            Response.StatusCode = 200;

            return Content("OK");
        }

        private void SaveUser(EditUserViewModel user)
        {
            UserModel dbUser = usersRepository.GetByID(user.UserId);

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;

            usersRepository.Save();
        }
    }
}
