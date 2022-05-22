using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    [TypeInfoBox("Точка входа в игру. Должно быть на самой первой сцене, где инициализируются все системы")]
    public class GameStartPoint : MonoBehaviour
    {
        public static bool IsInitialized { get; private set; }

        private static ScenesSettings Settings => ScenesSettings.Instance;

        private SceneField FirstScene => Settings.FirstScene;
        private bool AutoStartLoading => Settings.autoStartLoading;
        private LoadSceneMode LoadFirstSceneMode => Settings.loadFirstSceneMode;
        public AsyncOperation LoadingSceneOperation { get; private set; }
        public float ProgressLoading => LoadingSceneOperation?.progress ?? 0f;
        
        private string _goToSceneName;

        public void Awake()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;

                if (PlayModeController.IsForwardingToInitialization)
                {
                    _goToSceneName = PlayModeController.GetBackToSceneName;
                    PlayModeController.FinalizeForwarding();
                }
            }
        }
        
        private void Start()
        {
            if (!string.IsNullOrEmpty(_goToSceneName))
            {
                SceneManager.LoadSceneAsync(_goToSceneName);
                _goToSceneName = null;
                return;
            }

            if (AutoStartLoading)
            {
                LoadingSceneOperation = SceneManager.LoadSceneAsync(FirstScene, LoadFirstSceneMode);
                LoadingSceneOperation.allowSceneActivation = false;
            }
        }

        public void StartScene()
        {
            if (LoadingSceneOperation != null)
            {
                LoadingSceneOperation.allowSceneActivation = true;
            }
        }
    }
}