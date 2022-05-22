using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
// ReSharper disable CheckNamespace
#pragma warning disable CS4014
#pragma warning disable CS0649

namespace Gamebase
{
    [DefaultExecutionOrder(1)]
    public class InitializationSequence : MonoBehaviour
    {
        [SerializeField] private GameObject projectSystems;
        [SerializeField] private GameStartPoint gameStartPoint;

        private static LoadingSceneProgressor Progressor => LoadingSceneProgressor.Instance;
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            var loadingSceneProgressTarget = FindObjectOfType<LoadingSceneProgressTarget>();
            if (loadingSceneProgressTarget != null)
            {
                Progressor.OnProgressChanged += loadingSceneProgressTarget.SetProgress;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            Sequence();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private async UniTask Sequence()
        {
            projectSystems.SetActive(true);
            
            await UniTask.Yield();

            var loadingSceneProgress = gameStartPoint.ProgressLoading;
            gameStartPoint.gameObject.SetActive(true);
            while (enabled)
            {
                Progressor.SetProgress(loadingSceneProgress);

                if (gameStartPoint.LoadingSceneOperation != null && gameStartPoint.LoadingSceneOperation.progress >= 0.9f)
                {
                    Progressor.SetProgress(1f);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                    gameStartPoint.StartScene();
                    break;
                }

                await UniTask.Yield(_cancellationTokenSource.Token);
            }
        }
    }
}
