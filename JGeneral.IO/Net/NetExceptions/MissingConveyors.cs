using System;

namespace JGeneral.IO.Net.NetExceptions
{
    public class MissingConveyors : Exception
    {
        public MissingConveyors() : base("Missing crucial library JGeneral.Conveyors.\nAre you possibly using 'LogSenderServer' without importing JGeneral.Conveyor?")
        {
            
        }
    }
}