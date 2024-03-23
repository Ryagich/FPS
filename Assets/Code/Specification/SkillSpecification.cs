using System;
using UnityEngine;

[Serializable]
public struct SkillSpecification
{
    public string name;
    [Multiline] public string description;
    public SkillType type;
    public int value;
        
    public enum SkillType
    {
        Count,
        Time,
    }
}


