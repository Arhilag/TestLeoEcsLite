using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
// ReSharper disable CheckNamespace
#pragma warning disable CS4014

namespace Gamebase
{
    public abstract class PlayerPrefsBoolResource : IInitializable
    {
        [Inject]
        protected PlayerPrefsBoolResource(ResourcesSystem resourcesSystem)
        {
            _resourcesSystem = resourcesSystem;
        }
        
        private readonly ResourcesSystem _resourcesSystem;
        
        public void Initialize()
        {
            if (_resourcesSystem == null) return;
            _currentValue = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsKey, Convert.ToInt32(DefaultValue)));
            _resourcesSystem.Bool.Register(this);
            FirstEvent();
        }

        private async UniTask FirstEvent()
        {
            await UniTask.DelayFrame(3);
            OnResourceCountChanged?.Invoke(CurrentValue);
        }

        public abstract ResourceType Type { get; }
        protected abstract bool DefaultValue { get; }
        
        public PlayerPrefsResourceType PlayerPrefsType => PlayerPrefsResourceType.Bool;
        public event Action<bool> OnResourceCountChanged;
        private string PlayerPrefsKey => Type.ToString();
        
        private bool _currentValue;
        private bool CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnResourceCountChanged?.Invoke(_currentValue);
                PlayerPrefs.SetInt(PlayerPrefsKey, Convert.ToInt32(_currentValue));
            }
        }

        public bool Get()
        {
            return CurrentValue;
        }

        public void Set(bool value)
        {
            CurrentValue = value;
        }
    }
}