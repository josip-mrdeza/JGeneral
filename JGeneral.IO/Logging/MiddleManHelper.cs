using System;
using System.Reflection;
using System.Runtime.InteropServices;
using JGeneral.IO.Reflection;

namespace JGeneral.IO.Logging
{
    public static class MiddleManHelper
    {
        public enum MiddleManType
        {
            Field,
            Property
        }
        
        public static IMiddleMan<T, TData> AddMiddleMan<T, TData>(this T parentInstance, string propertyId, Action<TData> middleman, bool isField)
        {
            return isField switch
            {
                true  => new FieldMiddleMan<T, TData>(parentInstance, propertyId, middleman),
                false => new PropertyMiddleMan<T, TData>(parentInstance, propertyId, middleman)
            };
        }

        public static FieldMiddleMan<T, TData> CastToFieldType<T, TData>(this IMiddleMan<T, TData> instance)
        {
            return (FieldMiddleMan<T, TData>) instance;
        }
        
        public static PropertyMiddleMan<T, TData> CastToPropertyType<T, TData>(this IMiddleMan<T, TData> instance)
        {
            return (PropertyMiddleMan<T, TData>) instance;
        }
    }
}