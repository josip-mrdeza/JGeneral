

using System;
using System.Threading.Tasks;
using JGeneral.IO.Database;

namespace JGeneral.Conveyors.Wrappers
{
    /// <summary>
    /// Wraps <see cref="ConveyorReceiver"/> and <see cref="ConveyorSender"/> in one convenient package.
    /// </summary>
    public class ConveyorWrapper
    {
        protected ConveyorReceiver server;
        protected ConveyorSender client;
        protected string RemoteServerName;
        public Task ServerThread;
        
        public ConveyorWrapper(string serverId, string remoteId)
        {
            RemoteServerName = remoteId;
            server = new ConveyorReceiver(serverId);
            client = new ConveyorSender(remoteId);
            server.Connected += () => OnServerConnect?.Invoke(remoteId);
            server.OnReceived += json => OnServerReceived?.Invoke(json);
            client.OnFinishedSending += () => OnClientFinishedSending?.Invoke();
            ServerThread = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        server.WaitForConnection();
                        server.Receive().Wait();
                        server._serverStream.Disconnect();
                    }
                    catch (Exception e)
                    {
                        OnServerThreadExceptionOccured?.Invoke(e);
                    }
                    finally
                    {
                    }
                }
            });
        }

        public void Dispose()
        {
            server._serverStream.Dispose();
            client._clientStream.Dispose(); 
        }
        public void Connect()
        {
            client.Connect();
            OnClientConnect?.Invoke(RemoteServerName);
        }
        public async Task Transmit(object jsonObject)
        {
            await client.Transmit(jsonObject);
        }
        public event Action<string> OnServerConnect;
        public event Action<string> OnClientConnect;
        public event Action<ConveyorObject> OnServerReceived;
        public event Action OnClientFinishedSending;
        public event Action<Exception> OnServerThreadExceptionOccured;
    }
}