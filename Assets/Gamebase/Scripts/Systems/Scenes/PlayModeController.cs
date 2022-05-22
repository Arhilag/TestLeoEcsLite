using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
#pragma warning disable CS0649

namespace Gamebase
{
    [DefaultExecutionOrder(-150000)]
    [TypeInfoBox("При запуске игры с этой сцены - автоматически перейдет на первую сцену (для возможности запускать игру с любой сцены)")]
    public class PlayModeController : MonoBehaviour
    {
        private static ScenesSettings settings => ScenesSettings.Instance;
        private SceneField InitializationScene => settings.InitializationScene;
        
        [Tooltip("Требуется ли переходить после инициализации сразу на текущую сцену")]
        [SerializeField] private bool GetBackHere;

        public static bool IsForwardingToInitialization { get; private set; }
        public static string GetBackToSceneName;

        private void Awake()
        {
            if (!GameStartPoint.IsInitialized)
            {
                IsForwardingToInitialization = true;

                if (GetBackHere)
                    GetBackToSceneName = SceneManager.GetActiveScene().name;

                //Отключаем все объекты на сцене, чтобы в них не вызвался Awake
                var allobjects = FindObjectsOfType<GameObject>();
                foreach (var component in allobjects)
                {
                    component.SetActive(false);
                }

                SceneManager.LoadScene(InitializationScene);
            }
        }

        public static void FinalizeForwarding()
        {
            IsForwardingToInitialization = false;
            GetBackToSceneName = null;
        }
    }
}