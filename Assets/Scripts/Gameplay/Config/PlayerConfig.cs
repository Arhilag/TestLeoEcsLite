using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float _hp;
    public float HP => _hp;

    [SerializeField] private float _speed;
    public float Speed => _speed;
    public float Experience;
    
    public int WeaponCount;

}
