using System;
using System.Device.Gpio;
namespace JGeneral.IO.GPIO
{
    public class Controller
    {
        private protected GpioController _controller { get; set; }
        private protected byte _gpioPin;
        
        internal Controller(byte gpioPin, PinMode mode)
        {
            _gpioPin = gpioPin;
            _controller = new GpioController();
            _controller.OpenPin(gpioPin, mode);
        }

        public static Controller OpenWrite(byte gpioPin)
        {
            return new Controller(gpioPin, PinMode.Output);
        }
        
        public static Controller OpenRead(Pins.Datasheet gpioPin)
        {
            return new Controller((byte)gpioPin, PinMode.Input);
        }
        
        public static Controller OpenWrite(Pins.Datasheet gpioPin)
        {
            return new Controller((byte)gpioPin, PinMode.Output);
        }
        
        public static Controller OpenRead(byte gpioPin)
        {
            return new Controller(gpioPin, PinMode.Input);
        }

        public void Dispose()
        {
            _controller.Dispose();
        }
        public bool Write(bool signal)
        {
            if (_controller.GetPinMode(_gpioPin) == PinMode.Input)
            {
                throw new Exception("Can't write signal value to a read-only pin.");
            }
            try
            {
                _controller.Write(_gpioPin, signal ? PinValue.High : PinValue.Low);
                OnWrite?.Invoke(_gpioPin);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Reads data from an assigned gpio pin.
        /// </summary>
        /// <param name="pinValue">Outputs the value of the pin or 'false' if the read failed.</param>
        /// <returns>Whether the operation succeeded or failed.</returns>
        public bool Read(out bool pinValue)
        {
            if (_controller.GetPinMode(_gpioPin) != PinMode.Input)
            {
                throw new Exception("Can't write signal value to a write-only pin.");
            }
            try
            {
                pinValue = _controller.Read(_gpioPin) == PinValue.High;
                OnRead?.Invoke(_gpioPin);
                return true;
            }
            catch
            {
                pinValue = false;
                return false;
            }
        }
        
        public static event Action<int> OnWrite;
        public static event Action<int> OnRead;
    }
}