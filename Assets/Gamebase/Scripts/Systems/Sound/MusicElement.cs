using System;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    [Serializable]
    public class MusicElement
    {
        [Tooltip("Название музыки (на английском, без пробелов и специальных символов)")]
        public string name = "";
        
        [Tooltip("Звуковой клип")]
        public AudioClip musicClip;
    }
}