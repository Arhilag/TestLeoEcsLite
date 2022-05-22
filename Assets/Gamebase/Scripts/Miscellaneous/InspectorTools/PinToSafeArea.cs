using System;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Tools
{
    /// <summary>
    /// Изменяет размер элемента пользовательского интерфейса с помощью RectTransform с учетом безопасных областей
    /// текущего устройства.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class PinToSafeArea : MonoBehaviour
    {
        [SerializeField] private OrientationType orientationType = OrientationType.Landscape;
        
        private const float BOTTOM = 0f;
        private const float TOP = 0f;
        
        private RectTransform _rectTransform;
        private bool _subscribedToDeviceOrientationChange;
        
        private enum OrientationType
        {
            Landscape,
            Portrait
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            switch (orientationType)
            {
                case OrientationType.Landscape:
                    ApplyLandscapeSafeArea();
                    break;
                case OrientationType.Portrait:
                    ApplyPortraitSafeArea();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            UnsubcribeFromDeviceOrientationChange();
        }

        private void SubsribeToDeviceOrientationChange()
        {
            DeviceOrientationObserver.Instance.OnDeviceOrientationChange += ApplyLandscapeSafeArea;
            _subscribedToDeviceOrientationChange = true;
        }

        private void UnsubcribeFromDeviceOrientationChange()
        {
            if (!_subscribedToDeviceOrientationChange) return;
            
            DeviceOrientationObserver.Instance.OnDeviceOrientationChange -= ApplyLandscapeSafeArea;
            _subscribedToDeviceOrientationChange = false;
        }

        private void ApplyPortraitSafeArea()
        {
            var safeAreaRect = Screen.safeArea;

            var left = safeAreaRect.xMin;
            var bottom = safeAreaRect.yMin;
            var right = -(Screen.width - safeAreaRect.xMax);
            var top = -(Screen.height - safeAreaRect.yMax);

            _rectTransform.offsetMin = new Vector2(left, bottom);
            _rectTransform.offsetMax = new Vector2(right, top);
        }

        private void ApplyLandscapeSafeArea()
        {
            var safeAreaRect = Screen.safeArea;

            var left = safeAreaRect.xMin;
            var right = -(Screen.width - safeAreaRect.xMax);

            _rectTransform.offsetMin = new Vector2(left, BOTTOM);
            _rectTransform.offsetMax = new Vector2(right, TOP);
            
            if (_subscribedToDeviceOrientationChange) return;
            
            if (left != 0f || right != 0f)
            {
                SubsribeToDeviceOrientationChange();
            }
        }
    }
}
