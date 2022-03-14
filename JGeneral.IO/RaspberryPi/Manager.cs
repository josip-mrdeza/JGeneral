using System;
using System.Device.Gpio;

namespace JGeneral.IO.RaspberryPi
{
    public class Manager : GpioController
    {
        public static Manager PinManager;
        
        private Manager() : base(PinNumberingScheme.Board)
        {
            if (PinManager != null)
            {
                throw new Exception("Can't create manager, only one instance can be active at a time.");
            }

            PinManager = this;
        }

        public static bool Init()
        {      
            if (PinManager != null)
            {
                return false;
            }

            PinManager = new Manager();
            return true;
        }
        public Pin Open(GPIO pin)
        {
            var num = (int)pin;
            if (!IsPinOpen(num))
            {
                base.OpenPin(num);
            }
            return Pin.Create(num, true);
        }

        public void Close(GPIO pin)
        {
            var num = (int)pin;
            if (IsPinOpen(num))
            {
                base.ClosePin(num);
            }
        }
    }

    public readonly struct Pin
    {
        public readonly bool IsOpen;
        public readonly int PinNumber;
        public bool State { get => Read() == 1; }
        private Pin(int num, bool isOpen)
        {
            IsOpen = true;
            PinNumber = num;
        }

        public void Close() => Manager.PinManager.Close((GPIO)PinNumber);
        public byte Read() => (byte)Manager.PinManager.Read(PinNumber);
        public void Write(byte value) => Manager.PinManager.Write(PinNumber, value);
        public void Write(PinValue value) => Write((byte)value);
        public void Write(bool value) => Write((byte)(value ? 1 : 0));

        internal static Pin Create(int pinNumber, bool isOpen)
        {
            return new Pin(pinNumber, isOpen);
        }

        internal static Pin CreateAndOpen(GPIO pinNumber)
        {
            var pin = Manager.PinManager.Open(pinNumber);
            return pin;
        }
    }
}