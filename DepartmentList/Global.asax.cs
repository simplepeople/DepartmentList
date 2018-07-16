using System.Data.Entity.SqlServer;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DepartmentList.Domain.Contexts;

namespace DepartmentList
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.ConfigureContainer();
            AutomapperConfig.InitMapping();
            JsonMapperConfig.InitMapping();
            SqlProviderServices.SqlServerTypesAssemblyName ="Microsoft.SqlServer.Types, Version=13.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
            DepartmentContext.InitDb();
        }
    }
}
