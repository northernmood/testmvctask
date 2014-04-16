using System.Linq;
using System.Web.Mvc;
using testmvc.Filters;
using testmvc.Models;

namespace testmvc.Controllers
{
    using AutoMapper;
    using testmvc.Helpers;

    [InitializeSimpleMembership]
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            UsersContext context = new UsersContext();

            UsersListViewModel model = new UsersListViewModel();

            var q = from Users in context.Users where Users.LoginName != User.Identity.Name select Users;

            foreach (var u in q)
            {
                model.Users.Add(u);
            }

            ViewBag.CurrentUserIsAdmin = GetCurrentUser().IsAdmin;

            return View(model);
        }


        [HttpPost]
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            CultureHelper.SetCulture(culture, Request, Response);

            return Redirect(returnUrl);
        }
    }
}
