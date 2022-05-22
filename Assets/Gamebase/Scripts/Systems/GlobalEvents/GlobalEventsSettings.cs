using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Gamebase;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable RedundantTypeArgumentsOfMethod

public class GlobalEventsSettings : GamebaseSystemSettings<GlobalEventsSettings>
{
    [Title("Настройки глобальных событий")]
    [InfoBox("Система дает функционал для подписывания и вызова событий. Использовать разумно, не все события в игре стоит обрабатывать через систему глобальных событий.", InfoMessageType.None)]

    [Tooltip("Список глобальных событий")]
    [InfoBox("События ProgressLevelChanged и XPChanged удалить нельзя, т.к. они используются в Progress System.")]
    [OnValueChanged("ValueChanged", true)]
    [ValidateInput("ValidateStrings", "Элементы могут содержать только латинские символы и цифры и начинаться обязательно с буквы!")]
    [ValidateInput("ValidateDuplicate", "Имеются повторяющиеся элементы!")]
    [SerializeField]
    private List<GlobalEventElement> events = new List<GlobalEventElement>
    {
        new GlobalEventElement("ProgressLevelChanged"),
        new GlobalEventElement("XPChanged")
    };
    
    protected override void Save()
    {
        // Принудительное прописывание необходимых в других системах событий
        if (events.All(x => x.name != "ProgressLevelChanged"))
            events.Add(new GlobalEventElement("ProgressLevelChanged"));
        if (events.All(x => x.name != "XPChanged"))
            events.Add(new GlobalEventElement("XPChanged"));

        base.Save();
    }

    #region Validation

    protected override bool ValidateAll => valueChanged && ValidateStrings(events) && ValidateDuplicate(events);
    
    private bool ValidateStrings(List<GlobalEventElement> list)
    {
        return ValidateStringsBase<GlobalEventElement>(list, x => x.name);
    }

    private bool ValidateDuplicate(List<GlobalEventElement> list)
    {
        return ValidateDuplicateBase<GlobalEventElement>(list, x => x.name);
    }
    
    #endregion

#if UNITY_EDITOR
    /// <summary>
    /// Пересборка файла с перечислением
    /// </summary>
    [ContextMenu("Manual Rebuild Enum Code")]
    public override void RebuildEnumCode()
    {
        RebuidEnumCodeBase<GlobalEventElement>("GlobalEventTypeEnum", events, x => x.name);
    }
#endif
}