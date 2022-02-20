using System;
using System.Reflection;

namespace JGeneral.IO.Reflection
{
    public class StaticEventModifier<THandlerType> where THandlerType : Delegate
    {
        private EventInfo _eventInfo;
        private THandlerType _lastHandler;
        internal StaticEventModifier(Type t, string eventName)
        {
            _eventInfo = t.GetEvent(eventName);
        }
        internal StaticEventModifier(Type t, string eventName, BindingFlags flags)
        {
            _eventInfo = t.GetEvent(eventName, flags | BindingFlags.Static);
        }
        /// <summary>
        /// Adds an event handler to the event if it is public, otherwise throws.
        /// </summary>
        public void AddHandler(THandlerType action)
        {
            _eventInfo.AddEventHandler(null, action);
        }
        /// <summary>
        /// Removes the last event handler added by this class.
        /// </summary>
        public void RemoveLastHandler()
        {
            _eventInfo.RemoveEventHandler(null, _lastHandler);
        }
    }
}