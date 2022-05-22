using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Контроллер для управления ресурсами типа Int
    /// </summary>
    public class ResourcesIntController
    {
        public ResourcesIntController(Action<ResourceType, PlayerPrefsResourceType> onRegister)
        {
            _onRegister = onRegister;
        }

        private readonly Dictionary<ResourceType, PlayerPrefsIntResource> _resources =
            new Dictionary<ResourceType, PlayerPrefsIntResource>();
        private readonly Action<ResourceType, PlayerPrefsResourceType> _onRegister;

        internal void Register(PlayerPrefsIntResource resource)
        {
            if (resource.PlayerPrefsType != PlayerPrefsResourceType.Int)
            {
                DebugSystem.LogError($"[ResourcesSystem] - The resource type {resource.Type} cannot be registered as an Int resource");
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
        /// Прибавить ресурсу указанное значение
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="count">Значение, которое необходимо прибавить</param>
        public void Add(ResourceType resourceType, int count)
        {
            if (!ResourceFound(resourceType)) return;
            _resources[resourceType].Add(count);
        }
        
        /// <summary>
        /// Отнять от ресурса указанное значение
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="count">Значение, которое необходимо отнять</param>
        public void Take(ResourceType resourceType, int count)
        {
            if (!ResourceFound(resourceType)) return;
            _resources[resourceType].Take(count);
        }
        
        /// <summary>
        /// Установить ресурсу новое значение
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="count">Новое значение</param>
        public void Set(ResourceType resourceType, int count)
        {
            if (!ResourceFound(resourceType)) return;
            _resources[resourceType].Set(count);
        }
        
        /// <summary>
        /// Получить значение ресурса
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <returns>Значение ресурса</returns>
        public int Get(ResourceType resourceType)
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
        public async UniTask Subscribe(ResourceType resourceType, Action<int> action)
        {
            await UniTask.WaitUntil(() => _resources.ContainsKey(resourceType));
            _resources[resourceType].OnResourceCountChanged += action;
        }

        /// <summary>
        /// Отписаться от события изменения ресурса
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="action">Метод, на который осуществлялась подписка</param>
        public void Unsubscribe(ResourceType resourceType, Action<int> action)
        {
            if (!ResourceFound(resourceType)) return;
            _resources[resourceType].OnResourceCountChanged -= action;
        }

        private bool ResourceFound(ResourceType resourceType)
        {
            var result = _resources.ContainsKey(resourceType);
            if (!result)
            {
                DebugSystem.Log($"[ResourcesSystem] - The resource type {resourceType} is not registered in the system");
            }
            return result;
        }
    }
}