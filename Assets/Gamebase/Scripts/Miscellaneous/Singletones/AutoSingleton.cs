using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InvalidXmlDocComment
// ReSharper disable PossibleNullReferenceException
// ReSharper disable MemberCanBePrivate.Global

namespace Gamebase
{
    /// <summary>
    /// Позволяет быстро превращать любой класс в синглтон, через наследование от класса AutoSingleton<T>. От обычного Singleton<T> отличается логикой автоматического создания
    /// (его не нужно размещать на сцене, он будет создан при первом обращении) и
    /// отсутствием настраиваемых в инспекторе настроек (всегда инициализируется автоматически и всегда DontDestroyOnLoad)
    /// </summary>
    /// <typeparam name="T">Имя наследуемого класса</typeparam>
    public abstract class AutoSingleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// Метод, выполняемый при инициализации класса. Вызывается автоматически при создании экземпляра класса (недоступен извне)
        /// </summary>
        protected virtual void Initialize()
        {
        }

        protected virtual void Awake()
        {
            if (_instance == null) return;
            
            Debug.LogWarning($"[Singleton] Instance {typeof(T)} already exists. Destroying {name}...");
            DestroyImmediate(gameObject);
        }

        private static T _instance;
        /// <summary>
        /// Получить ссылку на экземпляр класса. Если экземпляр не существует, он будет автоматически создан
        /// </summary>
        public static T Instance
        {
            get
            {
                if (Exists) return _instance;
                
                var gameObject = new GameObject(typeof(T).ToString());
                DontDestroyOnLoad(gameObject);
                _instance = gameObject.AddComponent<T>();
                //Автоматическая инициализация при создании
                (_instance as AutoSingleton<T>).Initialize();
                return _instance;
            }
        }

        /// <summary>
        /// Проверить существование экземпляра класса
        /// </summary>
        public static bool Exists => _instance != null;
    }
}