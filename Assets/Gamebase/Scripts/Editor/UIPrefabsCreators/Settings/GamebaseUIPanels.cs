using System;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    [Serializable]
    public class GamebaseUIPanels
    {
        [Title("Панели (UIView)")]
        [Tooltip("Базовая панель")] public GameObject basePanel;
        [Tooltip("Диалог (с кнопками ДА/НЕТ)")] public GameObject dialog;
        [Tooltip("Информационная панель (с кнопкой ОК)")] public GameObject information;
        [Space]
        [Tooltip("Панель победы")] public GameObject victory;
        [Tooltip("Панель поражения")] public GameObject defeat;
    }
}