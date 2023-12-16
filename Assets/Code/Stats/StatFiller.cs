using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatFiller
{
    public event Action<float> CurrentChanged;
    public float Current { get; private set; }
    private float start;
    private Coroutine coroutine;

    private readonly float speed, lerpSpeed;
    private readonly Image fill;
    private readonly Stat stat;
    private readonly MonoBehaviour mono;

    public StatFiller(Image image, Stat stat, MonoBehaviour mono, float speed, float lerpSpeed)
    {
        fill = image;
        Current = stat.Value;
        this.speed = speed;
        this.lerpSpeed = lerpSpeed;
        this.mono = mono;
        this.stat = stat;

        Fill();

        stat.ValueChanged += Fill;
    }

    private void Fill()
    {
        coroutine ??= mono.StartCoroutine(Filling());
    }

    private IEnumerator Filling()
    {
        while (Current != stat.Value)
        {
            Current = Mathf.Lerp(Current, stat.Value, lerpSpeed);
            Current = Mathf.MoveTowards(Current, stat.Value, speed * Time.fixedDeltaTime);
            fill.fillAmount = Current / stat.Max;
            CurrentChanged?.Invoke(Current / stat.Max);
            yield return new WaitForFixedUpdate();
        }

        coroutine = null;
    }
}