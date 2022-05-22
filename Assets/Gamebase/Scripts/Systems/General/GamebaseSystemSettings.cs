using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gamebase;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

public abstract class GamebaseSystemSettings<T> : StaticScriptableObject<T> where T : ScriptableObject
{
    #region Validation

    protected abstract bool ValidateAll { get; }
    
    protected bool valueChanged;
    protected void ValueChanged()
    {
        valueChanged = true;
    }

    protected bool ValidateStringsBase<U>(List<U> list, Func<U, string> getItem)
    {
        var result = true;
        var pattern = @"^[A-Za-z][A-Za-z0-9]*$";
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                var element = getItem(item);
                if (element.IsNullOrWhitespace() || !Regex.IsMatch(element, pattern))
                    result = false;
            }
        }

        return result;
    }

    protected bool ValidateDuplicateBase<U>(List<U> list, Func<U, string> func)
    {
        return list.GroupBy(func).All(g => g.Count() == 1);
    }

    #endregion

    #region Save

    [InfoBox("Не забудьте сохранить список после изменений, чтобы он был включен в динамический Enum.", InfoMessageType.Warning)]
    [EnableIf("ValidateAll")]
    [Button("Сохранить список")]
    protected virtual void Save()
    {
#if UNITY_EDITOR
        RebuildEnumCode();
        valueChanged = false;
#endif
    }

#if UNITY_EDITOR
    public abstract void RebuildEnumCode();

    protected void RebuidEnumCodeBase<U>(string className, List<U> list, Func<U, string> func)
    {
        DynamicEnums.ReplaceAndSave(className, list.Select(func).ToList());
    }
#endif

    #endregion
}