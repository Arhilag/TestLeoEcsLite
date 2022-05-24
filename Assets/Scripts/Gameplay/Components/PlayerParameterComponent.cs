using System;
using Leopotam.EcsLite;
using UnityEngine;

[Serializable]
public struct PlayerParameterComponent
{
    public PlayerConfig Config;
    [HideInInspector] public float HP;
}
