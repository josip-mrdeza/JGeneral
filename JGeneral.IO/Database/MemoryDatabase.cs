using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;

namespace JGeneral.IO.Database
{
    public class MemoryDatabase : IDatabase
    {
        private readonly Random _random = new Random();
        private NamedPipeServerStream _pipe;
        /// <summary>
        /// All currently active <see cref="JWorker"/>s.
        /// </summary>
        public JWorker[] DbWorkers { get; protected set; }
        /// <summary>
        /// Past events loaded from <see cref="JWorker"/>s.
        /// </summary>
        public List<string> PastEvents { get; protected set; }
        public DbMemoryStorage MemoryStorage { get; protected set; }
        /// <summary>
        /// Creates a new instance of a <see cref="MemoryDatabase"/>.
        /// This database stores it's data in <see cref="MemoryStorage"/>.
        /// </summary>
        /// <param name="workerThreads"></param>
        public MemoryDatabase(int workerThreads = 4)
        {
            DbWorkers = new JWorker[workerThreads];
            PastEvents = new List<string>();
            MemoryStorage = new DbMemoryStorage();
            for (int i = 0; i < workerThreads; i++)
            {
                DbWorkers[i] = new JWorker();
                DbWorkers[i].Queue.Dequeued += (action, s) =>
                {
                    PastEvents.Add(s);
                };
            }
        }
        /// <summary>
        /// Assigns the work <see cref="Action"/> to a <see cref="JWorker"/>.
        /// </summary>
        /// <param name="work">The <see cref="Action"/> to invoke on the <see cref="JWorker"/>'s background thread.</param>
        /// <param name="workId">The anonymous method Id. -------------> ( USED PURELY FOR LOGGING )</param>
        public void AssignWork(Action work, string workId)
        {
            (DbWorkers.FirstOrDefault(x => x.Suitable) ?? DbWorkers[_random.Next(0, DbWorkers.Length)]).Assign(work, workId);
        }
        /// <summary>
        /// Assigns the work <see cref="Action"/> to a <see cref="JWorker"/>.
        /// </summary>
        /// <param name="work">The <see cref="Action"/> to invoke on the <see cref="JWorker"/>'s background thread.</param>
        public void AssignWork(Action work)
        {
            (DbWorkers.FirstOrDefault(x => x.Suitable) ?? DbWorkers[_random.Next(0, DbWorkers.Length)]).Assign(work);
        }

        public void EstablishPipe()
        {
            _pipe = new NamedPipeServerStream("_spipe", PipeDirection.InOut, 1);
        }
        /// <summary>
        /// Assigns the work [<see cref="Func{T1}"/>] to a <see cref="JWorker"/>.
        /// </summary>
        /// <param name="work">The <see cref="Func{T1}"/> to invoke on the <see cref="JWorker"/>'s background thread.</param>
        /// <typeparam name="T">The type of the returned instance.</typeparam>
        public async Task<T> AssignWork<T>(Func<T> work)
        {
            return await (DbWorkers.FirstOrDefault(x => x.Suitable) ?? DbWorkers[_random.Next(0, DbWorkers.Length)]).Assign(work);
        }
        /// <summary>
        /// Stores a specified instance of <see cref="JObject{T}"/> with it's id.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        public void Store<T>(JObject<T> jObject)
        {
            AssignWork(() =>
            {
                MemoryStorage.Storage.Add(jObject.Id, jObject);
            }, StorePrefix + $"document '{jObject.Id}' with value: '{jObject.ObjectData.ToString()}'");
        }
        /// <summary>
        /// Stores the specified instance with it's id.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        public void Store<T>(T instance, string id)
        {
            JObject<T> obj = instance;
            AssignWork(() =>
            {
                MemoryStorage.Storage.Add(id, obj);
            }, StorePrefix + $"document '{id}' with value: '{obj.ObjectData.ToString()}'");
        }
        /// <summary>
        /// Loads the item of type <see cref="T"/> with a qualifier Id from the <see cref="DbMemoryStorage"/>, asynchronously.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <returns>The item.</returns>
        public async Task<T> LoadAsync<T>(string id)
        {
            return (await AssignWork((() =>
            {
                try
                {
                    return (JObject<T>) MemoryStorage.Storage[id];
                }
                catch (Exception)
                {
                    //ignore
                }
                return null;
            }))).ObjectData;
        }
        /// <summary>
        /// Loads the item of type <see cref="T"/> with a qualifier Id from the <see cref="DbMemoryStorage"/>, synchronously.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <returns>The item.</returns>
        public T Load<T>(string id)
        {
            return LoadAsync<T>(id).GetAwaiter().GetResult();
        }
        
        private const string StorePrefix = "Stored";
    }
}