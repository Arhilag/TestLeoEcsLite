using System;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    [Serializable]
    public class GamebaseUISafeAreas
    {
        [Title("Safe Area")]
        [Tooltip("Для портретной ориентации")] public GameObject portrait;
        [Tooltip("Для ландшафтной ориентации")] public GameObject landspace;
    }
}