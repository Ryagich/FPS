using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private CharactersShower _shower;
    [SerializeField] private GameObject _chosen;
    [SerializeField] private GameObject _chose;
    [SerializeField] private Transform _parent;

    private GameObject currButton;
    private int index;

    private void Awake()
    {
        _shower.CharacterChanged += ShowButton;
    }

    private void ShowButton(int index)
    {
        this.index = index;

        if (currButton)
            Destroy(currButton);
        if (index == YandexGame.savesData.CharacterIndex)
            currButton = Instantiate(_chosen, _parent);
        else
        {
            currButton = Instantiate(_chose, _parent);
            currButton.GetComponent<Button>().onClick.AddListener(SelectCharacter);
        }
    }

    private void SelectCharacter()
    {
        YandexGame.savesData.CharacterIndex = index;
        YandexGame.SaveProgress();
        Destroy(currButton);
        currButton = Instantiate(_chosen, _parent);
    }
    
    public void Hide()
    {
        Destroy(currButton);
        currButton = null;
    }
}