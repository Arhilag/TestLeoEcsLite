using System;
using UnityEngine;

[Serializable]
public struct LevelSettingComponent
{
    public LevelConfig Setting;
    public WeaponConfig[] Weapons;
}
