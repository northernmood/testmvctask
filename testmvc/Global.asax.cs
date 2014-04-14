using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using testmvc.Configuration;
using testmvc.Helpers;

namespace testmvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapper.Mapper.CreateMap<testmvc.Models.UserModel, testmvc.Models.EditUserViewModel>();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpRequestWrapper wr = new HttpRequestWrapper(Request);
            string userCulture = CultureHelper.GetCurrentCulture(wr, Config.DefaultCulture);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(userCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(userCulture);
        }
    }
}