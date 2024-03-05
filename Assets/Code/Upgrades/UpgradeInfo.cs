using System;
using UnityEngine;

[Serializable]
public class UpgradeInfo
{
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public bool Opened { get; private set; }

    public void Open()
    {
        Opened = true;
    }
}