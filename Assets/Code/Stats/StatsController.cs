using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public event Action Died; 
    public float NeedHealth => Hp.Max - Hp.Value;
    public float NeedArmor => Armor.Max - Armor.Value;
    public bool IsInit { get; private set; } = false;
    [SerializeField, Range(.0f, 1f)] private float _ratio = .75f;
    [SerializeField] private float _minHealth = 0;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _valueHealth = 100;
    [SerializeField] private float _minArmor = 0;
    [SerializeField] private float _maxArmor = 100;
    [SerializeField] private float _valueArmor = 100;

    public Stat Hp {get; private set; }
    public Stat Armor {get; private set; }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (IsInit)
            return;
        Hp = new Stat(_maxHealth, _minHealth, _valueHealth);
        Armor = new Stat(_maxArmor, _minArmor, _valueArmor);
        IsInit = true;
    }
    
    public void TakeHealth(float value)
    {
        Hp.AddValue(value);
    }
    
    public void TakeArmor(float value)
    {
        Armor.AddValue(value);
    }
    
    public void TakeDamage(float value)
    {
        var ad = value * _ratio;
        if (ad > Armor.Value)
        {
            var ld = ad - Armor.Value;
            var hd = value * (1 - _ratio) + ld;

            Armor.AddValue(-ad);
            Hp.AddValue(-hd);
        }
        else
        {
            Armor.AddValue(-ad);
            Hp.AddValue(-value * (1 - _ratio));
        }

        if (Hp.Value <= 0)
        {
            Died?.Invoke();
        }
    }
}