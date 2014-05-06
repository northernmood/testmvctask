using System.Web.Mvc;
using testmvc.Models;
using testmvc.Helpers;
using WebMatrix.WebData;

namespace testmvc.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            UsersListViewModel model = new UsersListViewModel();

            foreach (var user in usersRepository.Get(u => u.UserId != WebSecurity.CurrentUserId))
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
