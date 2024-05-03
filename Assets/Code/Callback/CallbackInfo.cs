using System;
using UnityEngine;

[Serializable]
public class CallbackInfo
{
    [field: SerializeField, TextArea] public string Text { get; private set; }
    [field: SerializeField, TextArea] public CallbackTypes Type { get; private set; }

    public CallbackInfo(CallbackTypes type, string text)
    {
        Type = type;
        Text = text;
    }
}

public enum CallbackTypes
{
    Text,
    Health,
    Armor,
    Money,
    Crystal,
    MainSpell,
    AddSpell,
    Ammo,
    Grenade,
    Headshot,
    Damage,
}