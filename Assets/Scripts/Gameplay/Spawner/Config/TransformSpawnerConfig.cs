using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Config/TransformSpawnerConfig")]
public class TransformSpawnerConfig : ScriptableObject
{
    [SerializeField] private float _up;
    public float Up => _up;
    [SerializeField] private float _right;
    public float Right => _right;
    [SerializeField] private float _left;
    public float Left => _left;
    [SerializeField] private float _down;
    public float Down => _down;
}
