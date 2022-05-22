using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Контроллер для управления ресурсами типа String
    /// </summary>
    public class ResourcesStringController
    {
        public ResourcesStringController(Action<ResourceType, PlayerPrefsResourceType> onRegister)
        {
            _onRegister = onRegister;
            _tempStringBuilder = new StringBuilder();
        }

        private readonly Dictionary<ResourceType, PlayerPrefsStringResource> _resources =
            new Dictionary<ResourceType, PlayerPrefsStringResource>();
        private readonly Action<ResourceType, PlayerPrefsResourceType> _onRegister;
        private readonly StringBuilder _tempStringBuilder;

        internal void Register(PlayerPrefsStringResource resource)
        {
            if (resource.PlayerPrefsType != PlayerPrefsResourceType.String)
            {
                DebugSystem.LogError($"[ResourcesSystem] - The resource type {resource.Type} cannot be registered as an String resource");
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
        /// Осуществить конкатенацию строк
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="value">Строка, которую необходимо добавить в конец текущей строки</param>
        public void Add(ResourceType resourceType, string value)
        {
            if (!ResourceFound(resourceType)) return;
            _tempStringBuilder.Clear().Append(_resources[resourceType].Get()).Append(value);
            _resources[resourceType].Set(_tempStringBuilder.ToString());
        }

        /// <summary>
        /// Удалить все вхождения заданной строки из текущей
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="value">Строка, все вхождения которой необходимо удалить из текущей</param>
        public void Take(ResourceType resourceType, string value)
        {
            if (!ResourceFound(resourceType)) return;
            _tempStringBuilder.Clear().Append(_resources[resourceType].Get()).Replace(value, "");
            _resources[resourceType].Set(_tempStringBuilder.ToString());
        }
        
        /// <summary>
        /// Установить ресурсу новое значение
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="value">Новое значение</param>
        public void Set(ResourceType resourceType, string value)
        {
            if (!ResourceFound(resourceType)) return;
            _resources[resourceType].Set(value);
        }
        
        /// <summary>
        /// Получить значение ресурса
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <returns>Значение ресурса</returns>
        public string Get(ResourceType resourceType)
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
        public async UniTask Subscribe(ResourceType resourceType, Action<string> action)
        {
            await UniTask.WaitUntil(() => _resources.ContainsKey(resourceType));
            _resources[resourceType].OnResourceCountChanged += action;
        }

        /// <summary>
        /// Отписаться от события изменения ресурса
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="action">Метод, на который осуществлялась подписка</param>
        public void Unsubscribe(ResourceType resourceType, Action<string> action)
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