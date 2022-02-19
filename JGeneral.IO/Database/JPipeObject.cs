using System;

namespace JGeneral.IO.Database
{
    public readonly struct JPipeObject<T>
    {
        public readonly T TransmissionObject;
        public readonly Type TransmissionType;

        private JPipeObject(T transmissionObject, string id)
        {
            TransmissionObject = transmissionObject;
            TransmissionType = typeof(T);
        }

        public static JPipeObject<T> CreateInstance(T transmissionObject, string id = default)
        {
            if (typeof(T) == typeof(JObject<>))
            {
                return new JPipeObject<T>(transmissionObject, (transmissionObject as JObject<object>)!.Id);
            }
            return new JPipeObject<T>(transmissionObject, id);
        }
    }
}