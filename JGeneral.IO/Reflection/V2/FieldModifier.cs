using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace JGeneral.IO.Reflection.V2
{
    /// <summary>
    /// Dynamically modifies fields with reflection with a minimal performance impact.
    /// It is advised not to use this in combination with the struct data type because of boxing issues.
    /// </summary>
    /// <typeparam name="T">The Field's Type.</typeparam>
    /// <typeparam name="TParent">The Parent's Type.</typeparam>
    public class FieldModifier<T, TParent>
    {
        private TParent ParentInstance;
        private readonly FieldInfo Info;
        
        public FieldModifier(TParent parent, string fieldId, bool isPublic = false)
        {
            ParentInstance = parent;
            Info = typeof(TParent).GetField(fieldId, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Instance);
        }

        public T Get()
        {
            return (T) Info.GetValue(ParentInstance);
        }

        public void Set(in T value)
        {
            Info.SetValue(ParentInstance, value);
        }

        public void ChangeParent(in TParent value)
        {
            ParentInstance = value;
        }
    }
}