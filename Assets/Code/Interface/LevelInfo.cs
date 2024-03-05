using System;
using UnityEngine;

[Serializable]
public struct LevelInfo 
{
#pragma warning disable 0649
    public string Label;
    public int SceneIndex;
    [Multiline] public string Description;
    public Sprite screenshot;
    public bool IsVisible;
#pragma warning restore 0649
}