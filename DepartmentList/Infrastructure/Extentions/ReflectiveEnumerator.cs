using System;
using System.Collections.Generic;
using System.Linq;

namespace DepartmentList.Infrastructure.Extentions
{
    public static class ReflectiveEnumerator
    {
        public static IEnumerable<T> GetInstanceOfSubclasses<T>(bool exceptBaseAssembly) where T : class
        {
            return GetSubclasses<T>(exceptBaseAssembly).Select(type => (T)Activator.CreateInstance(type)).ToList();
        }

        public static IEnumerable<Type> GetSubclasses<T>(bool exceptBaseAssembly) where T : class
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(x => !exceptBaseAssembly || x != typeof(T).Assembly)
                .SelectMany(x => x.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))));
        }
    }
}