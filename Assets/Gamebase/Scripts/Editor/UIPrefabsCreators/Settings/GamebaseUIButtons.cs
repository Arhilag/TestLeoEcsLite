using System;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    [Serializable]
    public class GamebaseUIButtons
    {
        [Title("Кнопки")]
        public GameObject circleIcon;
        public GameObject roundedIcon;
        public GameObject circleIconText;
        public GameObject roundedIconText;
        public GameObject circleText;
        public GameObject roundedText;
        [Space]
        public GameObject ok;
        public GameObject yes;
        public GameObject no;
        public GameObject close;
        public GameObject settings;
    }
}