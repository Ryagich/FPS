using System;
using UnityEngine;
using UnityEngine.UI;

public class BeatingHeart : MonoBehaviour
{
    public float bpm { get; private set; }
    [SerializeField, Min(0)] private int _min = 80;
    [SerializeField, Min(0)] private int _max = 190;
    [SerializeField] private int _sharpness = 3;
    [SerializeField] private float _defSize = .75f;
    [SerializeField] private float _beatSize = 1.1f;

    private Image heart;
    private float t;
    private StatFiller filler;
    private Stat stat;
    private bool isBeating = true;

    public void Init(Image heart, Stat stat, StatFiller filler)
    {
        this.heart = heart;
        this.stat = stat;
        this.filler = filler;
    }
    
    public void StartBeating()
    {
        isBeating = true;
    }
    
    public void StopBeating()
    {
        heart.transform.localScale = Vector3.one * _defSize;
        isBeating = false;
    }
    
    private void FixedUpdate()
    {
        if (!heart || !isBeating)
            return;
        var d = 1 - (filler.Current / stat.Max); // 0 - 1
        bpm = _min + (_max - _min) * d;
        t += (bpm / 60) * Time.fixedDeltaTime * Mathf.PI * 2;
        var normalized = (Mathf.Pow(Mathf.Sin(t), _sharpness) + 1) * 0.5f;
        var scale = _defSize + (_beatSize - _defSize) * normalized;
        heart.transform.localScale = Vector3.one * scale;
    }
}