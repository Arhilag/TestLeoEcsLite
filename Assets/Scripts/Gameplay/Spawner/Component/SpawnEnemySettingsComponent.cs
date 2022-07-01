using System;
using UnityEngine;

[Serializable]
public struct SpawnEnemySettingsComponent
{
    public SettingEnemySpawn[] _settings;
}

[Serializable]
public struct SettingEnemySpawn
{
    public GameObject EnemyPrefab;
    public float DelaySpawn;
    public float TimeLimit;
    public float Speed;
    public float HP;
    public float Damage;
    public GameObject ExperienceCrystal;
}