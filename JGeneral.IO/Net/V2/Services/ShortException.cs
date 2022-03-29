using System;

namespace JGeneral.IO.Net.V2.Services
{
    public class ShortException
    {
        public string Time;
        public string Exception;

        private ShortException(Exception exception)
        {
            Exception = exception.Message;
            Time = DateTime.Now.ToLongTimeString();
        }

        public static implicit operator ShortException(Exception e)
        {
            return new ShortException(e);
        }
    }
}