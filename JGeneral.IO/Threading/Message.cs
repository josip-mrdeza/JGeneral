using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace JGeneral.IO.Threading
{
    public class Message<TAction> where TAction : MulticastDelegate
    {
        public TAction Function;
        public object[] Objs;
        public CancellationTokenSource TokenSource;

        public CancellationToken Token
        {
            get => TokenSource.Token;
        }

        public Message(TAction function, params object[] objs)
        {
            Function = function;
            Objs = objs;
            TokenSource = new CancellationTokenSource();
        }

        public void Execute()
        {
            Function.DynamicInvoke(Objs);
            TokenSource.Cancel();
        }

        public void Wait()
        {
            Token.WaitHandle.WaitOne();
        }

        public void RenewToken()
        {
            TokenSource = new CancellationTokenSource();
        }

        public static implicit operator Message<TAction>(TAction action)
        {
            return new(action);
        }
    }
}