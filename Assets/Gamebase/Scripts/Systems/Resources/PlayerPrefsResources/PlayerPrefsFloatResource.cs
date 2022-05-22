using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
// ReSharper disable CheckNamespace
#pragma warning disable CS4014

namespace Gamebase
{
    public abstract class PlayerPrefsFloatResource : IInitializable
    {
        [Inject]
        protected PlayerPrefsFloatResource(ResourcesSystem resourcesSystem)
        {
            _resourcesSystem = resourcesSystem;
        }
        
        private readonly ResourcesSystem _resourcesSystem;
        
        public void Initialize()
        {
            if (_resourcesSystem == null) return;
            _currentValue = PlayerPrefs.GetFloat(PlayerPrefsKey, DefaultValue);
            _resourcesSystem.Float.Register(this);
            FirstEvent();
        }

        private async UniTask FirstEvent()
        {
            await UniTask.DelayFrame(3);
            OnResourceCountChanged?.Invoke(CurrentValue);
        }

        public abstract ResourceType Type { get; }
        protected abstract float DefaultValue { get; }
        
        public PlayerPrefsResourceType PlayerPrefsType => PlayerPrefsResourceType.Float;
        public event Action<float> OnResourceCountChanged;
        private string PlayerPrefsKey => Type.ToString();
        
        private float _currentValue;
        private float CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnResourceCountChanged?.Invoke(_currentValue);
                PlayerPrefs.SetFloat(PlayerPrefsKey, _currentValue);
            }
        }

        public float Get()
        {
            return CurrentValue;
        }

        public void Add(float count)
        {
            if (count < 0f)
            {
                DebugSystem.LogError($"[ResourceSystem] [{PlayerPrefsKey}] - Была попытка передать в метод Add отрицательное значение, используйте метод Take для уменьшения ресурса.");
                return;
            }
            CurrentValue += count;
        }

        public void Take(float count)
        {
            if (count < 0f)
            {
                DebugSystem.LogError($"[ResourceSystem] [{PlayerPrefsKey}] - Была попытка передать в метод Take отрицательное значение, используйте метод Add для добавления ресурса.");
                return;
            }
            CurrentValue -= count;
        }

        public void Set(float count)
        {
            CurrentValue = count;
        }
    }
}