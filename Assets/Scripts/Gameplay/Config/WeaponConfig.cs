using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Config/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] private GameObject _projectile;
    public GameObject Projectile => _projectile;
    [SerializeField] private float _damage;
    public float Damage => _damage;
    [SerializeField] private float _speed;
    public float Speed => _speed;
    [SerializeField] private float _delay;
    public float Delay => _delay;
    [SerializeField] private float _lifeTime;
    public float LifeTime => _lifeTime;
    public float IndicationTime;
    public float IndicationDelay;
    public float Level;

}
