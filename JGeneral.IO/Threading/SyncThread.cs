using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using JGeneral.IO.Logging;

namespace JGeneral.IO.Threading
{
    public class SyncThread<TAction> where TAction : MulticastDelegate
    {
        public static readonly Context MainContext = Context.DefaultContext;
        public static event Action<object> OnCompleteMainThreadTask;
        private readonly Thread thread;
        private readonly string Id;
        private readonly ConsoleLogger _logger;
        private readonly string ReferenceObjectId;
        private readonly BlockingCollection<Message<TAction>> _collection;
        private bool IsCompleted;
        public SyncThreadState State
        {
            get;
            private set;
        }

        public SyncThread(Action threadStart = null)
        {
            try
            {
                _collection = new BlockingCollection<Message<TAction>>();
                _logger = new ConsoleLogger();
                thread = new Thread(() =>
                {
                    threadStart?.DynamicInvoke();
                    ProcessInfo();
                });
                thread.IsBackground = true;
                thread.Start();
                Id = MainContext.ContextID + '_' + thread.Name;
                SetIdle();
            }
            catch (Exception e)
            {
                SetError();
                _logger.Log(e, nameof(SyncThread<TAction>), "Constructor_0");
            }
        }

        private void ProcessInfo()
        {
            while (true)
            {
                try
                {
                    SetIdle();
                    var message = _collection.Take();
                    SetWorking();
                    OnTake?.Invoke(Id);
                    message.Execute();
                }
                catch (Exception e)
                {
                    SetError();
                    _logger.Log(e.InnerException ?? e, nameof(SyncThread<TAction>), nameof(ProcessInfo));
                }
            } 
        }
        public void TryExecuteItem(Message<TAction> item, SyncThreadMode threadMode = SyncThreadMode.Current, bool waitTilComplete = false)
        {
            item.ThreadMode = threadMode;
            _collection.Add(item);

            if (waitTilComplete)
            {
                item.Wait();
            }
        }
        public void TryExecuteItem(TAction item, SyncThreadMode threadMode = SyncThreadMode.Current, bool waitTilComplete = false)
        {
            TryExecuteItem((Message<TAction>) item, threadMode, waitTilComplete);
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
    }
}