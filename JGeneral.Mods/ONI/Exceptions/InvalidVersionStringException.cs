using System;

namespace JGeneral.Mods.ONI.Exceptions
{
    public class InvalidVersionStringException : Exception
    {
        public InvalidVersionStringException() : base("The mod version string provided in the ModInfo was invalid, make sure it is in a valid format; Example: '1.0.0.0'") {}
    }
}