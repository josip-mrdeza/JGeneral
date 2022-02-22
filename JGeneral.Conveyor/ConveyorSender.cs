using System;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading.Tasks;
using JGeneral.IO.Database;

namespace JGeneral.Conveyor
{
    /// <summary>
    /// Sends data to another process's instance of <see cref="ConveyorReceiver"/>.
    /// </summary>
    public sealed class ConveyorSender : IConveyor
    {
        public string Name { get; set; }
        public ConveyorObject Data { get; set; }
        private NamedPipeClientStream _clientStream;
        public readonly JsonStream _jsonStream;

        internal ConveyorSender(string serverId)
        {
            Name = ".";
            Data = new ConveyorObject();
            _clientStream = new NamedPipeClientStream(".", serverId, PipeDirection.Out, PipeOptions.None, TokenImpersonationLevel.Impersonation);
            _jsonStream = new JsonStream(_clientStream);
        }

        public void Connect() => _clientStream.Connect();
        
        [Obsolete]
        public async Task Transmit()
        {
            await _jsonStream.WriteJsonAsync(Data.ToJson());
            OnFinishedSending?.Invoke();
        }

        public async Task Transmit(object jsonObject)
        {
            var co = new ConveyorObject(jsonObject);
            await _jsonStream.WriteJsonAsync(co.ToJson());
            OnFinishedSending?.Invoke();
        }
        
        [Obsolete]
        public Task Receive()
        {
            throw new System.NotImplementedException();
        }

        public event Action OnFinishedSending;
    }
}