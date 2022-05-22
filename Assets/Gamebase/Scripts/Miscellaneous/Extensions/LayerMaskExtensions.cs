using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Класс, позволяющий проверить принадлежность слоя нужному
    /// </summary>
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Проверить принадлежность к маске.
        /// Пример использования:
        /// other.gameObject.layer.WithinMask(settings.layerMask)
        /// </summary>
        /// <param name="layer">Layer проверяемого объекта</param>
        /// <param name="mask">LayerMask, на принадлежность к которому нужно проверить объект</param>
        /// <returns></returns>
        public static bool WithinMask(this int layer, LayerMask mask)
        {
            return mask.LayerMaskContains(layer);
        }
        
        private static bool LayerMaskContains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}