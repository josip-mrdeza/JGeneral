using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JGeneral.IO.Database
{
    public class JWorker
    {
        private int _maxQueue;
        private DbQueue _queue;
        public DbQueue Queue { get => _queue; }
        /// <summary>
        /// Checks if the worker is suitable for usage at the current state.
        /// </summary>
        public bool Suitable
        {
            get => _queue.Count < 5;
        }
        /// <summary>
        /// Returns the current length of the queue according to the <see cref="Queue"/>.
        /// </summary>
        public long CurrentQueueLength
        {
            get => _queue.Count;
        }
        /// <summary>
        /// Creates a new instance of a <see cref="JWorker"/> along with it's queue.
        /// </summary>
        /// <param name="maxQueue">The maximum number of simultaneous operations on this worker at any given moment.</param>
        public JWorker(int maxQueue = 20)
        {
            _maxQueue = maxQueue;
            _queue = new DbQueue(maxQueue);
        }
        /// <summary>
        /// Assigns an <see cref="Action"/> to the <see cref="JWorker"/> with it's private name for logging purposes.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be executed.</param>
        /// <param name="methodName">A private name of the anonymous method used for logging purposes only.</param>
        public void Assign(Action action, string methodName = "[Anonymous_Method]")
        {
            _queue.QueueUp(action, methodName).Wait();
        }
        /// <summary>
        /// Assigns an <see cref="Func{T1}"/> to the <see cref="JWorker"/> with it's private name for logging purposes.
        /// </summary>
        /// <param name="work">The <see cref="Func{T1}"/> to be executed.</param>
        /// <param name="methodName">A private name of the anonymous method used for logging purposes only.</param>
        public async Task<T> Assign<T>(Func<T> work, string methodName = "[Anonymous_Method]")
        {
            return await _queue.QueueUp(work, methodName);
        }
    }
}