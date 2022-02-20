using System;
using System.Reflection;

namespace JGeneral.IO.Reflection
{
    public static class EventModifierHelper
    {
        /// <summary>
        /// Creates a new <see cref="StaticEventModifier{T, THandlerType}"/> for regular public events.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <typeparam name="T">The type containing the event.</typeparam>
        /// <typeparam name="THandlerType">The type of an event handler, ex. Action{T1}.</typeparam>
        /// <returns>An instance of a <see cref="EventModifier{T, THandlerType}"/>.</returns>
        public static StaticEventModifier<THandlerType> CreateNonPublicStaticEventModifier<THandlerType>(this Type t, string eventName) where THandlerType : Delegate
        {
            return new StaticEventModifier<THandlerType>(t, eventName, BindingFlags.NonPublic | BindingFlags.Static);
        }
        /// <summary>
        /// Creates a new <see cref="StaticEventModifier{T, THandlerType}"/> for non-public events.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <typeparam name="T">The type containing the event.</typeparam>
        /// <typeparam name="THandlerType">The type of an event handler, ex. Action{T1}.</typeparam>
        /// <returns>An instance of a <see cref="EventModifier{T, THandlerType}"/>.</returns>
        public static StaticEventModifier<THandlerType> CreatePublicStaticEventModifier<THandlerType>(this Type t, string eventName) where THandlerType : Delegate
        {
            return new StaticEventModifier<THandlerType>(t, eventName, BindingFlags.Public | BindingFlags.Static);
        }
        /// <summary>
        /// Creates a new <see cref="EventModifier{T, THandlerType}"/> for non-public events.
        /// </summary>
        /// <param name="containingTypeInstance">The instance to create the modifier on top of</param>
        /// <param name="eventName">The name of the event.</param>
        /// <typeparam name="T">The instance's type.</typeparam>
        /// <typeparam name="THandlerType">The type of an event handler, ex. Action{T1}.</typeparam>
        /// <returns>An instance of a <see cref="EventModifier{T, THandlerType}"/>.</returns>
        public static EventModifier<T, THandlerType> CreateNonPublicEventModifier<T, THandlerType>(this T containingTypeInstance, string eventName) where THandlerType : Delegate
        {
            return new EventModifier<T, THandlerType>(containingTypeInstance, eventName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
        /// <summary>
        /// Creates a new <see cref="EventModifier{T, THandlerType}"/> for regular public events.
        /// </summary>
        /// <param name="containingTypeInstance">The instance to create the modifier on top of</param>
        /// <param name="eventName">The name of the event.</param>
        /// <typeparam name="T">The instance's type.</typeparam>
        /// <typeparam name="THandlerType">The type of an event handler, ex. Action{T1}.</typeparam>
        /// <returns>An instance of a <see cref="EventModifier{T, THandlerType}"/>.</returns>
        public static EventModifier<T, THandlerType> CreatePublicEventModifier<T, THandlerType>(this T containingTypeInstance, string eventName) where THandlerType : Delegate
        {
            return new EventModifier<T, THandlerType>(containingTypeInstance, eventName, BindingFlags.Public | BindingFlags.Instance);
        }
    }
}