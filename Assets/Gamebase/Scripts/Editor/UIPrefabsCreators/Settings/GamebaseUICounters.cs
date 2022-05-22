using System;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    [Serializable]
    public class GamebaseUICounters
    {
        [Title("Текстовые Счетчики")]
        public GameObject simple;
        public GameObject graphicLeft;
        public GameObject graphicRight;
    }
}