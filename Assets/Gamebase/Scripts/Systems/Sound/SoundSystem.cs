using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
// ReSharper disable CheckNamespace
// ReSharper disable FunctionNeverReturns
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable CS4014
#pragma warning disable CS0649

namespace Gamebase
{
    /// <summary>
    /// Система звуков позволяет хранить все 2D звуковые эффекты и музыку с настройками в одном конфигурационном файле, а также предоставляет интерфейс для их воспроизведения, остановки и смешивания. Также система позволяет контролировать минимальную и максимальную задержки между воспроизведением двух вызванных одновременно звуковых эффектов.
    /// </summary>
    public class SoundSystem : IInitializable
    {
        [Inject]
        public SoundSystem(AudioSources audioSources)
        {
            _fxSource = audioSources.fxSource;
            _comboSequenceFxSource = audioSources.comboSource;
            _musicSource = audioSources.musicSource;
        }
        
        private readonly Queue<SoundWithTime> soundsQueue = new Queue<SoundWithTime>();

        public void Initialize()
        {
            MusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            SoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
            UpdateSequence();
        }
        
        private readonly AudioSource _fxSource;
        private readonly AudioSource _comboSequenceFxSource;
        private readonly AudioSource _musicSource;

        /// <summary>
        /// Настройки содержащие параметры и ссылки на все звуки
        /// </summary>
        private SoundSettings settings => SoundSettings.Instance;

        private bool soundOn;
        
        /// <summary>
        /// Статус активности звуков
        /// </summary>
        public bool SoundOn
        {
            get => soundOn;
            set
            {
                soundOn = value;
                _fxSource.outputAudioMixerGroup.audioMixer.SetVolume("VolumeFX", value ? 1f : 0f);
                _comboSequenceFxSource.outputAudioMixerGroup.audioMixer.SetVolume("VolumeFX", value ? 1f : 0f);
                PlayerPrefs.SetInt("SoundOn", value ? 1 : 0);
            }
        }

        private bool musicOn;
        
        /// <summary>
        /// Статус активности музыки
        /// </summary>
        public bool MusicOn
        {
            get => musicOn;
            set
            {
                if (value != musicOn)
                    _musicSource.time = 0f;

                _musicSource.outputAudioMixerGroup.audioMixer.SetVolume("VolumeMusic", value ? 1f : 0f);
                musicOn = value;
                PlayerPrefs.SetInt("MusicOn", value ? 1 : 0);
            }
        }
        
        private SoundWithTime workingSound;
        private float timer;
        private async UniTask UpdateSequence()
        {
            while (true)
            {
                await UniTask.Yield();
                if (soundsQueue.Count > 0)
                {
                    timer += Time.unscaledDeltaTime;
                    if (timer >= settings.minFXLatency)
                    {
                        workingSound = soundsQueue.Dequeue();
                        timer = 0f;
                        if (workingSound.unscaled || Time.unscaledTime - workingSound.time <= settings.maxFXLatency)
                        {
                            var sound = settings.GetSoundElement(workingSound.sound);
                            var clip = sound.clips[Random.Range(0, sound.clips.Length)];
                            _fxSource.PlayOneShot(clip, sound.volume);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Воспроизвести звук (немасштабируемый)
        /// </summary>
        /// <param name="sound">Звук (из прописанных в настройках SoundSystem)</param>
        public void PlaySoundUnscaled(Sound sound)
        {
            if (!SoundOn) return;

            var soundElement = settings.GetSoundElement(sound);
            var clip = soundElement.clips[Random.Range(0, soundElement.clips.Length)];
            _fxSource.PlayOneShot(clip, soundElement.volume);
        }

        /// <summary>
        /// Воспроизвести звук
        /// </summary>
        /// <param name="sound">Звук (из прописанных в настройках SoundSystem)</param>
        public void PlaySound(Sound sound)
        {
            if (!SoundOn) return;

            soundsQueue.Enqueue(new SoundWithTime(sound, Time.unscaledTime));
        }

        private Music playingMusic = Music.None;
        /// <summary>
        /// Запустить воспроизведение музыки
        /// </summary>
        /// <param name="music">Трек, которые требуется запустить (из прописанных в настройках SoundSystem)</param>
        /// <param name="loop">Требуется ли повтор (по умолчанию True)</param>
        public void PlayMusic(Music music, bool loop = true)
        {
            if (music == playingMusic) //Если данная музыка уже проигрывается
                return;
            PlayMusicCoroutine(music, loop);
        }

        /// <summary>
        /// Запускает две дорожки одновременно
        /// </summary>
        /// <param name="music">Первый трек, которые требуется запустить (из прописанных в настройках SoundSystem)</param>
        /// <param name="anotherMusic">Второй трек, которые требуется запустить (из прописанных в настройках SoundSystem)</param>
        /// <param name="loop">Требуется ли повтор (по умолчанию True)</param>
        /// <param name="fadePrevious">Остановить предыдущую дорожку после плавного затухания (по умолчанию True)</param>
        public void PlayMusic(Music music, Music anotherMusic, bool loop = true, bool fadePrevious = true)
        {
            if (music == playingMusic || music == anotherMusic) //Если данная музыка уже проигрывается
                return;
            PlayMusicCoroutine(music, anotherMusic, loop, fadePrevious);
        }

        /// <summary>
        /// Запускает один трек
        /// </summary>
        private async UniTask PlayMusicCoroutine(Music music, bool loop = true)
        {
            playingMusic = music;
            if (_musicSource.isPlaying)
            {
                await FadeCoroutine(_musicSource, settings.musicFadeOutLength);
            }

            StopMusic(_musicSource);

            _musicSource.clip = settings.GetMusicElement(music).musicClip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }

        /// <summary>
        /// Запускает два трека (первый трек будет считаться основным во всех условиях playingMusic)
        /// </summary>
        private async UniTask PlayMusicCoroutine(Music music, Music anotherMusic, bool loop = true, bool fadePrevious = true)
        {
            playingMusic = music;
            var sources = GetAllAudioSources(); //Получаем все AudioSource компоненты системы
            var secondMusicSource = sources.Length < 2 ? _musicSource.gameObject.AddComponent<AudioSource>() : sources[1]; //Создаем второй AudioSource, если ещё не имеем

            //Настраиваем второй audioSource по основному
            secondMusicSource.bypassEffects = _musicSource.bypassEffects;
            secondMusicSource.bypassListenerEffects = _musicSource.bypassListenerEffects;
            secondMusicSource.bypassReverbZones = _musicSource.bypassReverbZones;
            secondMusicSource.playOnAwake = _musicSource.playOnAwake;
            secondMusicSource.volume = _musicSource.volume;
            
            if (fadePrevious) //Необходимо ли заглушать предыдущую дорожку перед отключением
            {
                if (_musicSource.isPlaying)
                {
                    await FadeCoroutine(_musicSource, settings.musicFadeOutLength);
                }
            }

            StopMusic(_musicSource);

            _musicSource.clip = settings.GetMusicElement(music).musicClip;
            _musicSource.loop = loop;
            _musicSource.Play();

            secondMusicSource.clip = settings.GetMusicElement(anotherMusic).musicClip;
            secondMusicSource.loop = loop;
            secondMusicSource.Play();
        }

        /// <summary>
        /// Отключить с затуханием основную дорожку.
        /// </summary>
        public void FadeOutMusic()
        {
            FadeCoroutine(_musicSource, settings.musicFadeOutLength);
        }

        /// <summary>
        /// Отключает с затуханием выбранную музыку (если она включена). Полезно при одновременном запуске двух дорожек, чтобы отключить неосновную дорожку
        /// </summary>
        public void FadeOutMusic(Music music)
        {
            var sources = GetAllAudioSources();
            var source = sources.FirstOrDefault(x => x.clip == settings.GetMusicElement(music).musicClip);
            FadeCoroutine(source, settings.musicFadeOutLength);
        }

        private bool isFade;
        private readonly TimeSpan _fadeDelay = TimeSpan.FromSeconds(0.04f);
        private async UniTask FadeCoroutine(AudioSource source, float time)
        {
            if (isFade || source == null) return;

            isFade = true;
            var startVolume = _musicSource.volume;
            var start = startVolume;
            var end = startVolume * settings.minFadeVolumeMultiplier;
            var i = 0f;
            var step = 1f / time;

            while (i <= 1f)
            {
                i += step * Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(start, end, i);
                await UniTask.Delay(_fadeDelay, DelayType.Realtime);
            }

            source.volume = startVolume;
            isFade = false;
            StopMusic(source);
        }

        /// <summary>
        /// Проиграть звук повышающийся по тону, самый низкий (стандартной высоты) будет при параметре равном 0,
        /// затем при повышении параметра на 1 звук будет повышаться на полтона
        /// </summary>
        /// <param name="type">Звук (из прописанных в настройках SoundSystem)</param>
        /// <param name="semitoneCount">Высота звука (самый низкий (стандартной высоты) будет при параметре равном 0, затем при повышении параметра на 1 звук будет повышаться на полтона)</param>
        public void PlayComboSequenceSound(Sound type, int semitoneCount)
        {
            if (!SoundOn) return;

            _comboSequenceFxSource.pitch = Mathf.Pow(1.05946f, semitoneCount * 1);
            var sound = settings.GetSoundElement(type);
            var clip = sound.clips[Random.Range(0, sound.clips.Length)];

            _comboSequenceFxSource.PlayOneShot(clip, sound.volume);
        }

        /// <summary>
        /// Отключает основную дорожку
        /// </summary>
        public void StopMusic()
        {
            _musicSource.Stop();
        }

        /// <summary>
        /// Отключает выбранную дорожку (если она есть)
        /// </summary>
        public void StopMusic(AudioSource source)
        {
            if (source == null) return;
            source.Stop();
        }

        private AudioSource[] GetAllAudioSources()
        {
            var sources = _musicSource.gameObject.GetComponents<AudioSource>();
            return sources;
        }

        /// <summary>
        /// Установка паузы в звучании основной дорожки
        /// </summary>
        public void PauseMusic()
        {
            _musicSource.Pause();
        }

        /// <summary>
        /// Возобновление воспроизведения основной дорожки после паузы
        /// </summary>
        public void UnPauseMusic()
        {
            _musicSource.UnPause();
        }
    }
}