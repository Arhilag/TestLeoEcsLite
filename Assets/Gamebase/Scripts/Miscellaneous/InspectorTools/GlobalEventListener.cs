using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
// ReSharper disable CheckNamespace

namespace Gamebase.Tools
{
    /// <summary>
    /// Класс, позволяющий осуществлять подписку на GlobalEvent через UnityEvent. Подписка осуществляется автоматически,
    /// когда объект становится активным в иерархии. Отписка также автоматическая, но при становлении объекта
    /// неактивным в иерархии.
    /// </summary>
    public class GlobalEventListener : MonoBehaviour
    {
        [Inject]
        public void Construct(GlobalEventsSystem globalEventsSystem)
        {
            _globalEventsSystem = globalEventsSystem;
        }
        
        [SerializeField, Tooltip("Событие, на которое требуется осуществить подписку")]
        private GlobalEventType globalEventType;
        
        [SerializeField, Tooltip("События Unity Event, которые требуется выполнить при срабатывании события GlobalEvent")]
        private GlobalEventListenerUnityEvent onGlobalEventInvoke;

        private GlobalEventsSystem _globalEventsSystem;

        private void OnEnable()
        {
            _globalEventsSystem.Subscribe(globalEventType, onGlobalEventInvoke.Invoke);
        }

        private void OnDisable()
        {
            _globalEventsSystem.Unsubscribe(globalEventType, onGlobalEventInvoke.Invoke);
        }
    }

    [Serializable]
    public class GlobalEventListenerUnityEvent : UnityEvent{}
}