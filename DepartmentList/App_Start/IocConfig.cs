using System.Linq;
using System.Web.Http;
using DepartmentList.Infrastructure.Extensions;
using DepartmentList.Infrastructure.IoC;
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

            var mi = ReflectiveEnumerator.GetGenericMethods<ServiceContainer>(nameof(ServiceContainer.Register))
                .Where(x => x.GetParameters().Length == 0 && x.GetGenericArguments().Length == 1).ToList();
            foreach (var type in ReflectiveEnumerator.GetClassesWithInterface<IDependency>(false))
                mi.First().MakeGenericMethod(type).Invoke(container, null);
        }
    }
}