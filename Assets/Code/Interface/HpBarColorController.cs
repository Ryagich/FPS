using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarColorController : MonoBehaviour
{
    private Color emptyColor = Color.red;
    private Image hpBar;
    public void Init(StatFiller filler, Image image)
    {
        filler.CurrentChanged += ChangeColor;
        hpBar = image;   
    }

    public void ChangeColor(float value)
    {
        hpBar.color = Color.Lerp(emptyColor, Color.white, value);   
    }
    
}
