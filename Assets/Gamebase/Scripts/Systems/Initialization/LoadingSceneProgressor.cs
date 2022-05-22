using Sirenix.OdinInspector;
using System;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Gamebase
{
    /// <summary>
    /// Класс хранит прогресс, позволяет пополнять его извне и получать новое значение по событию.
    /// Прогресс является числом типа float от 0 до 1.
    /// </summary>
    public class LoadingSceneProgressor : Singleton<LoadingSceneProgressor>
    {
        /// <summary>
        /// Подписаться на событие изменения значения прогресса
        /// </summary>
        public Action<float> OnProgressChanged;

        [ShowInInspector]
        [ReadOnly]
        private float progress;
        private float Progress
        {
            get => progress;
            set
            {
                progress = Mathf.Clamp(value, 0f, 1f);
                OnProgressChanged?.Invoke(progress);
            }
        }

        public override void Initialize()
        {
            Progress = 0;
        }

        private void OnDestroy()
        {
            OnProgressChanged = null;
        }

        /// <summary>
        /// Добавить значение к текущему прогрессу
        /// </summary>
        /// <param name="value"></param>
        public void AddProgress(float value)
        {
            Progress += value;
        }

        /// <summary>
        /// Установить конкретное значение прогресса
        /// </summary>
        /// <param name="value"></param>
        public void SetProgress(float value)
        {
            Progress = value;
        }

        /// <summary>
        /// Получить текущее значение прогресс
        /// </summary>
        /// <returns></returns>
        public float GetProgress()
        {
            return Progress;
        }
    }
}
