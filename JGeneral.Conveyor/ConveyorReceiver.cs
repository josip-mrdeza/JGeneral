using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using JGeneral.IO.Database;

namespace JGeneral.Conveyors
{
    /// <summary>
    /// Used to receive data from another process's <see cref="ConveyorSender"/>.
    /// </summary>
    public sealed class ConveyorReceiver : IConveyor
    {
        public string Name { get; set; }
        internal readonly NamedPipeServerStream _serverStream;
        public readonly JsonStream _jsonStream;
        
        internal ConveyorReceiver(string serverId)
        {
            Name = serverId;
            _serverStream = new NamedPipeServerStream(serverId, PipeDirection.In, 2);
            _jsonStream = new JsonStream(_serverStream);
        }

        public void WaitForConnection()
        {
            _serverStream.WaitForConnection();
            Connected?.Invoke();
        }

        public async Task Receive()
        {
            var json = await _jsonStream.ReadJsonAsync();
            OnReceived?.Invoke(json.FromJsonText<ConveyorObject>()); 
        }

        public event Action Connected;
        public event Action<ConveyorObject> OnReceived;
    }
}