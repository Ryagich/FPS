using UnityEngine;
using UnityEngine.UI;
using YG;

public class DifficultyController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Button _easy;
    [SerializeField] private Button _normal;
    [SerializeField] private Button _hard;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
        {
            UpdateButtons();
        }
        else
        {
            YandexGame.GetDataEvent += UpdateButtons;
        }
    }

    public void ChangeButtonsState()
    {
        _canvas.SetActive(!_canvas.activeSelf);
    }
    
    public void UpdateButtons()
    {
        switch (YandexGame.savesData.DifficultIndex)
        {
            case 0:
                _easy.interactable = false;
                _normal.interactable = true;
                _hard.interactable = true;
                break;
            case 1:
                _easy.interactable = true;
                _normal.interactable = false;
                _hard.interactable = true;
                break;
            case 2:
                _easy.interactable = true;
                _normal.interactable = true;
                _hard.interactable = false;
                break;
        }
    }
}

enum Difficulty
{
    Easy,
    Normal,
    Hard,
}