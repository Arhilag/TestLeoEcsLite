using System;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable CheckNamespace
#pragma warning disable CS4014

namespace Gamebase
{
    public class InitializationSceneLoader : MonoBehaviour
    {
        private static ScenesSettings Settings => ScenesSettings.Instance;

        [Tooltip("Задержка, после которой начнется загрузка сцены инициализации Gamebase")]
        [MinValue(0f)]
        [SerializeField] private float delay = 0.25f;

        private void Awake()
        {
            StartInitializationScene();
        }

        private async UniTask StartInitializationScene()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            SceneManager.LoadSceneAsync(Settings.InitializationScene, LoadSceneMode.Additive);
        }
    }
}
