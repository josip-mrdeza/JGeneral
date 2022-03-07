using System;
using System.Linq;
using System.Reflection;

namespace JGeneral.IO.Reflection
{
    public class StaticExecutor<TReturn>
    {
        public MethodInfo Method;

        public StaticExecutor(Type type, string method, BindingFlags flags = BindingFlags.Public | BindingFlags.Static)
        {
            Method = type.GetMethod(method, flags);
        }
        
        /// <summary>
        /// Provides an efficient way to run a static method reflection-wise.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument array used to invoke the method with.</param>
        public TReturn Run(params object[] arg)
        {
            return (TReturn)Method.Invoke(null, arg);
        }
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise with an augmented parameter type.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument of custom type TArg.</param>
        /// <typeparam name="TArg">A custom type for the input parameters of the method.</typeparam>
        public TReturn Run<TArg>(TArg arg)
        {
            return (TReturn)Method.Invoke(null, new object[]{arg});
        }
    }

    public class StaticExecutor
    {
        public MethodInfo Method;

        public StaticExecutor(Type type, string method, BindingFlags flags = BindingFlags.Public | BindingFlags.Static)
        {
            Method = type.GetMethod(method, flags);
        }
        
        /// <summary>
        /// Provides an efficient way to run a static method reflection-wise.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument array used to invoke the method with.</param>
        public void Run(params object[] arg)
        {
            Method.Invoke(null, arg);
        }
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise with an augmented parameter type.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument of custom type TArg.</param>
        /// <typeparam name="TArg">A custom type for the input parameters of the method.</typeparam>
        public void Run<TArg>(TArg arg)
        {
            Method.Invoke(null, new object[]{arg});
        }
    }
}