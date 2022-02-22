using System.IO.Pipes;
using System.Threading.Tasks;

namespace JGeneral.Conveyor
{
    
    public interface IConveyor
    {
        public string Name { get; set; }
    }
}