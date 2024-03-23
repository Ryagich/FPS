using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageble : MonoBehaviour
{
    [SerializeField] private UnityEvent OnHit;
    [SerializeField] private UnityEvent OnDeath;

    [SerializeField, Min(.0f)] private float _hp;

    public void SetDamage(float value)
    {
        _hp -= value;
        if (_hp <= 0)
        {
            OnDeath?.Invoke();
        }
        else
        {
            OnHit?.Invoke();
        }
    }
}
