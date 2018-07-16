using System.Web.Http;
using DepartmentList.Domain;
using DepartmentList.Domain.Contexts;
using DepartmentList.Domain.EntityServices;
using DepartmentList.Services;
using LightInject;

namespace DepartmentList
{
    public class IocConfig
    {
        public static void ConfigureContainer()
        {
            var container = new ServiceContainer();
            container.RegisterApiControllers();
            container.EnablePerWebRequestScope();
            container.EnableWebApi(GlobalConfiguration.Configuration);
            container.Register<DepartmentEntityService>()
                .Register<DepartmentContext>()
                .Register<DepartmentService>();
        }
    }
}