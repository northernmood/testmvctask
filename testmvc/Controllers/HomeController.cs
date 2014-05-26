using System.Web.Mvc;
using testmvc.Models;
using testmvc.Helpers;
using testmvc.Repository;
using testmvc.WebSecurityImpl;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace testmvc.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUsersRepository repository, IWebSecurityWrapper webSecurity)
            : base(repository, webSecurity)
        {
        }

        [Authorize]
        public ActionResult Index()
        {
            UsersListViewModel model = new UsersListViewModel();

            foreach (var user in usersRepository.All().Where(u => u.UserId != WebSecurity.CurrentUserId))
            {
                model.Users.Add(user);
            }

            ViewBag.CurrentUserIsAdmin = GetCurrentUser().IsAdmin;

            return View(model);
        }


        [Authorize]
        public ActionResult GetUsersFiltered(string filter, string orderBy = "UserId", int page = 0, int pageSize = 2)
        {
            if (pageSize == 0) pageSize = 2;
            if (string.IsNullOrEmpty(orderBy)) orderBy = "UserId";

            UsersListViewModel model = new UsersListViewModel();

            IQueryable<UserModel> query = usersRepository.All().Where(u => u.UserId != WebSecurity.CurrentUserId);

            PropertyInfo p = typeof(UserModel).GetProperty(orderBy.Replace("_desc", ""));
            Func<UserModel, object> func = u => p.GetValue(u);

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(u => u.FirstName.IndexOf(filter) >= 0 || u.LastName.IndexOf(filter) >= 0);
            }

            List<UserModel> list;
            int total = query.Count();
            if (orderBy.EndsWith("_desc")) list = query.OrderByDescending(func).Skip(pageSize * page).Take(pageSize).ToList();
            else list = query.OrderBy(func).Skip(pageSize * page).Take(pageSize).ToList();

            model.Users.AddRange(list);

            ViewBag.Filter = filter;
            ViewBag.OrderBy = orderBy;
            ViewBag.Page = page;
            ViewBag.TotalPages = total / pageSize;

            return PartialView("_UsersList", model);
        }


        [HttpPost]
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            CultureHelper.SetCulture(culture, Request, Response);

            return Redirect(returnUrl);
        }
    }
}
