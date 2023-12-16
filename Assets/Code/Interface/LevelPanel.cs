using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _description;
    
    public void Init(Sprite sprite, string label, string description)
    {
        _image.sprite = sprite;
        _label.text = label;
        _description.text = description;
    }
}