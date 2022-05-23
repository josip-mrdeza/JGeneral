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
        public object Parent { get; set; }
        public _MemberInfo _info { get; set; }

        public byte cfg { get; }
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
                
                Modify_Cached = o => (_info as FieldInfo)!.SetValue(Parent, o);
                Get_Cached = () => (TVariableInfoData)(_info as FieldInfo)!.GetValue(Parent);
            }
            else
            {
                _info = piT.GetProperty(variableId, flags) as TVariableInfo;
                cfg = 1;   
                
                if (_info is null)
                {
                    throw new Exception("VSD is null, have you misspelled the name of the field, or is the field non-public?");
                }
                
                // if (!(_info as PropertyInfo)!.CanWrite)
                // {
                //     throw new Exception("Cannot instantiate class 'Modifier' on a get-only property.");
                // }
                
                Modify_Cached = o => (_info as PropertyInfo)!.SetValue(Parent, o);
                Get_Cached = () => (TVariableInfoData)(_info as PropertyInfo)!.GetValue(Parent);
            }
        }
        public void Modify(TVariableInfoData value) => Modify_Cached(value);
        public TVariableInfoData Get() => Get_Cached();

        private readonly Action<TVariableInfoData> Modify_Cached;
        private readonly Func<TVariableInfoData> Get_Cached;
    }
    public class Modifier<TVariableInfo> : IModifier<object> where TVariableInfo : MemberInfo
    {
        public object Parent { get; set; }
        public _MemberInfo _info { get; set; }

        public byte cfg { get; }
        
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

                Modify_Cached = o => (_info as FieldInfo)!.SetValue(Parent, o);
                Get_Cached = () => (_info as FieldInfo)!.GetValue(Parent);
            }
            else
            {
                _info = piT.GetProperty(variableId, flags) as TVariableInfo;
                cfg = 1;
                if (!(_info as PropertyInfo)!.CanWrite)
                {
                    throw new Exception("Cannot instantiate class 'Modifier' on a get-only property.");
                }
                
                Modify_Cached = o => (_info as PropertyInfo)!.SetValue(Parent, o);
                Get_Cached = () => (_info as PropertyInfo)!.GetValue(Parent);
            }
        }

        public void Modify(object value) => Modify_Cached(value);
        public object Get() => Get_Cached();

        private readonly Action<object> Modify_Cached;
        private readonly Func<object> Get_Cached;
    }
}