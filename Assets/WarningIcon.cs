using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningIcon : MonoCache
{
    [SerializeField, Min(.0f)] private float _power = 2f;
    [SerializeField, Min(.0f)] private float _speed = 0.1f;

    [SerializeField] private RectTransform _rectT;
    private float startT;

    private void Awake()
    {
        startT = _rectT.anchoredPosition.y;
    }

    protected override void Run()
    {
        var notT = Mathf.Sin(Time.time * _speed);
        var y = startT + Map.Translate(notT, -1, 1, -_power, _power);
        _rectT.anchoredPosition = _rectT.anchoredPosition.WithY2(y);
    }
}