using System;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Класс, предоставляющий доступ к проверке текущей ориентации устройства, а также дающий возможность подписаться
    /// на событие ее изменения
    /// </summary>
    public class DeviceOrientationObserver : AutoSingleton<DeviceOrientationObserver>
    {
        private const float TOLERANCE = float.Epsilon;

        private float _previousLeftSafeArea;
        private float _previousRightSafeArea;

        private DeviceOrientation _orientation = DeviceOrientation.Unknown;
        /// <summary> Текущая ориентация устройства </summary>
        public DeviceOrientation Orientation
        {
            get => _orientation;
            private set
            {
                if (value == _orientation) return;
                _orientation = value;
                OnDeviceOrientationChange?.Invoke();
                OnDeviceOrientationChangeTo?.Invoke(_orientation);
            }
        }

        /// <summary> Событие изменения ориентации устройства (без указания новой ориентации) </summary>
        public event Action OnDeviceOrientationChange;
        
        /// <summary> Событие изменения ориентации устройства (с указанием новой ориентации) </summary>
        public event Action<DeviceOrientation> OnDeviceOrientationChangeTo;

        private bool _notified;

        protected override void Initialize()
        {
            OnDeviceOrientationChange += () => _notified = true;
        }
        
        private void Update()
        {
            _notified = false;
            Orientation = Input.deviceOrientation;
            
            if (_notified) return;
            
            var safeAreaRect = Screen.safeArea;
            var leftSafeArea = safeAreaRect.xMin;
            var rightSafeArea = -(Screen.width - safeAreaRect.xMax);

            if (Math.Abs(leftSafeArea - _previousLeftSafeArea) > TOLERANCE ||
                Math.Abs(rightSafeArea - _previousRightSafeArea) > TOLERANCE)
            {
                _previousLeftSafeArea = leftSafeArea;
                _previousRightSafeArea = rightSafeArea;
                
                OnDeviceOrientationChange?.Invoke();
                OnDeviceOrientationChangeTo?.Invoke(Orientation);
            }
        }
    }
}
