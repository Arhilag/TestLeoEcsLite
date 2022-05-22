using UnityEngine;
#pragma warning disable CS0108
// ReSharper disable InvalidXmlDocComment
// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBeProtected.Global

namespace Gamebase
{
    /// <summary>
    /// Позволяет быстро превращать любой класс в синглтон, через наследование от класса Singleton<T>
    /// </summary>
    /// <typeparam name="T">Имя наследуемого класса</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        [Tooltip("Автоматически инициализировать класс при загрузке")]
        [SerializeField]
        private bool autoInitializeOnStart = false;

        [Tooltip("Не уничтожать объект при переключении на другую сцену")]
        [SerializeField]
        private bool dontDestroyOnLoad = false;

        /// <summary>
        /// Метод, выполняемый при инициализации класса
        /// </summary>
        public virtual void Initialize()
        {
        }

        protected virtual void Start()
        {
            if (autoInitializeOnStart)
            {
                Initialize();
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                if (this is T)
                {
                    _instance = this as T;
                    if (dontDestroyOnLoad && Application.isPlaying)
                    {
                        DontDestroyOnLoad(gameObject);
                    }
                }
            }
            else if (Application.isPlaying)
            {
                Debug.LogWarning($"[Singleton] Instance {typeof(T)} already exists. Destroying {name}...");
                DestroyImmediate(gameObject);
            }

        }

        private static T _instance;
        /// <summary>
        /// Получить ссылку на экземпляр класса
        /// </summary>
        public static T Instance => _instance;

        /// <summary>
        /// Проверить существование класса
        /// </summary>
        public static bool Exists => _instance != null;
    }
}