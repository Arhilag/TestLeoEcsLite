using System.Collections.Generic;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase
{
    /// <summary>
    /// Система ресурсов предоставляет интерфейс для работы с игровыми ресурсами целочисленного значения, дробными числами, строками и булевыми значениями.
    /// </summary>
    public class ResourcesSystem
    {
        private ResourcesIntController _int;
        private ResourcesFloatController _float;
        private ResourcesBoolController _bool;
        private ResourcesStringController _string;

        private readonly Dictionary<ResourceType, PlayerPrefsResourceType> _resourceTypes =
            new Dictionary<ResourceType, PlayerPrefsResourceType>();

        /// <summary>
        /// Контроллер для управления ресурсами типа Int
        /// </summary>
        public ResourcesIntController Int
        {
            get
            {
                _int ??= new ResourcesIntController(RegisterResourceType);
                return _int;
            }
        }
        
        /// <summary>
        /// Контроллер для управления ресурсами типа Float
        /// </summary>
        public ResourcesFloatController Float
        {
            get
            {
                _float ??= new ResourcesFloatController(RegisterResourceType);
                return _float;
            }
        }
        
        /// <summary>
        /// Контроллер для управления ресурсами типа Bool
        /// </summary>
        public ResourcesBoolController Bool
        {
            get
            {
                _bool ??= new ResourcesBoolController(RegisterResourceType);
                return _bool;
            }
        }
        
        /// <summary>
        /// Контроллер для управления ресурсами типа String
        /// </summary>
        public ResourcesStringController String
        {
            get
            {
                _string ??= new ResourcesStringController(RegisterResourceType);
                return _string;
            }
        }

        internal PlayerPrefsResourceType GetPlayerPrefsResourceType(ResourceType resourceType)
        {
            if (!_resourceTypes.ContainsKey(resourceType)) return PlayerPrefsResourceType.None;
            return _resourceTypes[resourceType];
        }

        private void RegisterResourceType(ResourceType resourceType, PlayerPrefsResourceType playerPrefsResourceType)
        {
            if (_resourceTypes.ContainsKey(resourceType)) return;
            _resourceTypes.Add(resourceType, playerPrefsResourceType);
        }
    }
}