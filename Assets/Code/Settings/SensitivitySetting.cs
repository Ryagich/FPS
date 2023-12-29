using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SensitivitySetting : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private bool _isX;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        UpdateSensitivity();
    }

    public void UpdateSensitivity()
    {
        if (_isX)
            YandexGame.savesData.SensitivityX = slider.value / 100;
        else
            YandexGame.savesData.SensitivityY = slider.value / 100;
        _text.text = slider.value.ToString();
        YandexGame.SaveProgress();
    }
}