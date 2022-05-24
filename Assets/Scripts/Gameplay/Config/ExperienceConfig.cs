using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Config/ExperienceConfig")]
public class ExperienceConfig : ScriptableObject
{
    [SerializeField] private float _experience;
    public float Experience => _experience;

}
