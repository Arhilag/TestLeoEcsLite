using System;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    [Serializable]
    public class SoundElement
    {
        [Tooltip("Название звукового эффекта (на английском, без пробелов и специальных символов)")]
        public string name = "";
        
        [Tooltip("Звуковой клип(клипы)")]
        public AudioClip[] clips;
        
        [Tooltip("Громкость звука")]
        [Range(0f, 1f)]
        public float volume = 1f;
    }
}