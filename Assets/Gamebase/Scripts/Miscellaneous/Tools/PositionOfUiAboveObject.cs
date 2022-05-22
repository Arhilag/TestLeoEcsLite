using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Класс, позволяющий определить на UI точку, соответствующую позиции объекта в мировом пространстве.
    /// Может быть полезно для отображения, например, прогресс-баров здоровья юнитов на Canvas не в мировом пространсве,
    /// а на едином Canvas в Screen Space Camera.
    /// </summary>
    public class PositionOfUiAboveObject
    {
        /// <summary>
        /// Получить координаты AnchoredPosition
        /// </summary>
        /// <param name="camera">Камера, используемая для рассчета проекции</param>
        /// <param name="canvasRectTransform">RectTransform, который используется на Canvas, на который требуется
        /// осуществить проекцию</param>
        /// <param name="targetTransform">Transform объекта, проекцию которого требуется найти</param>
        /// <returns>Координаты AnchoredPosition на Canvas, соответствующие позиции объекта в мировом
        /// пространстве.</returns>
        public static Vector2 GetAnchoredPosition(
            Camera camera, 
            RectTransform canvasRectTransform,
            Transform targetTransform)
        {
            var sizeDelta = canvasRectTransform.sizeDelta;
            var viewportPosition = camera.WorldToViewportPoint(targetTransform.position);
            return new Vector2(
                viewportPosition.x * sizeDelta.x - sizeDelta.x * 0.5f,
                viewportPosition.y * sizeDelta.y - sizeDelta.y * 0.5f);
        }
    }
}