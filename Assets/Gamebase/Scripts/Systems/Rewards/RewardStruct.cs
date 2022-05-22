using Sirenix.OdinInspector;
using System;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Структура одной награды.
    /// </summary>
    [Serializable]
    public struct RewardStruct
    {
        /// <summary>
        /// Тип награды
        /// </summary>
        [Tooltip("Тип награды")]
        public RewardType type;

        /// <summary>
        /// Количество, которое будет начислено
        /// </summary>
        [Tooltip("Количество, которое будет начислено")]
        [MinValue(0f)]
        public float value;

        /// <summary>
        /// Конструктор награды.
        /// </summary>
        /// <param name="type">Тип награды.</param>
        /// <param name="value">Количество, которое будет начислено.</param>
        public RewardStruct(RewardType type, float value)
        {
            this.type = type;
            this.value = Mathf.Clamp(value, 0, float.MaxValue);
        }
    }
}
