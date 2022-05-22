using UnityEngine;
using UnityEngine.Audio;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    public static class AudioExtensions
    {
        /// <summary>
        /// Установить громкость AudioMixer
        /// </summary>
        /// <param name="mixer">Миксер</param>
        /// <param name="exposedName">The name of 'The Exposed to Script' variable</param>
        /// <param name="value">value must be between 0 and 1</param>
        public static void SetVolume(this AudioMixer mixer, string exposedName, float value)
        {
            mixer.SetFloat(exposedName, Mathf.Lerp(-80.0f, 0.0f, Mathf.Clamp01(value)));
        }

        /// <summary>
        /// Получить громкость AudioMixer
        /// </summary>
        /// <param name="mixer">Миксер</param>
        /// <param name="exposedName">The name of 'The Exposed to Script' variable</param>
        /// <returns>Значение громкости в диапазоне от 0 до 1</returns>
        public static float GetVolume(this AudioMixer mixer, string exposedName)
        {
            if (mixer.GetFloat(exposedName, out float volume))
            {
                return Mathf.InverseLerp(-80.0f, 0.0f, volume);
            }

            return 0f;
        }
    }
}