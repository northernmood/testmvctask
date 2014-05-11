using System.Web.Mvc;
using testmvc.Models;
using testmvc.Helpers;
using testmvc.Repository;
using testmvc.WebSecurityImpl;
using System.Linq;

namespace testmvc.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController() : this(new UsersRepository(), new WebSecurityWrapper())
        {
        }

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


        [HttpPost]
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            CultureHelper.SetCulture(culture, Request, Response);

            return Redirect(returnUrl);
        }
    }
}
