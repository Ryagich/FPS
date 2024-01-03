using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
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

    public void Init()
    {
        StartCoroutine(Initing());
    }

    [Button]
    public void CheckLabel()
    {
        Debug.Log(_info[0].Label);
    }

    private IEnumerator Initing()
    {
        if (!inited)
        {
            yield return new WaitForSeconds(0f);
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