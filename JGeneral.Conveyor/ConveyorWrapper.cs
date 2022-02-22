

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
            server.OnReceived += json => OnServerReceived?.Invoke(new ConveyorObject(json));
            client.OnFinishedSending += () => OnClientFinishedSending?.Invoke();
            ServerThread = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await server.WaitForConnection();
                        await server.Receive();
                    }
                    catch
                    {
                        //ignored
                    }
                }
            });
        }

        public void Connect()
        {
            client.Connect();
            OnClientConnect?.Invoke();
        }
        public async Task Transmit(object jsonObject)
        {
            await client.Transmit(jsonObject);
        }
        public event Action OnServerConnect;
        public event Action OnClientConnect;
        public event Action<ConveyorObject> OnServerReceived;
        public event Action OnClientFinishedSending;
    }
}