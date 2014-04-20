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
            UsersListViewModel model = new UsersListViewModel();

            foreach (var user in usersRepository.Get(u => u.LoginName != User.Identity.Name))
            {
                model.Users.Add(user);
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
