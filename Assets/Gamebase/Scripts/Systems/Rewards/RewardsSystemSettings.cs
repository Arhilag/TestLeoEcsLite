using Gamebase;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable CheckNamespace

/// <summary>
/// Настройки системы наград
/// </summary>
public class RewardsSystemSettings : GamebaseSystemSettings<RewardsSystemSettings>
{
    #pragma warning disable CS0649
    [Title("Настройки системы наград")]
    [InfoBox("Система позволяет начислять игроку сформированную структуру ресурсов разом. Может использоваться для сетов в магазине, начисления нескольких бонусов одновременно за выполнение какого-либо действия и т.д.", InfoMessageType.None)]
    [Tooltip("Награды"), OnValueChanged("ValueChanged", true)]
    [ValidateInput("ValidateStrings", "Элементы могут содержать только латинские символы и цифры и начинаться обязательно с буквы!")]
    [ValidateInput("ValidateDuplicate", "Имеются повторяющиеся элементы!")]
    [SerializeField] private RewardElement[] rewards;
    #pragma warning restore CS0649

    #region Validation
    protected override bool ValidateAll => valueChanged && ValidateStrings(rewards) && ValidateDuplicate(rewards);

    private bool ValidateStrings(RewardElement[] list)
    {
        return ValidateStringsBase<RewardElement>(list.ToList(), x => x.name);
    }

    private bool ValidateDuplicate(RewardElement[] list)
    {
        return ValidateDuplicateBase<RewardElement>(list.ToList(), x => x.name);
    }
    #endregion

    /// <summary>
    /// Получить тип ресурса по типу награды
    /// </summary>
    /// <param name="type">Тип награды</param>
    /// <returns></returns>
    public ResourceType GetResourceType(RewardType type)
    {
        foreach (var item in rewards)
        {
            if (item.name.GetHashCode() == type.GetHashCode())
                return item.resourceType;
        }

        DebugSystem.LogError($"[RewardsSystemSettings] - A Reward with the {type} type was not found");
        return ResourceType.None;
    }

    /// <summary>
    /// Получить спрайт награды
    /// </summary>
    /// <param name="type">Тип награды</param>
    /// <returns></returns>
    public Sprite GetSprite(RewardType type)
    {
        foreach (var item in rewards)
        {
            if (item.name.GetHashCode() == type.GetHashCode())
                return item.sprite;
        }

        DebugSystem.LogError($"[RewardsSystemSettings] - A Reward with the {type} type was not found");
        return null;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Пересборка файла с перечислением
    /// </summary>
    [ContextMenu("Manual Rebuild Enum Code")]
    public override void RebuildEnumCode()
    {
        RebuidEnumCodeBase<RewardElement>("RewardTypeEnum", rewards.ToList(), x => x.name);
    }
#endif
}