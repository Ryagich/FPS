using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Talents : MonoBehaviour
{
    [field: SerializeField] public List<TalentInfo> TalentsInfo { get; private set; }
}
