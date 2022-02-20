using System.Reflection;
using System.Runtime.InteropServices;

namespace JGeneral.IO.Reflection
{
    public interface IModifier<TVariableInfoData>
    {
        public object Parent { get; }
        public _MemberInfo _info { get;}
        public byte cfg { get; }
        
        public TVariableInfoData Get();
        public void Modify(TVariableInfoData value);
    }
}