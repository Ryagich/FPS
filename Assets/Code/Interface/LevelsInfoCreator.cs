using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class LevelsInfoCreator : MonoBehaviour
{
    public event Action Inited;
    public List<Button> Buttons { get; private set; } = new();

    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _panelParent;
    [SerializeField] private LevelPanel _panel;
    [SerializeField] private Button _buttonPref;
    [SerializeField] private List<LevelInfo> _info = new();
    
    private LevelPanel panel = null;
    private bool inited;
    
    private void Awake()
    {
        if (YandexGame.SDKEnabled)
        {
            Init();
        }
        else
        {
            YandexGame.GetDataEvent += Init;
        }
    }

    private void Init()
    {
        if (inited)
            return;
        foreach (var i in _info)
        {
            var b = Instantiate(_buttonPref, _parent);
            b.GetComponentInChildren<TMP_Text>().text = i.Label;
            b.onClick.AddListener(() => CreateNewPanel(i.screenshot, i.Label, i.Description));
            b.onClick.AddListener(() =>
            {
                YandexGame.savesData.SceneIndex = i.SceneIndex;
                YandexGame.SaveProgress();
            });
            
            Buttons.Add(b);
        }
        inited = true;
        Inited?.Invoke();
    }
    
    public void DestroyPanel()
    {
        if (panel)
            Destroy(panel.gameObject);
        panel = null;
    }

    private void CreateNewPanel(Sprite sprite, string label, string description)
    {
        DestroyPanel();
        panel = Instantiate(_panel, _panelParent);
        panel.Init(sprite, label, description);
    }
}