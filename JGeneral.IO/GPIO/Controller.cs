using System;
using System.Device.Gpio;
namespace JGeneral.IO.GPIO
{
    public class Controller
    {
        private GpioController _controller { get; set; }
        private byte _gpioPin;
        
        internal Controller(byte gpioPin, PinMode mode)
        {
            _gpioPin = gpioPin;
            _controller = new GpioController();
            if (!_controller.IsPinOpen(gpioPin))
            {
                _controller.OpenPin(gpioPin, mode);
            }
        }

        public static Controller OpenWrite(byte gpioPin)
        {
            return new Controller(gpioPin, PinMode.Output);
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
                if (_controller.IsPinOpen(_gpioPin))
                {
                    _controller.Write(_gpioPin, signal);
                    OnWrite?.Invoke(_gpioPin);
                }
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