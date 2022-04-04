using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JGeneral.IO.Net.Tcp
{
    public class TcpRouter
    {
        private TcpListener Listener;
        private Task ServerThread;
        private List<Exception> ExceptionsOnServerThread = new List<Exception>();
        public TcpRouter(int port = 557)
        {
            Listener = TcpListener.Create(port);
            ServerThread = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        var client = await Listener.AcceptTcpClientAsync();
                        var network = client.GetStream();
                        using var context = TcpContext.CreateFromStream(network);
                        var data = await context.ReadAsync();
                        Console.WriteLine(Encoding.ASCII.GetString(data));
                    }
                    catch (Exception e)
                    {
                        ExceptionsOnServerThread.Add(e);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Start() => Listener.Start();
    }
}