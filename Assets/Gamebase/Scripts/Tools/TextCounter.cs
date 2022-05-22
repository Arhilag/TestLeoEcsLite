using System;
using Doozy.Engine.Progress;
using TMPro;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable AccessToModifiedClosure

namespace Gamebase
{
    /// <summary>
    /// Подписчик изменения количества целочисленного ресурса
    /// </summary>
    [RequireComponent(typeof(Progressor))]
    [RequireComponent(typeof(ProgressTargetTextMeshPro))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextCounter : MonoBehaviour
    {
        private Progressor textProgressor;
        private Func<int> _getCount;

        private void Awake()
        {
            textProgressor = GetComponent<Progressor>();
        }
        
        /// <summary>
        /// Инициализировать счетчик.
        /// Пример инициализации: textCounter.Initialize(() => CoinsCount, ref OnCoinsCountChanged);
        /// </summary>
        /// <param name="countGetter">Начальное значение счетчика</param>
        /// <param name="updateAction">Событие изменение счетчика</param>
        public void Initialize(Func<int> countGetter, ref Action<int> updateAction)
        {
            _getCount = countGetter;
            //Событие вызываемое при обновлении значения
            Action<int> onUpdate = null;

            //Создаем копию т.к. нельзя передавать в лямбда выражения ref параметры
            var updateActionCopy = updateAction;
            onUpdate = (v) =>
            {
                if (this != null)
                {
                    Refresh(v);
                }
                //Если объект счетчика уже не существует - отписываемся
                else
                {
                    updateActionCopy -= onUpdate;
                }
            };
            //Подписываемся на событие обновления значения
            updateActionCopy += onUpdate;
            //Перезаписываем ref параметр копией
            updateAction = updateActionCopy;

            Refresh(_getCount());
        }

        private void Refresh(int value)
        {
            if (textProgressor == null)
            {
                textProgressor = GetComponent<Progressor>();
            }
            
            textProgressor.SetValue(value);
        }

        /// <summary>
        /// Ручное первое обновление
        /// </summary>
        /// <param name="value">Значение, которое необходимо установить</param>
        public void FirstRefresh(int value)
        {
            if (textProgressor == null)
            {
                textProgressor = GetComponent<Progressor>();
            }
            
            textProgressor.SetValue(value, true);
        }

        /// <summary>
        /// Отписать счетчик от события
        /// </summary>
        /// <param name="updateAction">Событие, на которое счетчик был подписан</param>
        public void Unsubscribe(ref Action<int> updateAction)
        {
            updateAction -= Refresh;
        }
    }
}
