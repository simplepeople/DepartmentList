using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DepartmentList.Infrastructure.Extensions
{
    public static class ReflectiveEnumerator
    {
        public static IEnumerable<T> GetInstanceOfSubclasses<T>(bool exceptBaseAssembly) where T : class
        {
            return GetSubclasses<T>(exceptBaseAssembly).Select(type => (T)Activator.CreateInstance(type)).ToList();
        }

        public static IEnumerable<Type> GetSubclasses<T>(bool exceptBaseAssembly) where T : class
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !exceptBaseAssembly || x != typeof(T).Assembly);
            var types = assemblies.SelectMany(x =>x.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))));
            return types;
        }

        public static IEnumerable<Type> GetClassesWithInterface<TInterface>(bool exceptBaseAssembly)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !exceptBaseAssembly || x != typeof(TInterface).Assembly);
            var types = assemblies.SelectMany(x => x.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.GetInterfaces().Any(y => y == typeof(TInterface))));
            return types;
        }

        public static IEnumerable<MethodInfo> GetGenericMethods<TObject>(string methodName) where TObject : class
        {
            return typeof(TObject).GetMethods().Where(x => x.Name == methodName && x.IsGenericMethod);
        }

        public static MethodInfo GetCallerMethod()
        {
            return new StackFrame(3, false).GetMethod() as MethodInfo;
        }
    }
}