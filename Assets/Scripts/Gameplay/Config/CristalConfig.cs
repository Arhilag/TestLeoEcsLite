using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CristalConfig", menuName = "Config/CristalConfig")]
public class CristalConfig : ScriptableObject
{
    [SerializeField] private float _experience;
    public float Experience => _experience;

}
