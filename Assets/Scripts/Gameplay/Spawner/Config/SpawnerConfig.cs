using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Config/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{
    public GameObject _prefab;
    [SerializeField] private float _delay;
    public float Delay => _delay;
    [SerializeField] private float _timeLimit;
    public float TimeLimit => _timeLimit;
}
