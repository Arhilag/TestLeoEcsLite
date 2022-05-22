using Sirenix.OdinInspector;
using System.Linq;
using Gamebase;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable RedundantTypeArgumentsOfMethod
#pragma warning disable CS0649

/// <summary>
/// Содержит настройки звуковой системы
/// </summary>
public class SoundSettings : GamebaseSystemSettings<SoundSettings>
{
    [Title("Настройки звуков")]
    [InfoBox("Система позволяет хранить все 2D звуковые эффекты и музыку с настройками в одном конфигурационном файле, а также предоставляет интерфейс для их воспроизведения, остановки и смешивания.", InfoMessageType.None)]
    
    [Tooltip("Минимальная задержка между двумя звуками")]
    public float minFXLatency = 0.1f;
    
    [Tooltip("Максимальная задержка между двумя звуками. Если запрос на звук был в момент времени Х, а его очередь наступила только к Х+maxFXLatency, то звук воспроизведен не будет")]
    public float maxFXLatency = 0.5f;
    
    [Tooltip("Длительность фейда затухания музыки")]
    public float musicFadeOutLength = 1.5f;
    
    [Tooltip("Минимальное значение громкости музыки на котором заканчивается фейд и включается следующая композиция")]
    public float minFadeVolumeMultiplier = 0.2f;

    [Tooltip("Звуковые эффекты"), OnValueChanged("ValueChanged", true)]
    [ValidateInput("ValidateStringsSounds", "Элементы могут содержать только латинские символы и цифры и начинаться обязательно с буквы!")]
    [ValidateInput("ValidateDuplicateSounds", "Имеются повторяющиеся элементы!")]
    [SerializeField] private SoundElement[] sounds;
    
    [Tooltip("Музыкальные композиции"), OnValueChanged("ValueChanged", true)]
    [ValidateInput("ValidateStringsMusic", "Элементы могут содержать только латинские символы и цифры и начинаться обязательно с буквы!")]
    [ValidateInput("ValidateDuplicateMusic", "Имеются повторяющиеся элементы!")]
    [SerializeField] private MusicElement[] music;

    #region Validation

    protected override bool ValidateAll => valueChanged &&
                                           ValidateStringsSounds(sounds) &&
                                           ValidateDuplicateSounds(sounds) &&
                                           ValidateStringsMusic(music) &&
                                           ValidateDuplicateMusic(music);

    private bool ValidateStringsSounds(SoundElement[] list)
    {
        return ValidateStringsBase<SoundElement>(list.ToList(), x => x.name);
    }

    private bool ValidateDuplicateSounds(SoundElement[] list)
    {
        return ValidateDuplicateBase<SoundElement>(list.ToList(), x => x.name);
    }

    private bool ValidateStringsMusic(MusicElement[] list)
    {
        return ValidateStringsBase<MusicElement>(list.ToList(), x => x.name);
    }

    private bool ValidateDuplicateMusic(MusicElement[] list)
    {
        return ValidateDuplicateBase<MusicElement>(list.ToList(), x => x.name);
    }
    #endregion

    public SoundElement GetSoundElement(Sound soundType)
    {
        foreach (var item in sounds)
        {
            if (item.name.GetHashCode() == soundType.GetHashCode())
                return item;
        }
        DebugSystem.LogError($"[SoundSettings] - Не найден звук типа {soundType}!");
        return null;
    }

    public MusicElement GetMusicElement(Music musicType)
    {
        foreach (var item in music)
        {
            if (item.name.GetHashCode() == musicType.GetHashCode())
                return item;
        }
        DebugSystem.LogError($"[SoundSettings] - Не найдена музыка типа {musicType}!");
        return null;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Пересборка файла с перечислением
    /// </summary>
    [ContextMenu("Manual Rebuild Enum Code")]
    public override void RebuildEnumCode()
    {
        RebuidEnumCodeBase<SoundElement>("SoundEnum", sounds.ToList(), x => x.name);
        RebuidEnumCodeBase<MusicElement>("MusicEnum", music.ToList(), x => x.name);
    }
#endif
}