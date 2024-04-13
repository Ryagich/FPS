using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class TalentsCanvas : MonoBehaviour
{
    public static TalentsCanvas Instance;

    [field: SerializeField] public Transform Parent { get; private set; }
    [field: SerializeField] public TMP_Text Name { get; private set; }
    [field: SerializeField] public TMP_Text Description { get; private set; }

    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _card;
    [SerializeField] private Image _icon;
    [SerializeField] private SelectedTalent _selectedTalentPref;

    [SerializeField] private List<SelectedTalent> selectedTalents = new();
    private void Awake()
    {
        _card.gameObject.SetActive(false);
        Instance = this;
    }

    public void SetUp()
    {
        Clear();
        var selectedTalentsInfo = TalentsController.Instance.GetSelectedTalentsInfo();

        foreach (var info in selectedTalentsInfo)
        {
            var st = Instantiate(_selectedTalentPref, Parent);
            st.SetInfo(info);
            st.Button.onClick.AddListener(() => UpdateDescription(info));
            
            selectedTalents.Add(st);
        }
    }

    private void UpdateDescription(TalentInfo info)
    {
        _card.gameObject.SetActive(true);
        _icon.sprite = info.Sprite;
        Name.text = info.Name;
        Description.text = info.Description;
    }

    public void Clear()
    {
        _card.gameObject.SetActive(false);
        Name.text = "";
        Description.text = "";
        foreach (var st in selectedTalents)
        {
            Destroy(st.gameObject);
        }
        selectedTalents.Clear();
    }
}