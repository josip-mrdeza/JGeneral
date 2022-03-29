using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JGeneral.IO.Database;
using JGeneral.IO.Net.CommandArguments;

namespace JGeneral.IO.Net
{
    public class BasicRemoteServer : JServer, IRemoteServer
    {
        public Dictionary<string, List<IRemoteCommand>> QueuedCommands { get; private set; }
        public Dictionary<Guid, IRemoteCommand> Responses { get; private set; }
        private List<NetworkUser> _networkUsers { get; set; }

        public async Task Send(IRecipient recipient, Command queryCommand, byte[] data)
        {
            var guid = Guid.NewGuid();
            var com = new InternalRemoteCommand(guid, queryCommand, data);
            if (QueuedCommands is null)
            {
                return;
            }

            if (QueuedCommands.ContainsKey(recipient.Id))
            {
                QueuedCommands[recipient.Id].Add(com);
            }
            else
            {
                QueuedCommands.Add(recipient.Id, new List<IRemoteCommand>()
                {
                    com
                });
            }
        }
        public List<NetworkUser> GetRegisteredSessionUsers() => _networkUsers;

        public void LogBackIn(string id)
        {
            if (_networkUsers.Exists(x => x.Id == id))
            {
                var i = _networkUsers.FindIndex(x => x.Id == id);
                _networkUsers[i].LastSeen = DateTime.Now;
                return;
            }

            if (!QueuedCommands.ContainsKey(id))
            {
                QueuedCommands.Add(id, new List<IRemoteCommand>()); 
            }
            _networkUsers.Add(new NetworkUser(id));
        }

        public IRemoteCommand[] GetOneTimeUses()
        {
            var list =  QueuedCommands.Where(x => x.Key == "*").Select(x => x.Value.ToArray()).FirstOrDefault();
            QueuedCommands.Remove("*");
            return list;
        }                         
        
        private IRemoteCommand[] GetQueue(string id)
        {
            if (QueuedCommands is null)
            {
                return new IRemoteCommand[0];
            }
            return !QueuedCommands.ContainsKey(id) ? new IRemoteCommand[0] : QueuedCommands![id].ToArray();
        }
        

        protected override async Task HttpGet(HttpListenerContext listenerContext)
        {
            if (QueuedCommands is null)
            {
                listenerContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                listenerContext.Response.OutputStream.Close();
                return;
            }
            
            var id = listenerContext.Request.Url.Segments.Last();
            if (id == "/")
            {
                listenerContext.Response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                listenerContext.Response.OutputStream.Close();
                return;
            }
            else if (id == "users")
            {
                if (QueuedCommands.Count == 0)
                {
                    listenerContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    listenerContext.Response.OutputStream.Close();
                    return;
                }
                var users = QueuedCommands.Select(x => x.Key).ToJsonBytes();
                listenerContext.Response.ContentLength64 = users.Length;
                await listenerContext.Response.OutputStream.WriteAsync(users, 0, users.Length);
                return;
            }

            var data = GetOneTimeUses();
            if (data == null)
            {
                data = GetQueue(id);
                if (data.Length == 0)
                {
                    listenerContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    listenerContext.Response.OutputStream.Close(); 
                    return;
                }
            }

            var jsonData = data.ToJsonBytes();
            listenerContext.Response.ContentLength64 = jsonData.Length; 
            await listenerContext.Response.OutputStream.WriteAsync(jsonData, 0, jsonData.Length);
            LogBackIn(id);
        }
        /// <summary>
        /// Put is assigned to the Consume command.
        /// </summary>
        protected override async Task HttpPut(HttpListenerContext listenerContext, long length)
        {
            if (QueuedCommands is null)
            {
                return;
            }
            var id = listenerContext.Request.Url.Segments.Last();
            LogBackIn(id);
            if (!QueuedCommands.ContainsKey(id))
            {
                listenerContext.Response.StatusCode = 204;
                return;
            }
            byte[] buffer = new byte[listenerContext.Request.ContentLength64];
            await listenerContext.Request.InputStream.ReadAsync(buffer, 0, buffer.Length);
            var json = Encoding.ASCII.GetString(buffer);
            var response = json.FromJsonText<InternalRemoteCommand>();
            var cmdId = response.CommandId;
            Responses.Add(response.CommandId, response);
            QueuedCommands[id].RemoveAll(x => x.CommandId == cmdId);
            OnConsumedCommand?.Invoke(id, response.CommandId);
        }
        /// <summary>
        /// Post request is assigned to the Register method on client side.
        /// </summary>
        protected override async Task HttpPost(HttpListenerContext listenerContext, long length)
        {
            if (QueuedCommands is null)
            {
                return;
            }
            var id = listenerContext.Request.Url.Segments.Last();
            
            if (listenerContext.Request.Url.Segments[1] == "sender/")
            {
                var buffer = new byte[length];
                await listenerContext.Request.InputStream.ReadAsync(buffer, 0, buffer.Length);
                if (!QueuedCommands.ContainsKey(id))
                {
                    QueuedCommands.Add(id, new List<IRemoteCommand>()
                    {
                        new InternalRemoteCommand(Guid.NewGuid(), Command.Shell, buffer)
                    });
                    listenerContext.Response.StatusCode = 201;
                }
                else
                {
                    QueuedCommands[id].Add(new InternalRemoteCommand(Guid.NewGuid(), Command.Shell, buffer));
                    listenerContext.Response.StatusCode = 201;
                }
            }
            else
            {
                LogBackIn(id);
                if (!QueuedCommands.ContainsKey(id))
                {
                    QueuedCommands.Add(id, new List<IRemoteCommand>());
                    OnRegister?.Invoke(id);
                    listenerContext.Response.StatusCode = 201;
                    return;
                }
                else
                {
                    var bytes = new byte[length];
                    await listenerContext.Request.InputStream.ReadAsync(bytes, 0, bytes.Length);
                    var bAsJson = bytes.FromJsonBytes<ShellArgs>();
                    var guid = Guid.NewGuid();
                    OnAddCommand?.Invoke(id, guid);
                    var commands = QueuedCommands[id]; 
                    commands.Add(new InternalRemoteCommand(guid, Command.Shell, bytes));
                    var respData = Encoding.ASCII.GetBytes($"Current commands queued for '{id}': {commands.Count.ToString()}\n" + commands.ToJson());
                    await listenerContext.Response.OutputStream.WriteAsync(respData, 0, respData.Length);
                    listenerContext.Response.StatusCode = (int) HttpStatusCode.Created;
                }

                OnFailedToRegister?.Invoke(id);
            }
        }
        //secondary get method.
        protected override async Task HttpPatch(HttpListenerContext listenerContext, long length)
        {
        }

        public BasicRemoteServer()
        {
            QueuedCommands = new Dictionary<string, List<IRemoteCommand>>();
            Responses = new Dictionary<Guid, IRemoteCommand>();
            _networkUsers = new List<NetworkUser>();
            base.Start(new CancellationToken());
        }

        public event Action<string> OnRegister;
        public event Action<string> OnFailedToRegister;
        public event Action<string, Guid> OnConsumedCommand;
        public event Action<string, Guid> OnAddCommand;
    }
}