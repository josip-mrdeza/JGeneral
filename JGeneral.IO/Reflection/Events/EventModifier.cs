using System;
using System.Reflection;

namespace JGeneral.IO.Reflection
{
    public class EventModifier<T, THandlerType> where THandlerType : Delegate
    {
        private T _instance;
        private readonly EventInfo _eventInfo;
        private THandlerType _lastHandler;
        internal EventModifier(T instance, string eventName)
        {
            _instance = instance;
            _eventInfo = typeof(T).GetEvent(eventName);
        }
        internal EventModifier(T instance, string eventName, BindingFlags flags)
        {
            _instance = instance;
            _eventInfo = typeof(T).GetEvent(eventName, flags);
        }
        /// <summary>
        /// Adds an event handler to the event if it is public, otherwise throws.
        /// </summary>
        public void AddHandler(THandlerType action)
        {
            _eventInfo.AddEventHandler(_instance, action);
        }
        /// <summary>
        /// Removes the last event handler added by this class.
        /// </summary>
        public void RemoveLastHandler()
        {
            _eventInfo.RemoveEventHandler(_instance, _lastHandler);
        }
    }
        public class EventModifier<THandlerType> where THandlerType : Delegate
        {
            private object _instance;
            private readonly EventInfo _eventInfo;
            private THandlerType _lastHandler;
            internal EventModifier(object instance, string eventName)
            {
                _instance = instance;
                _eventInfo = instance.GetType().GetEvent(eventName);
            }
            internal EventModifier(object instance, string eventName, BindingFlags flags)
            {
                _instance = instance;
                _eventInfo = instance.GetType().GetEvent(eventName, flags);
            }
            /// <summary>
            /// Adds an event handler to the event if it is public, otherwise throws.
            /// </summary>
            public void AddHandler(THandlerType action)
            {
                _eventInfo.AddEventHandler(_instance, action);
            }
            /// <summary>
            /// Removes the last event handler added by this class.
            /// </summary>
            public void RemoveLastHandler()
            {
                _eventInfo.RemoveEventHandler(_instance, _lastHandler);
            }
        }
}