using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private float _timeLimit;
    public float TimeLimit => _timeLimit;
    public float GlobalTime;

}
