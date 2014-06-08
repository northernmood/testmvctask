using AutoMapper;
using System.Linq;
using System.Web.Mvc;
using testmvc.App_LocalResources;
using testmvc.Models;
using testmvc.Repository;
using System;
using log4net;
using testmvc.WebSecurityImpl;

namespace testmvc.Controllers
{
    public class AccountController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountController));

        public AccountController(IUsersRepository repository, IWebSecurityWrapper webSecurity)
            : base(repository, webSecurity)
        {
        }

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


        [Authorize]
        public ActionResult Edit(EditUserViewModel user)
        {
            if(!ModelState.IsValid)
            {
                var results = ModelState
                    .Where(m => m.Value.Errors.Count > 0)
                    .Select(m => new { Field = m.Key, Message = m.Value.Errors[0].ErrorMessage }).ToArray();

                //ModelState.AddModelError("", "error message");

                Response.StatusCode = 400;

                return Json(results);
            }

            SaveUser(user);
            
            Response.StatusCode = 200;

            return Json("OK");
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
