using System;
using System.IO;

namespace JGeneral.IO.Net
{
    public interface IRemoteCommand
    {
        public Guid CommandId { get; }
        public Command RelayedCommand { get; }
        public byte[] TransferredData { get; }
    }

    public enum Command
    {
        ProgramStart,
        ProgramEnd,
        ProgramNames,
        ProgramPIDs,
        FileCreate,
        FileDelete,
        Update,        
        Download,
        Shell,
        
    }
}