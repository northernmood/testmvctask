using System.Web.Mvc;
using System.Web.Routing;
using testmvc.Configuration;
using testmvc.Filters;
using testmvc.Helpers;
using testmvc.Models;
using testmvc.Repository;
using testmvc.WebSecurityImpl;
using WebMatrix.WebData;

namespace testmvc.Controllers
{
    [InitializeSimpleMembership]
    public abstract class BaseController : Controller
    {
        public IUsersRepository usersRepository { get; set; }
        public IWebSecurityWrapper WebSecurity { get; set; }

        public BaseController(IUsersRepository repository, IWebSecurityWrapper webSecurity)
        {
            usersRepository = repository;
            WebSecurity = webSecurity;
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
