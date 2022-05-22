using System;
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global

namespace Gamebase
{
    /// <summary>
    /// Обертка вокруг поля со встроенным событием на его изменение
    /// </summary>
    /// <typeparam name="T">Тип поля</typeparam>
    public class EventField<T>
    {
        private T _value;
        
        /// <summary>
        /// Значение поля
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
        
        /// <summary>
        /// Событие изменения поля
        /// </summary>
        public event Action<T> OnValueChanged;
        
        public static implicit operator T(EventField<T> param) => param.Value;
    }
}