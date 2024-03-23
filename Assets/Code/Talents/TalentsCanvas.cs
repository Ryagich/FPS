using UnityEngine;
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
    [SerializeField] private SelectedTalent _selectedTalentPref;

    private List<SelectedTalent> selectedTalents = new();

    private void Awake()
    {
        Instance = this;
    }

    public void SetUp()
    {
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
        Name.text = info.Name;
        Description.text = info.Description;
    }

    public void Clear()
    {
        Name.text = "";
        Description.text = "";
        foreach (var st in selectedTalents)
        {
            Destroy(st.gameObject);
        }
        selectedTalents.Clear();
    }
}