using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
// ReSharper disable UnusedMember.Global
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Класс, осуществляющий вызов заданного глобального события при вызове его метода Invoke через испектор или код
    /// </summary>
    public class GlobalEventInvoker : MonoBehaviour
    {
        [Inject]
        public void Construct(GlobalEventsSystem globalEventsSystem)
        {
            _globalEventsSystem = globalEventsSystem;
        }
        
        [SerializeField, Tooltip("Событие, которое требуется вызвать при вызове метода Invoke")]
        private GlobalEventType globalEventType;
        
        private GlobalEventsSystem _globalEventsSystem;

        /// <summary>
        /// Вызвать заданное глобальное событие
        /// </summary>
#if UNITY_EDITOR
        [Button]
#endif
        public void Invoke()
        {
            _globalEventsSystem?.Invoke(globalEventType);
        }
    }
}