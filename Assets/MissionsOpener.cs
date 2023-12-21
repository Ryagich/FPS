using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MissionsOpener : MonoBehaviour
{
    [SerializeField] private Color _disableHighlightColor;

    private LevelsInfoCreator creator;
    private void Awake()
    {
        creator = GetComponent<LevelsInfoCreator>();
        if (creator.Buttons.Count > 0)
        {
            SetButtonsCurrentState();
        }
        else
        {
            creator.Inited += SetButtonsCurrentState;
        }
    }

    private void SetButtonsCurrentState()
    {
        for (int i = 0; i < creator.Buttons.Count; i++)
        {
            if (!YandexGame.savesData.OpenedLevels[i])
            {
                var colors = creator.Buttons[i].colors;
                colors.disabledColor = _disableHighlightColor;
                creator.Buttons[i].colors = colors;
                creator.Buttons[i].interactable = false;
            }
        }
    }
}
