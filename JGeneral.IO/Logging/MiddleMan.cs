using System;
using System.Reflection;
using JGeneral.IO.Reflection;

namespace JGeneral.IO.Logging
{
    public interface IMiddleMan<out T, TData>
    {
        public Modifier<FieldInfo, TData> ModifierField { get; }
        public Modifier<PropertyInfo, TData> ModifierProperty { get; }
        public T Instance { get; }
        public Func<TData> MiddleManGet { get; }
        public Action<TData> MiddleManSet { get; }
        public TData Data { get; set; }
    }
}