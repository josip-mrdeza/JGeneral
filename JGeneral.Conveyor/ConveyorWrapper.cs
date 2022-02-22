

using System;
using System.Threading.Tasks;
using JGeneral.IO.Database;

namespace JGeneral.Conveyor
{
    /// <summary>
    /// Wraps <see cref="ConveyorReceiver"/> and <see cref="ConveyorSender"/> in one convenient package.
    /// </summary>
    public class ConveyorWrapper
    {
        protected ConveyorReceiver server;
        protected ConveyorSender client;
        public Task ServerThread;
        public ConveyorWrapper(string serverId, string remoteId)
        {
            server = new ConveyorReceiver(serverId);
            client = new ConveyorSender(remoteId);
            server.Connected += () => OnServerConnect?.Invoke();
            server.OnReceivedJson += json => OnServerReceivedJson?.Invoke(json);
            server.OnReceivedConveyorObject += co => OnServerReceivedConveyorObject?.Invoke(co);
            client.OnFinishedSending += () => OnClientFinishedSending?.Invoke();
            ServerThread = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await server.WaitForConnection();
                        await server.ReceiveConveyorObject();
                    }
                    catch
                    {
                        //ignored
                    }
                }
            });
        }

        public void ConnectToServer()
        {
            client.Connect();
        }

        private async Task WaitForConnection()
        {
            await server.WaitForConnection();
        }

        private async Task Receive()
        {
            var json = await server._jsonStream.ReadJsonAsync();
            server.Data = (json).FromJsonText<ConveyorObject>();
        }
        
        private async Task<ConveyorObject> ReceiveConveyorObject()
        {
            var received =  (await server._jsonStream.ReadJsonAsync()).FromJsonText<ConveyorObject>();
            server.Data = received;
            return received;
        }
        
        public async Task Transmit(object jsonObject)
        {
            await client.Transmit(jsonObject);
        }
        
        
        public event Action OnServerConnect;
        public event Action<string> OnServerReceivedJson;
        public event Action<ConveyorObject> OnServerReceivedConveyorObject;
        public event Action OnClientFinishedSending;
    }
}