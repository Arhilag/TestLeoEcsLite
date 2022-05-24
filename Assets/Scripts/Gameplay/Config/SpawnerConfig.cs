using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Config/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{
    public GameObject _prefab;
    [SerializeField] private float _up;
    public float Up => _up;
    [SerializeField] private float _right;
    public float Right => _right;
    [SerializeField] private float _left;
    public float Left => _left;
    [SerializeField] private float _down;
    public float Down => _down;
    [SerializeField] private float _delay;
    public float Delay => _delay;
    [SerializeField] private float _totalTime;
    public float TotalTime => _totalTime;
}
