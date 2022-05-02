using System;
using System.IO;
using System.Threading.Tasks;
using DiscordRPC;
using JGeneral.IO.Threading;

namespace JGeneral.IO.Interop.Discord
{
    public class JRichPresence
    {
        public DiscordRpcClient Client;
        public SyncThread<Action> Updater = new SyncThread<Action>();
        public SyncThread<Action> RPC = new SyncThread<Action>();
        public RichPresence RichPresence = new RichPresence()
        {
            Details = File.ReadAllText("Discord/Config/appDetails.txt"),
            State = File.ReadAllText("Discord/Config/appState.txt"),
            Timestamps = new Timestamps(DateTime.Now - new TimeSpan(3, 13, 15)),
            Assets = PresenceHelper.LoadAssetsFromDisk()
        };
        
        internal JRichPresence()
        {
            Client = new DiscordRpcClient(File.ReadAllText("Discord/Config/application_id.txt"));
            Client.Initialize();
            Client.SetPresence(RichPresence);
            Updater.TryExecuteItem(() =>
            {
                while (true)
                {
                    Client.Invoke();
                    Task.Delay(10).Wait();
                }
            });
            RPC.TryExecuteItem(() =>
            {
                while (true)
                {
                    Client.SetPresence(RichPresence);
                    Task.Delay(100).Wait();
                }
            });
        }
        
        internal JRichPresence(string appId)
        {
            Client = new DiscordRpcClient(appId);
            Client.Initialize();
            Client.SetPresence(RichPresence);
            Updater.TryExecuteItem(() =>
            {
                while (true)
                {
                    Client.Invoke();
                    Task.Delay(1000).Wait();
                }
            });
            RPC.TryExecuteItem(() =>
            {
                while (true)
                {
                    Client.SetPresence(RichPresence);
                    Task.Delay(1000).Wait();
                }
            });
        }

        public static JRichPresence Create()
        {
            return new JRichPresence();
        }
        
        public static JRichPresence Create(string presenceId)
        {
            return new JRichPresence(presenceId);
        }
        
        public static JRichPresence Create(Presence presence)
        {
            return new JRichPresence(presence.AsId());
        }
        
        ~JRichPresence()
        {
            Client.Dispose();
        }
    }
}