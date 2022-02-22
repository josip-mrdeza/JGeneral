using System.Reflection;

namespace JGeneral.IO.Reflection
{
    public class Executor<TParent, TReturn>
    {
        public MethodInfo Method;
        public TParent Parent;
        public Executor(string method, TParent parentInstance)
        {
            Parent = parentInstance;
            Method = typeof(TParent).GetMethod(method);
        }
        
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument array used to invoke the method with.</param>
        /// <returns>The method result of type <typeparamref name="TReturn"/>.</returns>
        public TReturn Run(params object[] arg)
        {
            return (TReturn)Method.Invoke(Parent, arg);
        }
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise with an augmented parameter type.
        /// Note that values might be boxed in the case that they are structs.

        /// </summary>
        /// <param name="arg">An argument of custom type TArg.</param>
        /// <typeparam name="TArg">A custom type for the input parameters of the method.</typeparam>
        /// <returns>The method result of type <typeparamref name="TReturn"/>.</returns>
        public TReturn Run<TArg>(TArg arg)
        {
            return (TReturn)Method.Invoke(Parent, new object[]{arg});
        }
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise with an augmented return type.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument array used to invoke the method with.</param>
        /// <typeparam name="TAltReturn">An alternate return type.</typeparam>
        /// <returns>The method result of custom type <typeparamref name="TAltReturn"/>.</returns>
        public TAltReturn Run<TAltReturn>(params object[] arg)
        {
            return (TAltReturn)Method.Invoke(Parent, arg);
        }
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise with augmented return and parameter types.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument of custom type TArg.</param>
        /// <typeparam name="TAltReturn">An alternate return type.</typeparam>
        /// <typeparam name="TArg">A custom type for the input parameters of the method.</typeparam>
        /// <returns>The method result of custom type <typeparamref name="TAltReturn"/>.</returns>
        public TAltReturn Run<TAltReturn, TArg>(TArg arg)
        {
            return (TAltReturn)Method.Invoke(Parent, new object[]{arg});
        }
    }
    public class Executor<TReturn>
    {
        public MethodInfo Method;
        public object Parent;
        
        private Executor(string method, object parentInstance)
        {
            Parent = parentInstance;
            Method = parentInstance.GetType().GetMethod(method);
        }
        
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument array used to invoke the method with.</param>
        public TReturn Run(params object[] arg)
        {
            return (TReturn)Method.Invoke(Parent, arg);
        }
        
        /// <summary>
        /// Provides an efficient way to run a method reflection-wise with an augmented parameter type.
        /// Note that values might be boxed in the case that they are structs.
        /// </summary>
        /// <param name="arg">An argument of custom type TArg.</param>
        /// <typeparam name="TArg">A custom type for the input parameters of the method.</typeparam>
        public TReturn Run<TArg>(TArg arg)
        {
            return (TReturn)Method.Invoke(Parent, new object[]{arg});
        }
    }
}