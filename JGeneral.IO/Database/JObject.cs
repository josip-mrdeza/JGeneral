using System;

namespace JGeneral.IO.Database
{
    public class JObject<T>
    {
        private readonly object threadLock;
        private T _objData;
        public T ObjectData
        {
            get
            {
                lock (threadLock)
                {
                    return _objData;
                }   
            }
            set
            {
                lock (threadLock)
                {
                    _objData = value;
                }
            }
        }

        public string Id { get; set; }

        public JObject(T objectData, string id)
        {
            threadLock = new object();
            ObjectData = objectData;
            Id = id;
        }
        /// <summary>
        /// Converts an arbitrary <see cref="Object"/> to a <see cref="JObject{T}"/>.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>A new <see cref="JObject{T}"/>.</returns>
        public static implicit operator JObject<T>(T obj)
        {
            return new (obj, default);
        }
        /// <summary>
        /// Converts an <see cref="JObject{T}"/> to a <see cref="T"/> <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="JObject{T}"/> to convert.</param>
        /// <returns>An instance of the T stored inside the <see cref="JObject{T}"/>.</returns>
        public static implicit operator T(JObject<T> obj)
        {
            return obj.ObjectData;
        }
        /// <summary>
        /// </summary>
        public static implicit operator JObject<object>(JObject<T> obj)
        {
            return new (obj._objData, obj.Id);
        }
    }
}