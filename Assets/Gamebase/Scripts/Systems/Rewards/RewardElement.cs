using System;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    [Serializable]
    public class RewardElement
    {
        [Tooltip("Название награды (на английском, без пробелов и специальных символов)")]
        public string name = "";

        [Tooltip("Тип ресурса, который будет пополняться данной наградой")]
        public ResourceType resourceType;

        [Tooltip("Иконка награды")]
        public Sprite sprite;
    }
}