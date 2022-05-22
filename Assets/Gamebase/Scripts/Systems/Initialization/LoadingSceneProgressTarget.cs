using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingSceneProgressTarget : MonoBehaviour
    {
        [SerializeField] private float durationFade = 0.25f;
        [ShowInInspector] [ReadOnly] public float Progress { get; private set; }

        private CanvasGroup canvasGroup;
        private const float TOLERANCE = float.Epsilon;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Progress = 0f;
        }

        internal async void SetProgress(float value)
        {
            Progress = value;

            if (Math.Abs(Progress - 1f) < TOLERANCE)
            {
                DOTween.Sequence().Append(canvasGroup.DOFade(0f, durationFade));
                await Task.Delay(TimeSpan.FromSeconds(durationFade));
                if (canvasGroup)
                {
                    canvasGroup.gameObject.SetActive(false);
                }
            }
        }

    }
}