﻿using AutoMapper;
using System.Linq;
using System.Web.Mvc;
using testmvc.App_LocalResources;
using testmvc.Filters;
using testmvc.Models;
using WebMatrix.WebData;

namespace testmvc.Controllers
{
    [InitializeSimpleMembership]
    public class AccountController : BaseController
    {
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
            UsersContext context = new UsersContext();

            RegisterUserViewModel user = new RegisterUserViewModel();

            if (!context.Users.Any())
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

            UsersContext context = new UsersContext();

            // First user is admin
            bool isAdmin = !context.Users.Any();

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

            UsersContext context = new UsersContext();

            var userById = from Users in context.Users where Users.UserId == _id select Users;

            EditUserViewModel user = Mapper.Map<EditUserViewModel>(userById.FirstOrDefault<UserModel>());

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

            UsersContext context = new UsersContext();

            UserModel dbUser = context.Users.FirstOrDefault(u => u.UserId == user.UserId);

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;

            context.SaveChanges();

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [Authorize]
        public string Edit(int id, string firstName, string lastName, string email)
        {
            return "OK";
        }
    }
}
