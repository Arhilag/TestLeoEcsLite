using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Класс для "статического" экземпляра ScriptableObject, который должен храниться в папке Resources/Settings.
    /// Класс наследуемый от StaticScriptableObject будет доступен в коде с помощью метода Instance.
    /// </summary>
    /// <typeparam name="T">Имя наследуемого класса (имя ассета должно быть на английском, без пробелов и специальных символов)</typeparam>
    public class StaticScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        /// <summary>
        /// Получить ссылку на экземпляр ScriptableObject
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var path = $"Settings/{typeof(T).Name}";
                    _instance = Resources.Load(path) as T;
                }

                return _instance;
            }
        }
    }
}