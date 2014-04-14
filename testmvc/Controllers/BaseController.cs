using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using testmvc.Configuration;
using testmvc.Helpers;
using testmvc.Models;
using System.Linq;

namespace testmvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            ViewBag.Culture = CultureHelper.GetCurrentCulture(requestContext.HttpContext.Request, Config.DefaultCulture);

            base.Initialize(requestContext);
        }

        protected UserModel GetCurrentUser()
        {
            if (!Request.IsAuthenticated || User == null) return null;

            UsersContext context = new UsersContext();

            var q = from Users in context.Users where Users.LoginName == User.Identity.Name select Users;

            return q.FirstOrDefault<UserModel>();
        }
    }
}
