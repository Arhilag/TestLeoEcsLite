using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
// ReSharper disable CheckNamespace
#pragma warning disable CS4014

namespace Gamebase
{
    /// <summary>
    /// Абстрактный класс для ресурсов, хранение целочисленного значения которых должно быть реализовано через PlayerPrefs
    /// </summary>
    public abstract class PlayerPrefsIntResource : IInitializable
    {
        [Inject]
        protected PlayerPrefsIntResource(ResourcesSystem resourcesSystem)
        {
            _resourcesSystem = resourcesSystem;
        }
        
        private readonly ResourcesSystem _resourcesSystem;
        
        public void Initialize()
        {
            if (_resourcesSystem == null) return;
            _currentValue = PlayerPrefs.GetInt(PlayerPrefsKey, DefaultValue);
            _resourcesSystem.Int.Register(this);
            FirstEvent();
        }

        private async UniTask FirstEvent()
        {
            await UniTask.DelayFrame(3);
            OnResourceCountChanged?.Invoke(CurrentValue);
        }

        public abstract ResourceType Type { get; }
        protected abstract int DefaultValue { get; }
        
        public PlayerPrefsResourceType PlayerPrefsType => PlayerPrefsResourceType.Int;
        public event Action<int> OnResourceCountChanged;
        private string PlayerPrefsKey => Type.ToString();
        
        private int _currentValue;
        private int CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnResourceCountChanged?.Invoke(_currentValue);
                PlayerPrefs.SetInt(PlayerPrefsKey, _currentValue);
            }
        }

        public int Get()
        {
            return CurrentValue;
        }

        public void Add(int count)
        {
            if (count < 0)
            {
                DebugSystem.LogError($"[ResourceSystem] [{PlayerPrefsKey}] - Была попытка передать в метод Add отрицательное значение, используйте метод Take для уменьшения ресурса.");
                return;
            }
            CurrentValue += count;
        }

        public void Take(int count)
        {
            if (count < 0)
            {
                DebugSystem.LogError($"[ResourceSystem] [{PlayerPrefsKey}] - Была попытка передать в метод Take отрицательное значение, используйте метод Add для добавления ресурса.");
                return;
            }
            CurrentValue -= count;
        }

        public void Set(int count)
        {
            CurrentValue = count;
        }
    }
}