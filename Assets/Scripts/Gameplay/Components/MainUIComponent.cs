using System;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct MainUIComponent
{
    public TextMeshProUGUI Text_time;
    public TextMeshProUGUI Text_level;
    public TextMeshProUGUI Text_countKill;
    public Scrollbar Levelbar;
}
