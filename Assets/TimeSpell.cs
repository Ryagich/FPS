using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpell : MonoBehaviour
{
    public static TimeSpell Instance;

    [SerializeField, Min(.0f)] private float _time = 7f;

    private float time = .0f;

    private void Awake()
    {
        Instance = this;
    }

    public void Activate()
    {
        StartCoroutine(HidingTime());
    }

    private IEnumerator HidingTime()
    {
        time = _time;
        while (Time.timeScale > .5f)
        {
            Time.timeScale -= Time.fixedDeltaTime;
            time -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        yield return new WaitForSeconds(time);
        while (Time.timeScale < 1f)
        {
            Time.timeScale += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}