using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testmvc.Controllers;
using testmvc.Repository;
using testmvc.WebSecurityImpl;

namespace testmvc.App_Start
{
    public class DependenciesInitializer
    {
        public static IContainer RegisterDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<UsersRepository>().As<IUsersRepository>();
            builder.RegisterType<WebSecurityWrapper>().As<IWebSecurityWrapper>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            return builder.Build();
        }
    }
}