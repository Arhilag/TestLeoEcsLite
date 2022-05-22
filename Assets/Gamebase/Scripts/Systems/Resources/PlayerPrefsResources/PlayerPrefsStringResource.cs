using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
// ReSharper disable CheckNamespace
#pragma warning disable CS4014

namespace Gamebase
{
    public abstract class PlayerPrefsStringResource : IInitializable
    {
        [Inject]
        protected PlayerPrefsStringResource(ResourcesSystem resourcesSystem)
        {
            _resourcesSystem = resourcesSystem;
        }
        
        private readonly ResourcesSystem _resourcesSystem;
        
        public void Initialize()
        {
            if (_resourcesSystem == null) return;
            _currentValue = PlayerPrefs.GetString(PlayerPrefsKey, DefaultValue);
            _resourcesSystem.String.Register(this);
            FirstEvent();
        }

        private async UniTask FirstEvent()
        {
            await UniTask.DelayFrame(3);
            OnResourceCountChanged?.Invoke(CurrentValue);
        }

        public abstract ResourceType Type { get; }
        protected abstract string DefaultValue { get; }
        
        public PlayerPrefsResourceType PlayerPrefsType => PlayerPrefsResourceType.String;
        public event Action<string> OnResourceCountChanged;
        private string PlayerPrefsKey => Type.ToString();
        
        private string _currentValue;
        private string CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnResourceCountChanged?.Invoke(_currentValue);
                PlayerPrefs.SetString(PlayerPrefsKey, _currentValue);
            }
        }

        public string Get()
        {
            return CurrentValue;
        }

        public void Set(string value)
        {
            CurrentValue = value;
        }
    }
}