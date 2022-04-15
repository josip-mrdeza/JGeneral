using System;
using System.Collections.Concurrent;
using System.Threading;
using JGeneral.IO.Logging;

namespace JGeneral.IO.Threading
{
    public class SyncThread<TAction> where TAction : MulticastDelegate
    {
        public readonly string Id;

        public SyncThreadState State
        {
            get;
            private set;
        }
        private readonly Thread thread;
        private readonly ConsoleLogger _logger;
        private readonly string ReferenceObjectId;
        private bool IsCompleted;
        private readonly BlockingCollection<Message<TAction>> Collection;
        private BlockingCollection<Message<TAction>> collection
        {
            get
            {
                lock (Id)
                {
                    return Collection;
                }
            }
        }
        internal SyncThread(string id)
        {
            try
            {
                Collection = new BlockingCollection<Message<TAction>>();
                _logger = new ConsoleLogger();
                thread = new Thread(() => ProcessInfo());
                thread.IsBackground = true;
                thread.Start();
                Id = id ?? thread.Name;
                SetIdle();
            }
            catch (Exception e)
            {
                SetError();
                _logger.Log(e, nameof(SyncThread<TAction>), "Constructor_0");
            }
        }

        internal SyncThread(string id, ConsoleLogger logger)
        {
            try
            {
                Collection = new BlockingCollection<Message<TAction>>();
                _logger = logger ?? new ConsoleLogger();
                thread = new Thread(() => ProcessInfo());
                thread.IsBackground = true;
                thread.Start();
                Id = id ?? thread.Name;
                SetIdle();
            }
            catch (Exception e)
            {
                SetError();
                _logger.Log(e, nameof(SyncThread<TAction>), "Constructor_1");
            }
        }

        private void ProcessInfo()
        {
            while (true)
            {
                try
                {
                    SetIdle();
                    var message = Collection.Take();
                    SetWorking();
                    OnTake?.Invoke(Id);
                    message.Execute();
                }
                catch (Exception e)
                {
                    SetError();
                    _logger.Log(e.InnerException ?? e, nameof(SyncThread<TAction>), nameof(ProcessInfo));
                    OnExceptionOccured?.Invoke(Id, e);
                }
            } 
        }

        public void TryExecuteItemWait(Message<TAction> item)
        {
            Collection.Add(item);
            item.Wait();
        }
        
        public void TryExecuteItem(Message<TAction> item)
        {
            Collection.Add(item);
        }

        public void TryExecuteItemWait(TAction item)
        {
            Message<TAction> message = item;
            Collection.Add(message);
            message.Wait();
        }
        
        public void TryExecuteItem(TAction item)
        {
            Collection.Add(item);
        }
        
        private void SetWorking()
        {
            State = SyncThreadState.Working;
        }
        private void SetIdle()
        {
            State = SyncThreadState.Idle;
        }
        private void SetOffline()
        {
            State = SyncThreadState.Offline;
        }

        private void SetError()
        {
            State = SyncThreadState.Threw;
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