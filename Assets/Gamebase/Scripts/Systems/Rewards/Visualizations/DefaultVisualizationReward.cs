using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Стандартная визуализация награды на месте (плавное появление, подъем на высоту спрайта и исчезновение)
    /// </summary>
    public class DefaultVisualizationReward : IRewardVisualization
    {
        /// <summary>
        /// Конструктор стандартной анимации выдачи награды
        /// </summary>
        /// <param name="rewardType">Тип награды, иконку которой необходимо отобразить</param>
        /// <param name="parent">Объект в иерархии, который будет родителем спрайта награды (должен являться Canvas или
        /// находиться внутри него)</param>
        /// <param name="duration">Длительность отображения иконки в секундах</param>
        /// <param name="startPosition">Позиция появления награды (опционально, по умолчанию - центр экрана)</param>
        /// <param name="scale">Масштаб награды (опционально, по умолчанию награда имеет размер 100х100 и масштаб 1 по
        /// всем трем осям)</param>
        public DefaultVisualizationReward(RewardType rewardType, Transform parent, float duration, Vector3 startPosition = default, float scale = 1f)
        {
            _sprite = RewardsSystemSettings.Instance.GetSprite(rewardType);
            _parent = parent;
            _duration = duration;
            _startPosition = startPosition;
            _scale = scale;
        }

        private readonly Sprite _sprite;
        private readonly Transform _parent;
        private readonly float _duration;
        private readonly Vector3 _startPosition;
        private readonly float _scale;
        
        private GameObject _imageGameObject;
        private Image _image;

        private const string DEFAULT_IMAGE_NAME = "Reward";
        private const float FADE_DURATION = 0.2f;
        private const float DESTROY_DELAY = 0.1f;
        private readonly Vector2 _defaultSizeDelta = new Vector2(100f, 100f);
        private readonly Color _defaultColor = new Color(1f, 1f, 1f, 0f);
        
        /// <summary>
        /// Запустить анимацию выдачи награды
        /// </summary>
        public async UniTask Invoke()
        {
            _imageGameObject = CreateImage();
            var imageRect = _image.GetComponent<RectTransform>();

            DOTween.Sequence()
                .Append(_image.DOFade(1f, FADE_DURATION))
                .Join(imageRect.DOLocalMoveY(imageRect.localPosition.y + imageRect.sizeDelta.y, _duration))
                .Append(_image.DOFade(0f, FADE_DURATION));

            await UniTask.Delay(TimeSpan.FromSeconds(FADE_DURATION * 2 + _duration));

            DestroyImage();
        }

        private GameObject CreateImage()
        {
            var newImageGameObject = new GameObject(DEFAULT_IMAGE_NAME);
            newImageGameObject.transform.parent = _parent;

            _image = newImageGameObject.AddComponent<Image>();
            _image.rectTransform.anchoredPosition = Vector2.zero;
            _image.rectTransform.sizeDelta = _defaultSizeDelta;
            _image.color = _defaultColor;
            _image.sprite = _sprite;
            _image.raycastTarget = false;

            newImageGameObject.transform.position = _startPosition;
            newImageGameObject.transform.localScale = Vector3.one * _scale;

            return newImageGameObject;
        }

        private void DestroyImage()
        {
            _imageGameObject.SetActive(false);
            Object.Destroy(_imageGameObject, DESTROY_DELAY);
        }
    }
}
