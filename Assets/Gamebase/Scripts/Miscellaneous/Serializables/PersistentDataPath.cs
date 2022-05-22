using System.IO;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Класс, предоставляющий адрес данных директории игры
    /// </summary>
    public static class PersistentDataPath
    {
        private static string _value;
        private static bool _initialized;

        /// <summary>
        /// Адрес данных директории игры
        /// </summary>
        public static string Value
        {
            get
            {
                if (_initialized) return _value;
                
                _value = Application.persistentDataPath;
                if (!Directory.Exists(_value))
                {
                    Directory.CreateDirectory(_value);
                }
                _initialized = true;
                return _value;
            }
        }
    }
}