using System;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    [Serializable]
    public class GlobalEventElement
    {
        [Tooltip("Название события (на английском, без пробелов и специальных символов)")]
        public string name = "";

        public GlobalEventElement() { }

        public GlobalEventElement(string name)
        {
            this.name = name;
        }
    }
}