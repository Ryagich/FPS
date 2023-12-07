using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float m = .25f;
    [SerializeField] private int _sharpness = 3;
    [SerializeField, Range(.1f, 1f)] private float _hidePercent = .5f;
    [SerializeField] private float _defSize = 1.2f;
    [SerializeField] private float _beatSize = 1f;

    private StatFiller filler;
    private Stat stat;
    private Image bloodScreen;
    private BeatingHeart heart;
    private float t;

    public void Init(Image screen, Stat stat, StatFiller filler, BeatingHeart heart)
    {
        bloodScreen = screen;
        this.stat = stat;
        this.filler = filler;
        this.heart = heart;
    }

    private void FixedUpdate()
    {
        if (!bloodScreen)
            return;
        t += (heart.bpm * m / 60) * Time.fixedDeltaTime * Mathf.PI * 2;
        var normalized = (Mathf.Pow(Mathf.Sin(t), _sharpness) + 1) * 0.5f;
        var scale = _defSize + (_beatSize - _defSize) * normalized;
        bloodScreen.transform.localScale = Vector3.one * scale;
        bloodScreen.gameObject.SetActive(filler.Current / stat.Max  <= _hidePercent);
    }
}