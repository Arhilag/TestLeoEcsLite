using System;
using System.Collections.Generic;
using Zenject;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Gamebase
{
    /// <summary>
    /// Система глобальных игровых событий. Дает функционал для подписывания и вызова событий. Для добавления новых типов событий необходимо править перечисление GlobalEventType. Использовать разумно, не все события в игре стоит обрабатывать через систему глобальных событий. Удобно для прототипирования.
    /// </summary>
    public class GlobalEventsSystem : IInitializable, IDisposable
    {
        private readonly Dictionary<GlobalEventType, Action> _events = new Dictionary<GlobalEventType, Action>();

        public void Initialize()
        {
            var values = Enum.GetValues(typeof(GlobalEventType));
            for (var i = 0; i < values.Length; i++)
            {
                _events.Add((GlobalEventType)values.GetValue(i), () => { });
            }
            Log("System initialized");
        }

        public void Dispose()
        {
            ClearAllEvents();
        }

        /// <summary>
        /// Очистить все события
        /// </summary>
        public void ClearAllEvents()
        {
            var values = Enum.GetValues(typeof(GlobalEventType));
            for (var i = 0; i < Enum.GetValues(typeof(GlobalEventType)).Length; i++)
            {
                _events[(GlobalEventType)values.GetValue(i)] = null;
            }
        }

        /// <summary>
        /// Подписать метод на вызов любого глобального события
        /// </summary>
        /// <param name="action"></param>
        public void SubscribeToAll(Action<GlobalEventType> action)
        {
            var values = Enum.GetValues(typeof(GlobalEventType));
            for (var i = 0; i < Enum.GetValues(typeof(GlobalEventType)).Length; i++)
            {
                var globalEventType = (GlobalEventType)values.GetValue(i);
                _events[globalEventType] += () => { action?.Invoke(globalEventType); };
            }
            Log("Subscribe to all");
        }

        /// <summary>
        /// Подписаться на событие
        /// </summary>
        /// <param name="type">Тип события (из перечисленных в GlobalEventType)</param>
        /// <param name="action">Метод, который требуется подписать на событие</param>
        public void Subscribe(GlobalEventType type, Action action)
        {
            _events[type] += action;
            Log($"Subscribe ({type})");
        }

        /// <summary>
        /// Отписаться от события
        /// </summary>
        /// <param name="type">Тип события (из перечисленных в GlobalEventType)</param>
        /// <param name="action">Метод, который требуется отписать от события</param>
        public void Unsubscribe(GlobalEventType type, Action action)
        {
            _events[type] -= action;
            Log($"Unsubscribe ({type})");
        }

        /// <summary>
        /// Вызвать срабатывание события
        /// </summary>
        /// <param name="type">Тип события (из перечисленных в GlobalEventType)</param>
        public void Invoke(GlobalEventType type)
        {
            _events[type]?.Invoke();
            Log($"Invoke Global Event ({type})");
        }

        private void Log(string message)
        {
            if (!DebugSystem.EnableGamebaseMessages) return;
            DebugSystem.Log($"[GlobalEventsSystem] - {message}");
        }
    }
}