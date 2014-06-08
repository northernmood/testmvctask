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
using System.Web.UI;

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
            ViewBag.CurrentUserIsAdmin = GetCurrentUser().IsAdmin;

            return View();
        }


        [Authorize]
        [OutputCache(Location = OutputCacheLocation.Server, Duration = 60, SqlDependency = "Default:UserProfile", VaryByParam="*")]
        public ActionResult GetUsersFiltered(string filter, string orderBy = "UserId", int page = 0, int pageSize = 2)
        {
            if (pageSize <= 0) pageSize = 2;
            if (string.IsNullOrEmpty(orderBy)) orderBy = "UserId";

            IQueryable<UserModel> query = usersRepository.All().Where(u => u.UserId != WebSecurity.CurrentUserId);

            PropertyInfo p = typeof(UserModel).GetProperty(orderBy.Replace("_desc", ""));
            Func<UserModel, object> filt = u => p.GetValue(u);
            
            Func<IQueryable<UserModel>, IOrderedEnumerable<UserModel>> orderer = q => orderBy.EndsWith("_desc")
                ? q.OrderByDescending(filt)
                : q.OrderBy(filt);

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(u => 
                       u.FirstName.IndexOf(filter) >= 0 
                    || u.LastName.IndexOf(filter) >= 0
                    || u.Email.IndexOf(filter) >= 0);
            }

            List<UserModel> list = orderer(query).Skip(pageSize * page).Take(pageSize).ToList();

            ViewBag.Filter = filter;
            ViewBag.OrderBy = orderBy;
            ViewBag.Page = page;
            ViewBag.TotalPages = query.Count() / pageSize;

            return PartialView("_UsersList", new UsersListViewModel(list));
        }


        [HttpPost]
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            CultureHelper.SetCulture(culture, Request, Response);

            return Redirect(returnUrl);
        }
    }
}
