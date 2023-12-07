using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct LevelInfo
{
#pragma warning disable 0649
    public string Label;
    public int SceneIndex;
    [Multiline] public string Description;
    public Sprite screenshot;
#pragma warning restore 0649
}