using System.IO.Pipes;
using System.Threading.Tasks;

namespace JGeneral.Conveyor
{
    
    public interface IConveyor
    {
        public string Name { get; set; }
        public ConveyorObject Data { get; set; }
        public Task Transmit();
        public Task Receive();
    }
}