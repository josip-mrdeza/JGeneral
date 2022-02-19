using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JGeneral.IO.Database
{
    public class DbQueue
    {
        private List<Action> _arr;
        public long Count { get => _arr.Count; }
        /// <summary>
        /// Gets the currently executing <see cref="MulticastDelegate"/>.
        /// </summary>
        public Action Current { get; private set; }
        /// <summary>
        /// Creates a new queue.
        /// </summary>
        /// <param name="length">The length of the queue.</param>
        public DbQueue(int length)
        {
            _arr = new List<Action>(length);
            Setup();
        }
        /// <summary>
        /// Creates a new queue.
        /// </summary>
        public DbQueue()
        {
            _arr = new List<Action>();
        }
        /// <summary>
        /// Queues up a new <see cref="Action"/> to the internal list.
        /// </summary>
        /// <param name="method">The <see cref="Action"/> to queue.</param>
        /// <param name="methodName">The private method name to assign to the anonymous <see cref="Action"/>.</param>
        /// <exception cref="NotImplementedException">Throws if the provided <see cref="Action"/> is null.</exception>
        public async Task QueueUp(Action method, string methodName)
        {
            if (method is null)
            {
                throw new NotImplementedException("Can't queue null action.");
            }

            await Task.Run(() =>
            {
                _arr.Add(method);
                Current = method;
                Enqueued?.Invoke(method, methodName);
                Current = null;
            });
        }
        /// <summary>
        /// Queues up a new <see cref="Func{T}"/> to the internal list.
        /// </summary>
        /// <param name="work">The <see cref="Func{T}"/> to queue.</param>
        /// <param name="methodName">The private method name to assign to the anonymous <see cref="Func{T}"/>.</param>
        /// <exception cref="NotImplementedException">Throws if the provided <see cref="Func{T}"/> is null.</exception>
        public async Task<T> QueueUp<T>(Func<T> work, string methodName)
        {
            if (work is null)
            {
                throw new NotImplementedException("Can't queue null method.");
            }

            return await Task.Run(new Func<T>(() =>
            {
                _arr.Add(DefaultAction);
                Current = DefaultAction;
                var result = work.Invoke();
                Dequeued.Invoke(DefaultAction, methodName);
                Current = null;
                return result;
            }));
        }

        private void Setup()
        {
            Enqueued += (action, str) =>
            {
                action.Invoke();
                Dequeued?.Invoke(action, str);
            };
            Dequeued += (action, str) =>
            {
                if (action is not null)
                {
                    _arr.Remove(action);
                }
            };
        }
        public event Action<Action, string> Enqueued;
        public event Action<Action, string> Dequeued;
        private static Action DefaultAction = new Action(() => { });
    }
}