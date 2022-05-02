using System;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace JGeneral.IO.Threading
{
    public class Message<TAction> where TAction : MulticastDelegate
    {
        public TAction Function;
        public object[] Objs;
        public CancellationTokenSource TokenSource;

        public object Result
        {
            get;
            private set;
        }

        public SyncThreadMode ThreadMode
        {
            get;
            set;
        }

        public CancellationToken Token
        {
            get => TokenSource.Token;
        }

        public Message(TAction function, params object[] objs)
        {
            Function = function;
            Objs = objs;
            TokenSource = new CancellationTokenSource();
            ThreadMode = SyncThreadMode.Current;
        }
        
        public Message(TAction function, SyncThreadMode threadMode, params object[] objs)
        {
            Function = function;
            Objs = objs;
            TokenSource = new CancellationTokenSource();
            ThreadMode = threadMode;
        }

        public void Execute()
        {
            Result = Function.DynamicInvoke(Objs);
            TokenSource.Cancel();
        }

        public void Wait()
        {
            Token.WaitHandle.WaitOne();
        }

        public void RenewToken()
        {
            TokenSource = new CancellationTokenSource();
            Result = null;
        }

        public static implicit operator Message<TAction>(TAction action)
        {
            return new(action, SyncThreadMode.Current);
        }
    }
}