using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Config/ExperienceConfig")]
public class ExperienceConfig : ScriptableObject
{
    [SerializeField] private int[] _experienceToUp;
    public int[] ExperienceToUp => _experienceToUp;

}
