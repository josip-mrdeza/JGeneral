using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JGeneral.IO.Net
{
    public interface IRemoteClient : IRecipient
    {
        public Task<IRemoteCommand[]> CheckQueue();
    }
    public interface IRemoteServer
    {
        public Dictionary<string, List<IRemoteCommand>> QueuedCommands { get; }
        
        public Task Send(IRecipient recipient, Command queryCommand, byte[] data);
    }
}