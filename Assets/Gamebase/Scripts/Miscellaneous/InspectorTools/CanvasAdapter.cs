using UnityEngine;
using UnityEngine.UI;
// ReSharper disable CheckNamespace
// ReSharper disable RedundantDefaultMemberInitializer

namespace Gamebase.Tools
{
    /// <summary>
    /// Класс, автоматически меняющий Scale Factor в CanvasScaler в зависимости от текущего аспекта экрана
    /// </summary>
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasAdapter : MonoBehaviour
    {
        [SerializeField] private float targetAspect = 1.777f;
        [SerializeField] private float minimalMatch = 0f;
        [SerializeField] private float maximalMatch = 1f;
        
        private CanvasScaler _сanvasScaler;

        private void Awake()
        {
            _сanvasScaler = GetComponent<CanvasScaler>();
            Refresh();
        }
        
        private void Refresh()
        {
            var width = Screen.width;
            var height = Screen.height;
            _сanvasScaler.matchWidthOrHeight = (float) width / height > targetAspect ? maximalMatch : minimalMatch;
            _сanvasScaler.scaleFactor = GetScale(width, height, _сanvasScaler);
        }
        
        private float GetScale(int width, int height, CanvasScaler canvasScaler)
        {
            var scalerReferenceResolution = canvasScaler.referenceResolution;
            var widthScale = width / scalerReferenceResolution.x;
            var heightScale = height / scalerReferenceResolution.y;
            var matchWidthOrHeight = canvasScaler.matchWidthOrHeight;
 
            return Mathf.Pow(widthScale, 1f - matchWidthOrHeight)*
                   Mathf.Pow(heightScale, matchWidthOrHeight);
        }
    }
}