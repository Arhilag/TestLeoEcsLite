using System;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    [Serializable]
    public class GamebaseUICanvaces
    {
        [Title("Канвасы")]
        [Tooltip("Стандартный канвас (без доп.настроек)")] public GameObject baseCanvas;
        [Space]
        [Tooltip("Горизонтальный Shrink канвас")] public GameObject horizontalShrink;
        [Tooltip("Горизонтальный Expand канвас")] public GameObject horizontalExpand;
        [Tooltip("Вертикальный Shrink канвас")] public GameObject verticalShrink;
        [Tooltip("Вертикальный Expand канвас")] public GameObject verticalExpand;
    }
}