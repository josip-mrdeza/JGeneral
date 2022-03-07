using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JGeneral.Conveyors.Wrappers
{
    /// <summary>
    /// Provides a means to transfer managed objects to other processes through <see cref="IConveyor"/>s.
    /// </summary>
    public class Conveyor : ConveyorWrapper
    {
        public Conveyor(string serverId, string remoteId) : base(serverId, remoteId)
        {
        }

        public static async Task Broadcast(object jsonObject)
        {
            var pipes = ListAllJPipes();
            foreach (var sender in pipes.Select(x => x.Replace(@"\\.\pipe\", string.Empty)).Select(pipe => new ConveyorSender(pipe)))
            {
                sender.Connect();
                await sender.Transmit(jsonObject);
            }
        }
        /// <summary>
        /// Creates a conveyor pipe with a leading string of 'joki_'.
        /// </summary>
        public static Conveyor CreateJConveyor(string serverId, string remoteId)
        {
            return new ($"joki_{serverId}", remoteId);
        }
        /// <summary>
        /// Lists all pipes with a leading string of 'joki_'.
        /// </summary>
        /// <returns></returns>
        public static List<string> ListAllJPipes()
        {
            return Directory.GetFiles(@"\\.\pipe\").Where(x => x.Contains("joki_")).ToList();
        }
    }
}