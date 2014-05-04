using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using testmvc.Configuration;
using testmvc.Helpers;
using testmvc.Models;
using System.Linq;
using testmvc.Repository;

namespace testmvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IUsersRepository usersRepository;

        public BaseController()
        {
            usersRepository = new UsersRepository();
        }

        public BaseController(IUsersRepository repository)
        {
            usersRepository = repository;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            ViewBag.Culture = CultureHelper.GetCurrentCulture(requestContext.HttpContext.Request, Settings.DefaultCulture);

            base.Initialize(requestContext);
        }

        protected UserModel GetCurrentUser()
        {
            if (!Request.IsAuthenticated || User == null) return null;

            return usersRepository.Get(x => x.LoginName == User.Identity.Name).FirstOrDefault<UserModel>();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                usersRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}
