using System;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Gamebase
{
    [Serializable]
    public class GameFeatureSetting
    {
        public string Name;
        [TextArea] public string Description;
        public int ProgressLevelRequired;
    }
}