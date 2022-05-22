using Sirenix.OdinInspector;
using System.Collections.Generic;
using Gamebase;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Global
// ReSharper disable RedundantTypeArgumentsOfMethod

public class ProgressSettings : GamebaseSystemSettings<ProgressSettings>
{
    [Title("Настройки системы прогресса")]
    [InfoBox("Система прогресса предоставляет интерфейс для работы с уровнем игрового аватара и опытом, а также с разблокируемыми фичами игры, которые должны открываться только по достижении игровым аватаром определенного уровня.", InfoMessageType.None)]
    
    [Tooltip("Использовать опыт")]
    public bool UseXP;
    
    [ShowIf("UseXP")]
    public bool OpenLevelsWithXP;
    
    [ShowIf("UseXP")]
    public bool ClearXPAfterEachLevel;
    
    [ShowIf("UseXP")]
    public List<ProgressLevelSetting> levelSettings;
    
    [OnValueChanged("ValueChanged", true)]
    [ValidateInput("ValidateStrings", "Элементы могут содержать только латинские символы и цифры и начинаться обязательно с буквы!")]
    [ValidateInput("ValidateDuplicate", "Имеются повторяющиеся элементы!")]
    public List<GameFeatureSetting> featureSettings;

    #region Validation
    protected override bool ValidateAll => valueChanged && ValidateStrings(featureSettings) && ValidateDuplicate(featureSettings);

    private bool ValidateStrings(List<GameFeatureSetting> list)
    {
        return ValidateStringsBase<GameFeatureSetting>(list, x => x.Name);
    }

    private bool ValidateDuplicate(List<GameFeatureSetting> list)
    {
        return ValidateDuplicateBase<GameFeatureSetting>(list, x => x.Name);
    }
    #endregion

#if UNITY_EDITOR
    /// <summary>
    /// Пересборка файла с перечислением
    /// </summary>
    [ContextMenu("Manual Rebuild Enum Code")]
    public override void RebuildEnumCode()
    {
        RebuidEnumCodeBase<GameFeatureSetting>("GameFeatureType", featureSettings, x => x.Name);
    }
#endif

    public GameFeatureSetting GetFeatureSetting(GameFeatureType type)
    {
        foreach (var item in featureSettings)
        {
            if (item.Name.GetHashCode() == type.GetHashCode())
                return item;
        }

        DebugSystem.LogError($"[ProgressSettings] - Не найдена фича типа {type}!");
        return null;
    }
}