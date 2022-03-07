using System;

namespace JGeneral.IO.Net
{
    public interface IRemoteResponse
    {
        public Guid CommandId { get; }
        public Command RelayedCommand { get; }
        public byte[] TransferredData { get; }
    }
}