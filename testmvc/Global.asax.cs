using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using testmvc.App_Start;
using testmvc.Configuration;
using testmvc.Helpers;
using testmvc.Models;
using testmvc.Repository;

namespace testmvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            AutoMapper.Mapper.CreateMap<UserModel, EditUserViewModel>();

            IContainer container = DependenciesInitializer.RegisterDependencies();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlCacheDependencyAdmin.EnableNotifications(cs);
            SqlCacheDependencyAdmin.EnableTableForNotifications(cs, "UserProfile");
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpRequestWrapper wr = new HttpRequestWrapper(Request);
            string userCulture = CultureHelper.GetCurrentCulture(wr, Settings.DefaultCulture);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(userCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(userCulture);
        }
    }
}