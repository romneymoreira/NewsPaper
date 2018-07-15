using ProjetoJornal.Base;
using ProjetoJornal.Context;
using ProjetoJornal.Controllers;
using ProjetoJornal.Repository;
using ProjetoJornal.Repository.Interface;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProjetoJornal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            // Register your types, for instance:
            container.Register<ISiteRepository, SiteRepository>(Lifestyle.Scoped);
            container.Register<ILoginService, LoginService>(Lifestyle.Scoped);
            container.Register<IDashboardRepository, DashboardRepository>(Lifestyle.Scoped);
            container.Register<SiteContext>(Lifestyle.Scoped);
            container.Register(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifestyle.Scoped);
            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (MvcApplication)sender;
            var context = app.Context;
            var ex = app.Server.GetLastError();
            context.Response.Clear();
            context.ClearError();

            var httpException = ex as HttpException;

            var routeData = new RouteData();
            routeData.Values["controller"] = "errors";
            routeData.Values["exception"] = ex;
            routeData.Values["action"] = "http500";

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values["action"] = "http404";
                        break;
                    case 500:
                        routeData.Values["action"] = "http500";
                        break;
                }
            }

            IController controller = new ErrorsController();
            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }
    }
}
