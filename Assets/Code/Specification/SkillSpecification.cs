using System;
using UnityEngine;

[Serializable]
public struct SkillSpecification
{
    public string name;
    [Multiline] public string description;
    public SkillType type;
    public int count;
        
    public enum SkillType
    {
        Main,
        Added,
    }
}


