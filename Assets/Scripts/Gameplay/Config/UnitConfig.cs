using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfig", menuName = "Config/UnitConfig")]
public class UnitConfig : ScriptableObject
{
    [SerializeField] private float _hp;
    public float HP => _hp;
    
    [SerializeField] private float _damage;
    public float Damage => _damage;
    
    [SerializeField] private float _speed;
    public float Speed => _speed;
    
    public int WeaponCount;

}
