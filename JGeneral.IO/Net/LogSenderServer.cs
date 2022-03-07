using System;
using System.Linq;
using System.Threading.Tasks;
using JGeneral.IO.Net.NetExceptions;
using JGeneral.IO.Reflection;

namespace JGeneral.IO.Net
{
    /// <summary>
    /// Broadcasts all it's received data ( from <see cref="BasicRemoteServer"/>'s requests ) to all available named pipes created with the Conveyor.CreateJConveyor() method.
    /// </summary>
    public sealed class LogSenderServer : BasicRemoteServer
    {
        private static StaticExecutor<Task> _reflectionAwaiter;
        /// <exception cref="MissingConveyors">Throws when the executing assembly does not have the Assembly
        /// JGeneral.Conveyor loaded.</exception>
        public LogSenderServer()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.FirstOrDefault(x => x.GetName().Name == "JGeneral.Conveyor"); 
            
            if (assembly == null)
            {
                throw new MissingConveyors();
            }

            var type = assembly!.GetType("JGeneral.Conveyors.Wrappers.Conveyor");
            _reflectionAwaiter ??= new StaticExecutor<Task>(type, "Broadcast");
            base.OnCompleteGet += async s => await _reflectionAwaiter.Run(s);
            base.OnCompletePost += async s => await _reflectionAwaiter.Run(s);
            base.OnCompletePut += async s => await _reflectionAwaiter.Run(s);
            base.OnFailedGet += async exception => await _reflectionAwaiter.Run(exception);
            base.OnFailedPost += async exception => await _reflectionAwaiter.Run(exception);
            base.OnFailedPut += async exception => await _reflectionAwaiter.Run(exception);
            base.OnNonOverridenMethodInvocation += async s => await _reflectionAwaiter.Run(s);
        }
    }
    
    
}