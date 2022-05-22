using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable Unity.InefficientPropertyAccess

namespace Gamebase
{
    /// <summary>
    /// Расширение классов Transform и GameObject на возможность напрямую менять ReadOnly свойства Transform.
    /// </summary>
    public static class TransformExtensions
    {
        #region Установка глобальных значений
        
        /// <summary>
        /// Изменить значение позиции по X
        /// </summary>
        /// <param name="tr">Transform объекта</param>
        /// <param name="newValue">Новое значение</param>
        public static void X(this Transform tr, float newValue)
        {
            tr.position = new Vector3(newValue, tr.position.y, tr.position.z);
        }

        /// <summary>
        /// Изменить значение позиции по Y
        /// </summary>
        /// <param name="tr">Transform объекта</param>
        /// <param name="newValue">Новое значение</param>
        public static void Y(this Transform tr, float newValue)
        {
            tr.position = new Vector3(tr.position.x, newValue, tr.position.z);
        }

        /// <summary>
        /// Изменить значение позиции по Z
        /// </summary>
        /// <param name="tr">Transform объекта</param>
        /// <param name="newValue">Новое значение</param>
        public static void Z(this Transform tr, float newValue)
        {
            tr.position = new Vector3(tr.position.x, tr.position.y, newValue);
        }

        /// <summary>
        /// Изменить значение позиции по X
        /// </summary>
        /// <param name="go">Объект</param>
        /// <param name="newValue">Новое значение</param>
        public static void X(this GameObject go, float newValue)
        {
            go.transform.position = new Vector3(newValue, go.transform.position.y, go.transform.position.z);
        }

        /// <summary>
        /// Изменить значение позиции по Y
        /// </summary>
        /// <param name="go">Объект</param>
        /// <param name="newValue">Новое значение</param>
        public static void Y(this GameObject go, float newValue)
        {
            go.transform.position = new Vector3(go.transform.position.x, newValue, go.transform.position.z);
        }

        /// <summary>
        /// Изменить значение позиции по Z
        /// </summary>
        /// <param name="go">Объект</param>
        /// <param name="newValue">Новое значение</param>
        public static void Z(this GameObject go, float newValue)
        {
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, newValue);
        }
        
        #endregion

        #region Получение глобальных значений
        
        /// <summary>
        /// Получить значение позиции по X
        /// </summary>
        /// <param name="tr">Трансформ объекта</param>
        /// <returns>Значение позиции по X</returns>
        public static float X(this Transform tr)
        {
            return tr.position.x;
        }

        /// <summary>
        /// Получить значение позиции по Y
        /// </summary>
        /// <param name="tr">Трансформ объекта</param>
        /// <returns>Значение позиции по Y</returns>
        public static float Y(this Transform tr)
        {
            return tr.position.y;
        }

        /// <summary>
        /// Получить значение позиции по Z
        /// </summary>
        /// <param name="tr">Трансформ объекта</param>
        /// <returns>Значение позиции по Z</returns>
        public static float Z(this Transform tr)
        {
            return tr.position.z;
        }

        /// <summary>
        /// Получить значение позиции по X
        /// </summary>
        /// <param name="go">Объект</param>
        /// <returns>Значение позиции по X</returns>
        public static float X(this GameObject go)
        {
            return go.transform.position.x;
        }

        /// <summary>
        /// Получить значение позиции по Y
        /// </summary>
        /// <param name="go">Объект</param>
        /// <returns>Значение позиции по Y</returns>
        public static float Y(this GameObject go)
        {
            return go.transform.position.y;
        }

        /// <summary>
        /// Получить значение позиции по Z
        /// </summary>
        /// <param name="go">Объект</param>
        /// <returns>Значение позиции по Z</returns>
        public static float Z(this GameObject go)
        {
            return go.transform.position.z;
        }

        #endregion

        #region Установка локальных значений

        /// <summary>
        /// Изменить значение локальной позиции по X
        /// </summary>
        /// <param name="tr">Transform объекта</param>
        /// <param name="newValue">Новое значение</param>
        public static void LocalX(this Transform tr, float newValue)
        {
            tr.localPosition = new Vector3(newValue, tr.localPosition.y, tr.localPosition.z);
        }

        /// <summary>
        /// Изменить значение локальной позиции по Y
        /// </summary>
        /// <param name="tr">Transform объекта</param>
        /// <param name="newValue">Новое значение</param>
        public static void LocalY(this Transform tr, float newValue)
        {
            tr.localPosition = new Vector3(tr.localPosition.x, newValue, tr.localPosition.z);
        }

        /// <summary>
        /// Изменить значение локальной позиции по Z
        /// </summary>
        /// <param name="tr">Transform объекта</param>
        /// <param name="newValue">Новое значение</param>
        public static void LocalZ(this Transform tr, float newValue)
        {
            tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, newValue);
        }

        /// <summary>
        /// Изменить значение локальной позиции по X
        /// </summary>
        /// <param name="go">Объект</param>
        /// <param name="newValue">Новое значение</param>
        public static void LocalX(this GameObject go, float newValue)
        {
            go.transform.localPosition = new Vector3(newValue, go.transform.localPosition.y, go.transform.localPosition.z);
        }

        /// <summary>
        /// Изменить значение локальной позиции по Y
        /// </summary>
        /// <param name="go">Объект</param>
        /// <param name="newValue">Новое значение</param>
        public static void LocalY(this GameObject go, float newValue)
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, newValue, go.transform.localPosition.z);
        }

        /// <summary>
        /// Изменить значение локальной позиции по Z
        /// </summary>
        /// <param name="go">Объект</param>
        /// <param name="newValue">Новое значение</param>
        public static void LocalZ(this GameObject go, float newValue)
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, newValue);
        }
        
        #endregion

        #region Получение локальных значений

        /// <summary>
        /// Получить значение локальной позиции по X
        /// </summary>
        /// <param name="tr">Трансформ объекта</param>
        /// <returns>Значение локальной позиции по X</returns>
        public static float LocalX(this Transform tr)
        {
            return tr.localPosition.x;
        }

        /// <summary>
        /// Получить значение локальной позиции по Y
        /// </summary>
        /// <param name="tr">Трансформ объекта</param>
        /// <returns>Значение локальной позиции по Y</returns>
        public static float LocalY(this Transform tr)
        {
            return tr.localPosition.y;
        }

        /// <summary>
        /// Получить значение локальной позиции по Z
        /// </summary>
        /// <param name="tr">Трансформ объекта</param>
        /// <returns>Значение локальной позиции по Z</returns>
        public static float LocalZ(this Transform tr)
        {
            return tr.localPosition.z;
        }

        /// <summary>
        /// Получить значение локальной позиции по X
        /// </summary>
        /// <param name="go">Объект</param>
        /// <returns>Значение локальной позиции по X</returns>
        public static float LocalX(this GameObject go)
        {
            return go.transform.localPosition.x;
        }

        /// <summary>
        /// Получить значение локальной позиции по Y
        /// </summary>
        /// <param name="go">Объект</param>
        /// <returns>Значение локальной позиции по Y</returns>
        public static float LocalY(this GameObject go)
        {
            return go.transform.localPosition.y;
        }

        /// <summary>
        /// Получить значение локальной позиции по Z
        /// </summary>
        /// <param name="go">Объект</param>
        /// <returns>Значение локальной позиции по Z</returns>
        public static float LocalZ(this GameObject go)
        {
            return go.transform.localPosition.z;
        }
        
        #endregion
    }
}