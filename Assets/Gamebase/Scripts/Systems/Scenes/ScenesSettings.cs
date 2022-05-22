using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Gamebase
{
    public class ScenesSettings : StaticScriptableObject<ScenesSettings>
    {
        [Title("Настройки сцен")]
        [InfoBox("Здесь можно настроить сцены, которые будут загружаться автоматически (рекомендуется оставить значения по умолчанию, которые были прописаны при инициализации Gamebase).", InfoMessageType.None)]
        [InfoBox("Сцена предзагрузки с логотипом игры")]
        [SerializeField]
        public SceneField LoadingScene;
        [InfoBox("Сцена загрузки модулей Gamebase")]
        [SerializeField]
        public SceneField InitializationScene;
        [InfoBox("Сцена, которая будет запускаться после инициализации Gamebase по умолчанию")]
        [SerializeField]
        public SceneField FirstScene;

        [Title("Настройки сцены загрузки модулей Gamebase")]
        [Tooltip("Нужно ли сразу загружать следующую сцену")]
        [SerializeField]
        public bool autoStartLoading = true;
        [Tooltip("Режим загрузки первой сцены")]
        [SerializeField]
        public LoadSceneMode loadFirstSceneMode = LoadSceneMode.Single;
    }
}
