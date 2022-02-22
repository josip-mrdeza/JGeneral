using System;

namespace JGeneral.Conveyor
{
    public class ConveyorObject
    {
        public Guid Id;
        public object Data;

        public ConveyorObject(object data = null)
        {
            Id = Guid.NewGuid();
            Data = data;
        }
    }
}