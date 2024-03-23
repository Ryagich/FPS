using System;
using UnityEngine;

[Serializable]
public class TalentInfo 
{
    [field: SerializeField, TextArea] public string Name { get; private set; }
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Color BackgroundColor { get; private set; }
    [field: SerializeField] public bool Opened { get; private set; }

    public void Open()
    {
        Opened = true;
    }
    
    public void Close()
    {
        Opened = false;
    }
}
