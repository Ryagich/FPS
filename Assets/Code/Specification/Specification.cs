using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Specification
{
    public string name;
    [Multiline] public string history;
    public int health;
    public int armour;
    public float speed;
    public float ammo;
    public List<SkillSpecification> skills;
}
