using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace JGeneral.IO.Reflection
{
    public class Modifier<TVariableInfo, TVariableInfoData> : IModifier<TVariableInfoData> where TVariableInfo : MemberInfo
    {
        public Modifier(string variableId, object parentInstance, BindingFlags flags)
        {
            Parent = parentInstance;
            var piT = Parent.GetType();
            if (typeof(TVariableInfo) == typeof(FieldInfo))
            {
                _info = piT.GetField(variableId, flags) as TVariableInfo;
                cfg = 0;
                if (_info is null)
                {
                    throw new Exception("VSD is null, have you misspelled the name of the field, or is the field non-public?");
                }
            }
            else
            {
                _info = piT.GetProperty(variableId, flags) as TVariableInfo;
                cfg = 1;
                if (!(_info as PropertyInfo)!.CanWrite)
                {
                    throw new Exception("Cannot instantiate class 'Modifier' on a get-only property.");
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
                    (_info as FieldInfo)!.SetValue(Parent, value);
                    break;
                }
                case 1:
                {
                    (_info as PropertyInfo)!.SetValue(Parent, value);
                    break;  
                }
            }
        }

        public object Parent { get; set; }
        public _MemberInfo _info { get; set; }

        public byte cfg { get; }
        public TVariableInfoData Get()
        {
            return cfg switch
            {
                0 => (TVariableInfoData) ((_info as FieldInfo)!.GetValue(Parent)),
                1 => (TVariableInfoData) ((_info as PropertyInfo)!.GetValue(Parent)),
                _ => throw new NotImplementedException("A strange error has occured.")
            };
        }
    }
    public class Modifier<TVariableInfo> : IModifier<object> where TVariableInfo : MemberInfo
    {
        public Modifier(string variableId, object parentInstance, BindingFlags flags)
        {
            Parent = parentInstance;
            var piT = Parent.GetType();
            if (typeof(TVariableInfo) == typeof(FieldInfo))
            {
                _info = piT.GetField(variableId, flags) as TVariableInfo;
                cfg = 0;
                if (_info is null)
                {
                    throw new Exception("VSD is null, have you misspelled the name of the field, or is the field non-public?");
                }
            }
            else
            {
                _info = piT.GetProperty(variableId, flags) as TVariableInfo;
                cfg = 1;
                if (!(_info as PropertyInfo)!.CanWrite)
                {
                    throw new Exception("Cannot instantiate class 'Modifier' on a get-only property.");
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
                    (_info as FieldInfo)!.SetValue(Parent, value);
                    break;
                }
                case 1:
                {
                    (_info as PropertyInfo)!.SetValue(Parent, value);
                    break;  
                }
            }
        }

        public object Parent { get; set; }
        public _MemberInfo _info { get; set; }

        public byte cfg { get; }
        public object Get()
        {
            return cfg switch
            {
                0 => ((_info as FieldInfo)!.GetValue(Parent)),
                1 => ((_info as PropertyInfo)!.GetValue(Parent)),
                _ => throw new NotImplementedException($"Cannot read member as {_info.GetType().FullName}.")
            };
        }
    }
}