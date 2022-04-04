using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using JGeneral.IO.Logging;
using JGeneral.IO.Reflection.V2;

namespace JGeneral.IO.Threading
{
    public class SyncThread<TAction> where TAction : MulticastDelegate
    {
        public readonly string Id;
        private readonly BlockingCollection<Message<TAction>> Collection;
        private readonly Thread thread;
        private readonly ConsoleLogger _logger;
        private readonly string ReferenceObjectId;
        private bool IsCompleted;
        public SyncThread(string id)
        {
            Id = id;
            Collection = new BlockingCollection<Message<TAction>>();
            _logger = new ConsoleLogger();
            thread = new Thread(() => ProcessInfo());
            thread.IsBackground = true;
            thread.Start();
        }

        public SyncThread(string id, ConsoleLogger logger)
        {
            Id = id;
            Collection = new BlockingCollection<Message<TAction>>();
            _logger = logger;
            thread = new Thread(() => ProcessInfo());
            thread.IsBackground = true;
            thread.Start();
        }

        private void ProcessInfo()
        {
            while (true)
            {
                try
                {
                    var message = Collection.Take();
                    OnTake?.Invoke(Id);
                    message.Execute();
                }
                catch (Exception e)
                {
                    _logger.Log(e.InnerException ?? e, nameof(SyncThread<TAction>), nameof(ProcessInfo));
                    OnExceptionOccured?.Invoke(Id, e);
                }
            } 
        }

        public void TryExecuteItem(Message<TAction> item)
        {
            Collection.Add(item);
            item.Wait();
        }
        
        /// <summary>
        /// Represents an event that is called whenever a <see cref="SyncThread{TAction}"/> takes an item from the <see cref="BlockingCollection{T}"/>.
        /// The input string parameter represents the calling thread's id.
        /// </summary>
        public static event Action<string> OnTake;
        /// <summary>
        /// Represents an event that is called whenever a <see cref="SyncThread{TAction}"/> throws an exception in it's processing method '<see cref="ProcessInfo"/>'.
        /// The input string parameter represents the calling thread's id.
        /// </summary>
        public static event Action<string, Exception> OnExceptionOccured;
    }
}