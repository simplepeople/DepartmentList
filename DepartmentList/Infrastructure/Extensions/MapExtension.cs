using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace DepartmentList.Infrastructure.Extensions
{
    public static class MapExtension
    {
        private static MethodInfo _mapMethodInfo;
        private static MethodInfo MapMethodInfo
        {
            get
            {
                if (_mapMethodInfo == null)
                    _mapMethodInfo = ReflectiveEnumerator.GetGenericMethods<Mapper>(nameof(Mapper.Map)).First();
                return _mapMethodInfo;
            }
        }

        private static Dictionary<Type, MethodInfo> Methods { get; set; }

        static MapExtension()
        {
            Methods = new Dictionary<Type, MethodInfo>();
        }

        public static dynamic Map(this object source)
        {
            var type = ReflectiveEnumerator.GetCallerMethod().ReturnType;
            if (!Methods.TryGetValue(type, out var mi))
            {
                mi = MapMethodInfo.MakeGenericMethod(type);
                Methods[type] = mi;
            }
            return mi.Invoke(Mapper.Instance, new []{source});
        }
    }
}