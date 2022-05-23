using System;
using System.Reflection;
using JGeneral.IO.Reflection;

namespace JGeneral.IO.Logging
{
    public class PropertyMiddleMan<T, TData> : IMiddleMan<T, TData>
    {
        public Modifier<FieldInfo, TData> ModifierField { get; }
        public Modifier<PropertyInfo, TData> ModifierProperty { get; }
        public T Instance { get; }
        public Func<TData> MiddleManGet { get; }
        public Action<TData> MiddleManSet { get; }

        public TData Data
        {
            get => MiddleManGet.Invoke();
            set => MiddleManSet.Invoke(value);
        }

        internal PropertyMiddleMan(T instance, string propertyId, Action<TData> middleman)
        {
            Instance = instance;
            ModifierProperty = Instance.CreatePublicModifierFrom<PropertyInfo, TData>(propertyId);
            
            MiddleManGet = () =>
            {
                var result = ModifierProperty.Get();
                middleman.Invoke(result);

                return result;
            };
            
            MiddleManSet = value =>
            {
                ModifierProperty.Modify(value);
                middleman.Invoke(value);
            };
        }
    }
}