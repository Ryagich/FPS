using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FireFlicker : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField, Range(.0f, 1f)] private float _tickPower = .1f;
    [SerializeField] private float _maxIntensity;
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _time = 1f;
    [SerializeField] private float _timeDifference = .5f;
    [SerializeField] private float _lerpTime=.02f;
    private float intensity;
    private float time;

    private void Awake()
    {
        intensity = _light.intensity;
    }
    
    private void FixedUpdate()
    {
        if (time <= 0f)
        {
            time = _time + Random.Range(_timeDifference, _timeDifference);
            intensity = Mathf.Clamp(intensity += Random.Range(-_tickPower, _tickPower), _minIntensity,
                _maxIntensity);
        }

        time -= Time.fixedDeltaTime;
        _light.intensity = Mathf.Lerp(_light.intensity, intensity,  _lerpTime);
    }
}