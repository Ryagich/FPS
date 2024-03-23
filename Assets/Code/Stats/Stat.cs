using System;
using UnityEngine;

public class Stat
{
    public event Action ValueChanged;
    public float Max { get; private set; }
    public float Min { get; private set; }
    public float Value { get; private set; }

    public Stat(float max, float min, float value)
    {
        Max = max;
        Min = min;
        Value = value;
    }

    public void AddValue(float value)
    {
        Value = Mathf.Clamp(Value + value, Min, Max);
        ValueChanged?.Invoke();
    }

    public void ChangeValue(float newValue)
    {
        Value= Mathf.Clamp(newValue, Min, Max);
    }
    
    public void ChangeMax(float newMax)
    {
        Max = newMax;
    }
}