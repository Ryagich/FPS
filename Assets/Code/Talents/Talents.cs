using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Talents")]
public class Talents : ScriptableObject
{
    [field: SerializeField] public List<TalentInfo> TalentsInfo { get; private set; }
}
