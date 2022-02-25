using System;
using System.Reflection;

namespace JGeneral.IO.Reflection
{
    public static class ModifierHelper
    {
        public static Modifier<TVariableInfo, TVariableInfoDataType> CreatePublicModifierFrom<TVariableInfo, TVariableInfoDataType>(this Object instance, string variableId)
            where TVariableInfo : MemberInfo
        {
            return new (variableId, instance, BindingFlags.Instance | BindingFlags.Public);
        }
        public static Modifier<TVariableInfo> CreatePublicModifierFrom<TVariableInfo>(this Object instance, string variableId)
            where TVariableInfo : MemberInfo
        {
            return new (variableId, instance, BindingFlags.Instance | BindingFlags.Public);
        }
        public static Modifier<TVariableInfo, TVariableInfoDataType> CreateNonPublicModifierFrom<TVariableInfo, TVariableInfoDataType>(this Object instance, string variableId)
            where TVariableInfo : MemberInfo
        {
            return new (variableId, instance, BindingFlags.Instance | BindingFlags.NonPublic);
        }
        public static Modifier<TVariableInfo> CreateNonPublicModifierFrom<TVariableInfo>(this Object instance, string variableId)
            where TVariableInfo : MemberInfo
        {
            return new (variableId, instance, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static StaticModifier<TVariableInfo, TVariableInfoDataType> CreatePublicStaticModifierFrom<TVariableInfo, TVariableInfoDataType>(this Type t, string variableId) 
            where TVariableInfo : MemberInfo
        {
            return new (t, variableId, BindingFlags.Public | BindingFlags.Static);
        }
        public static StaticModifier<TVariableInfo> CreatePublicStaticModifierFrom<TVariableInfo>(this Type t, string variableId)
            where TVariableInfo : MemberInfo
        {
            return new (t, variableId, BindingFlags.Public | BindingFlags.Static);
        }
        public static StaticModifier<TVariableInfo, TVariableInfoDataType> CreateNonPublicStaticModifierFrom<TVariableInfo, TVariableInfoDataType>(this Type t, string variableId)
            where TVariableInfo : MemberInfo
        {
            return new (t, variableId, BindingFlags.NonPublic | BindingFlags.Static);
        }
        public static StaticModifier<TVariableInfo> CreateNonPublicStaticModifierFrom<TVariableInfo>(this Type t, string variableId)
            where TVariableInfo : MemberInfo
        {
            return new (t, variableId, BindingFlags.NonPublic | BindingFlags.Static);
        }
    }
}