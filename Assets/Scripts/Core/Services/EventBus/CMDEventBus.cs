using System;
using System.Collections.Generic;

namespace Core.Services.EventBus
{
    public static class CMDEventBus
    {
        // Словарь событий по типу
        private static readonly Dictionary<Type, List<Delegate>>
            _eventListeners = new Dictionary<Type, List<Delegate>>();

        private static bool _isPreparingKeyCard = false;

        // Подписка на событие определенного типа
        public static void Subscribe<T>(Action<T> listener) where T : class
        {
            Type eventType = typeof(T);

            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<Delegate>();
            }

            _eventListeners[eventType].Add(listener);
        }

        // Отписка от события
        public static void Unsubscribe<T>(Action<T> listener) where T : class
        {
            Type eventType = typeof(T);

            if (!_eventListeners.TryGetValue(eventType, out List<Delegate> eventListener))
            {
                return;
            }

            eventListener.Remove(listener);

            if (_eventListeners[eventType].Count == 0)
            {
                _eventListeners.Remove(eventType);
            }
        }

        // Вызов события для всех подписчиков
        public static void Publish<T>(T publishedEvent) where T : class
        {
            Type eventType = typeof(T);

            if (!_eventListeners.ContainsKey(eventType))
            {
                return;
            }

            foreach (Delegate listener in _eventListeners[eventType])
            {
                (listener as Action<T>)?.Invoke(publishedEvent);
            }
        }
    }
}
