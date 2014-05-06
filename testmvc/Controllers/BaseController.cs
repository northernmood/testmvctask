using System.Web.Mvc;
using System.Web.Routing;
using testmvc.Configuration;
using testmvc.Filters;
using testmvc.Helpers;
using testmvc.Models;
using testmvc.Repository;
using WebMatrix.WebData;

namespace testmvc.Controllers
{
    [InitializeSimpleMembership]
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

        [Authorize]
        protected UserModel GetCurrentUser()
        {
            return usersRepository.GetByID(WebSecurity.CurrentUserId);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                usersRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}
