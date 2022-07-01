using System;
using UnityEngine;

public struct WeaponContainerComponent
{
    public WeaponInstance[] Weapons;
}

public struct WeaponInstance
{
    public GameObject Projectile;
    public float DelayShoot;
    public float Damage;
    public float Speed;
    public float LifeTime;
    public float CountingTime;
}
