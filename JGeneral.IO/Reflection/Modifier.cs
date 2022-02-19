using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JGeneral.IO.Reflection
{
    public static class ModifierExtensions
    {
        public static Modifier CreateModifierFrom(this Object instance, string variableId)
        {
            return new (variableId, instance);
        }

        public static Modifier<TVariableInfo, TVariableInfoDataType, TParent> CreateModifierFrom<TVariableInfo, TVariableInfoDataType,
            TParent>(this object instance, string variableId) where TParent : class where TVariableInfo : MemberInfo
        {
            return new (variableId, (TParent)instance);
        }
    }
    public class Modifier<TVariableInfo, TVariableInfoData, TParent> where TVariableInfo : MemberInfo where TParent : class
    {
        public readonly TParent Parent;
        public readonly TVariableInfo VariableStartData;
        private byte cfg;
        
        public Modifier(string variableId, TParent parentInstance)
        {
            Parent = parentInstance;
            if (typeof(TVariableInfo) == typeof(FieldInfo))
            {
                VariableStartData = typeof(TParent).GetField(variableId) as TVariableInfo;
                cfg = 0;
                if (VariableStartData is null)
                {
                    throw new Exception("VSD is null, have you misspelled the name of the field, or is the field non-public?");
                }
            }
            else
            {
                VariableStartData = typeof(TParent).GetProperty(variableId) as TVariableInfo;
                cfg = 1;
                if (!(VariableStartData as PropertyInfo)!.CanWrite)
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
                    (VariableStartData as FieldInfo)!.SetValue(Parent, value);
                    break;
                }
                case 1:
                {
                    (VariableStartData as PropertyInfo)!.SetValue(Parent, value);
                    break;  
                }
            }
        }
    }
    public class Modifier<TVariableInfo, TVariableInfoData> where TVariableInfo : MemberInfo
    {
        public readonly object Parent;
        public readonly TVariableInfo VariableStartData;
        private byte cfg;
        public Modifier(string variableId, object parentInstance)
        {
            Parent = parentInstance;
            var piT = Parent.GetType();
            if (typeof(TVariableInfo) == typeof(FieldInfo))
            {
                VariableStartData = piT.GetField(variableId) as TVariableInfo;
                cfg = 0;
                if (VariableStartData is null)
                {
                    throw new Exception("VSD is null, have you misspelled the name of the field, or is the field non-public?");
                }
            }
            else
            {
                VariableStartData = piT.GetProperty(variableId) as TVariableInfo;
                cfg = 1;
                if (!(VariableStartData as PropertyInfo)!.CanWrite)
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
                    (VariableStartData as FieldInfo)!.SetValue(Parent, value);
                    break;
                }
                case 1:
                {
                    (VariableStartData as PropertyInfo)!.SetValue(Parent, value);
                    break;  
                }
            }
        }
    }
    public class Modifier
    {
        public readonly object Parent;
        public readonly object VariableStartData;
        private byte cfg;
        
        public Modifier(string variableId, object parentInstance)
        {
            Parent = parentInstance;
            var piT = Parent.GetType();
            VariableStartData = piT.GetField(variableId);
            if (VariableStartData != null)
            {
                cfg = 0;
            }
            else
            {
                VariableStartData = piT.GetProperty(variableId);
                cfg = 1;
                if (!(VariableStartData as PropertyInfo)!.CanWrite)
                {
                    throw new Exception("Cannot instantiate class 'Modifier' on a get-only property.");
                }
            }
        }
        /// <param name="value">The new value of type TVariableInfoData</param>
        public void Modify<TVariableInfoData>(TVariableInfoData value)
        {
            switch (cfg)
            {
                case 0:
                {
                    (VariableStartData as FieldInfo)!.SetValue(Parent, value);
                    break;
                }
                case 1:
                {
                    (VariableStartData as PropertyInfo)!.SetValue(Parent, value);
                    break;  
                }
            }
        } 
    }
}