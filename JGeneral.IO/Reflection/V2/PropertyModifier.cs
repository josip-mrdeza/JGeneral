using System;
using System.Reflection;

namespace JGeneral.IO.Reflection.V2
{
    /// <summary>
    /// Dynamically modifies properties with reflection with a minimal performance impact.
    /// It is advised not to use this in combination with the struct data type because of boxing issues.
    /// </summary>
    /// <typeparam name="T">The Field's Type.</typeparam>
    /// <typeparam name="TParent">The Parent's Type.</typeparam>
    public class PropertyModifier<T, TParent>
    {
        private TParent ParentInstance;
        private readonly PropertyInfo Info;
        private readonly Func<TParent, T> GetPropertyMethod;
        private readonly Action<TParent, T> SetPropertyMethod;

        public PropertyModifier(TParent parent, string fieldId, bool isPublic = false)
        {
            ParentInstance = parent;
            Info = typeof(TParent).GetProperty(fieldId, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Instance);
            GetPropertyMethod = (Func<TParent, T>)
                Delegate.CreateDelegate(typeof(Func<TParent, T>),
                                        Info!.GetGetMethod(!isPublic));
            SetPropertyMethod = (Action<TParent, T>)
                Delegate.CreateDelegate(typeof(Action<TParent, T>),
                                        Info!.GetSetMethod(!isPublic));
        }

        public T Get()
        {
            return GetPropertyMethod(ParentInstance);
        }

        public void Set(in T value)
        {
            SetPropertyMethod(ParentInstance, value);
        }
        
        public void ChangeParent(in TParent value)
        {
            ParentInstance = value;
        }
    }
}