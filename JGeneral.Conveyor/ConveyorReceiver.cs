using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using JGeneral.IO.Database;

namespace JGeneral.Conveyor
{
    /// <summary>
    /// Used to receive data from another process's <see cref="ConveyorSender"/>.
    /// </summary>
    public sealed class ConveyorReceiver : IConveyor
    {
        public string Name { get; set; }
        public ConveyorObject Data { get; set; }
        private readonly NamedPipeServerStream _serverStream;
        public readonly JsonStream _jsonStream;
        
        internal ConveyorReceiver(string serverId)
        {
            Name = serverId;
            Data = new ConveyorObject();
            _serverStream = new NamedPipeServerStream(serverId, PipeDirection.In, 2);
            _jsonStream = new JsonStream(_serverStream);
        }

        public async Task WaitForConnection()
        {
            await _serverStream.WaitForConnectionAsync();
            Connected?.Invoke();
        }
        [Obsolete]
        public Task Transmit()
        {
            throw new NotImplementedException();
        }

        public async Task Receive()
        {
            var json = await _jsonStream.ReadJsonAsync();
            Data = (json).FromJsonText<ConveyorObject>();
            OnReceivedJson?.Invoke(json);
        }
        
        public async Task ReceiveConveyorObject()
        {
            var json = await _jsonStream.ReadJsonAsync();
            var received =  json.FromJsonText<ConveyorObject>();
            Data = received;
            OnReceivedJson?.Invoke(json);
            OnReceivedConveyorObject?.Invoke(received);
        }

        public event Action Connected;
        public event Action<string> OnReceivedJson;
        public event Action<ConveyorObject> OnReceivedConveyorObject;
    }
}