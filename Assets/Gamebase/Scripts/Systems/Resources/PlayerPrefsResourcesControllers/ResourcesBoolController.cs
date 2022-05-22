using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Контроллер для управления ресурсами типа Bool
    /// </summary>
    public class ResourcesBoolController
    {
        public ResourcesBoolController(Action<ResourceType, PlayerPrefsResourceType> onRegister)
        {
            _onRegister = onRegister;
        }
        
        private readonly Dictionary<ResourceType, PlayerPrefsBoolResource> _resources =
            new Dictionary<ResourceType, PlayerPrefsBoolResource>();
        private readonly Action<ResourceType, PlayerPrefsResourceType> _onRegister;

        internal void Register(PlayerPrefsBoolResource resource)
        {
            if (resource.PlayerPrefsType != PlayerPrefsResourceType.Bool)
            {
                DebugSystem.LogError($"[ResourcesSystem] - The resource type {resource.Type} cannot be registered as an Bool resource");
                return;
            }

            if (_resources.ContainsKey(resource.Type))
            {
                DebugSystem.LogError($"[ResourcesSystem] - The resource type {resource.Type} is already registered in the system");
                return;
            }
            
            _resources.Add(resource.Type, resource);
            _onRegister?.Invoke(resource.Type, resource.PlayerPrefsType);
        }
        
        /// <summary>
        /// Установить ресурсу новое значение
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="value">Новое значение</param>
        public void Set(ResourceType resourceType, bool value)
        {
            if (!ResourceFound(resourceType)) return;
            _resources[resourceType].Set(value);
        }
        
        /// <summary>
        /// Получить значение ресурса
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <returns>Значение ресурса</returns>
        public bool Get(ResourceType resourceType)
        {
            if (!_resources.ContainsKey(resourceType)) return default;
            return _resources[resourceType].Get();
        }

        /// <summary>
        /// Подписаться на изменение значения ресурса (в том числе и при попытке установить то же значение, что было
        /// ранее). Подписка осуществляется сразу после регистрации ресурса в системе.
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="action">Метод, который требуется выполнить при изменении значения ресурса</param>
        public async UniTask Subscribe(ResourceType resourceType, Action<bool> action)
        {
            await UniTask.WaitUntil(() => _resources.ContainsKey(resourceType));
            _resources[resourceType].OnResourceCountChanged += action;
        }

        /// <summary>
        /// Отписаться от события изменения ресурса
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="action">Метод, на который осуществлялась подписка</param>
        public void Unsubscribe(ResourceType resourceType, Action<bool> action)
        {
            if (!ResourceFound(resourceType)) return;
            _resources[resourceType].OnResourceCountChanged -= action;
        }

        private bool ResourceFound(ResourceType resourceType)
        {
            var result = _resources.ContainsKey(resourceType);
            if (!result)
            {
                DebugSystem.LogError($"[ResourcesSystem] - The resource type {resourceType} is not registered in the system");
            }
            return result;
        }
    }
}