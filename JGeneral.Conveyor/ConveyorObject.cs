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
    
    public class ConveyorObject<T>
    {
        public Guid Id;
        public T Data;

        public ConveyorObject(T data)
        {
            Id = Guid.NewGuid();
            Data = data;
        }

        public static implicit operator ConveyorObject(ConveyorObject<T> genericConveyorObject)
        {
            return new (genericConveyorObject.Data);
        }
    }
}