using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable RedundantTypeArgumentsOfMethod

public class ResourcesSystemSettings : GamebaseSystemSettings<ResourcesSystemSettings>
{ 
    [Title("Настройки системы ресурсов")]
    [InfoBox("Система ресурсов предоставляет интерфейс для работы с игровыми ресурсами целочисленного значения, дробными числами, строками и булевыми значениями.", InfoMessageType.None)]

    [Tooltip("Список ресурсов")]
    [InfoBox("Ресурсы ProgressLevel и Xp удалить нельзя, т.к. они используются в Progress System.")]
    [OnValueChanged("ValueChanged", true)]
    [ValidateInput("ValidateStrings", "Элементы могут содержать только латинские символы и цифры и начинаться обязательно с буквы!")]
    [ValidateInput("ValidateDuplicate", "Имеются повторяющиеся элементы!")]
    [SerializeField]
    private List<ResourceElement> elements = new List<ResourceElement>
    {
        new ResourceElement("ProgressLevel"),
        new ResourceElement("Xp")
    };
    
    protected override void Save()
    {
        // Принудительное прописывание необходимых в других системах элементов
        if (elements.All(x => x.name != "ProgressLevel"))
            elements.Add(new ResourceElement("ProgressLevel"));
        if (elements.All(x => x.name != "Xp"))
            elements.Add(new ResourceElement("Xp"));

        base.Save();
    }

    #region Validation

    protected override bool ValidateAll => valueChanged && ValidateStrings(elements) && ValidateDuplicate(elements);
    
    private bool ValidateStrings(List<ResourceElement> list)
    {
        return ValidateStringsBase<ResourceElement>(list, x => x.name);
    }

    private bool ValidateDuplicate(List<ResourceElement> list)
    {
        return ValidateDuplicateBase<ResourceElement>(list, x => x.name);
    }
    
    #endregion

#if UNITY_EDITOR
    /// <summary>
    /// Пересборка файла с перечислением
    /// </summary>
    [ContextMenu("Manual Rebuild Enum Code")]
    public override void RebuildEnumCode()
    {
        RebuidEnumCodeBase<ResourceElement>("ResourceTypeEnum", elements, x => x.name);
    }
#endif
}

[Serializable]
public class ResourceElement
{
    [Tooltip("Название ресурса (на английском, без пробелов и специальных символов)")]
    public string name = "";

    public ResourceElement() { }

    public ResourceElement(string name)
    {
        this.name = name;
    }
}