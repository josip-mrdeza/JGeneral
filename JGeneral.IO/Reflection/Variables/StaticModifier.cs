using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace JGeneral.IO.Reflection
{
    public class StaticModifier<TVariableInfo, TVariableInfoData> : IModifier<TVariableInfoData> where TVariableInfo : MemberInfo
    {
        public object Parent { get => null; }
        public _MemberInfo _info { get; private set; }
        public byte cfg { get; private set; }

        internal StaticModifier(Type t, string variableId, BindingFlags flags)
        {
            if (typeof(TVariableInfo) == typeof(FieldInfo))
            {
                _info = t.GetField(variableId, flags) as TVariableInfo;
                cfg = 0;
                if (_info is null)
                {
                    throw new Exception("VSD is null, have you misspelled the name of the field, or is the field non-public?");
                }
            }
            else
            {
                _info = t.GetProperty(variableId, flags) as TVariableInfo;
                cfg = 1;
                if (!(_info as PropertyInfo)!.CanWrite)
                {
                    throw new Exception("Cannot instantiate class 'StaticModifier' on a get-only property.");
                }
            }
        }

        /// <param name="value">The new value of type TVariableInfoData</param>
        public void Modify(TVariableInfoData value)
        {
            switch (cfg)
            {
                case 0:
                {
                    (_info as FieldInfo)!.SetValue(null, value);
                    break;
                }
                case 1:
                {
                    (_info as PropertyInfo)!.SetValue(null, value);
                    break;  
                }
            } 
        }

        public TVariableInfoData Get()
        {
            return cfg switch
            {
                0 => (TVariableInfoData) ((_info as FieldInfo)!.GetValue(null)),
                1 => (TVariableInfoData) ((_info as PropertyInfo)!.GetValue(null)),
                _ => throw new NotImplementedException($"Cannot read member as {_info.GetType().FullName}.")
            };
        }
    }
    
    public class StaticModifier<TVariableInfo> : IModifier<object> where TVariableInfo : MemberInfo
    {
        public object Parent { get => null; }
        public _MemberInfo _info { get; private set; }
        public byte cfg { get; private set; }

        internal StaticModifier(Type t, string variableId, BindingFlags flags)
        {
            if (typeof(TVariableInfo) == typeof(FieldInfo))
            {
                _info = t.GetField(variableId, flags) as TVariableInfo;
                cfg = 0;
                if (_info is null)
                {
                    throw new Exception("VSD is null, have you misspelled the name of the field, or is the field non-public?");
                }
            }
            else
            {
                _info = t.GetProperty(variableId, flags) as TVariableInfo;
                cfg = 1;
                if (!(_info as PropertyInfo)!.CanWrite)
                {
                    throw new Exception("Cannot instantiate class 'StaticModifier' on a get-only property.");
                }
            }
        }

        /// <param name="value">The new value of type TVariableInfoData</param>
        public void Modify(object value)
        {
            switch (cfg)
            {
                case 0:
                {
                    (_info as FieldInfo)!.SetValue(null, value);
                    break;
                }
                case 1:
                {
                    (_info as PropertyInfo)!.SetValue(null, value);
                    break;  
                }
            } 
        }

        public object Get()
        {
            return cfg switch
            {
                0 => ((_info as FieldInfo)!.GetValue(null)),
                1 => ((_info as PropertyInfo)!.GetValue(null)),
                _ => throw new NotImplementedException($"Cannot read member as {_info.GetType().FullName}.")
            };
        }
    }
}