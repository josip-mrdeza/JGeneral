using System;
using System.Text;
using JGeneral.IO.Database;
using Newtonsoft.Json;

namespace JGeneral.IO.Net
{
    [JsonObject]
    internal class InternalRemoteCommand : IRemoteCommand
    {
        public Guid CommandId { get; internal set; }
        [JsonRequired]
        public Command RelayedCommand { get; internal set; }
        [JsonRequired]
        public string StringiifiedCommand { get; internal set; }
        public byte[] TransferredData { get; internal set; }
        public string JsonData
        {
            get => Encoding.ASCII.GetString(TransferredData);
        }
        [JsonConstructor]
        public InternalRemoteCommand(Guid commandId, Command relayedCommand, byte[] transferredData)
        {
            CommandId = commandId;
            RelayedCommand = relayedCommand;
            StringiifiedCommand = relayedCommand.ToString();
            TransferredData = transferredData;
        }
        // [JsonConstructor]
        // internal InternalRemoteCommand(Guid commandId, string sentCommand, byte[] transferredData)
        // {
        //     Enum.TryParse(sentCommand, out Command command);
        //     StringiifiedCommand = sentCommand;
        //     CommandId = commandId;
        //     RelayedCommand = command;
        //     TransferredData = transferredData;
        // }
    }
}