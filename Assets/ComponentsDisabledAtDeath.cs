using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsDisabledAtDeath : MonoBehaviour
{
    [SerializeField] private List<GameObject> _toDisable;

    public void Disable()
    {
        foreach (var c in _toDisable)
        {
            c.SetActive(false);
        }
    }

    public void Active()
    {
        foreach (var c in _toDisable)
        {
            c.SetActive(true);
        }
    }
}