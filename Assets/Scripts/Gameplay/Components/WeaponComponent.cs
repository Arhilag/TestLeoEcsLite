using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeaponComponent
{
    // public WeaponConfig[] Weapons;
    public WeaponLevelSettings[] LevelSettings;
}

[Serializable]
public struct WeaponLevelSettings
{
    public string Name;
    public int Level;
}
