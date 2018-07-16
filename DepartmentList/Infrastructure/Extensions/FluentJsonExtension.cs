using System;
using System.Linq.Expressions;
using FluentJsonNet;

namespace DepartmentList.Infrastructure.Extensions
{
    public static class FluentJsonExtension
    {
        public static JsonMap<TClassType> CreateMap<TClassType, TFieldType>(this JsonMap<TClassType> type, Expression<Func<TClassType, TFieldType>> field, string name)
        {
            type.Map(field, name);
            return type;
        }
    }
}