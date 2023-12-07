using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SavesReseter : MonoBehaviour
{
    [SerializeField] private bool reset = false;
    private void Awake()
    {
    }

    private void Init()
    {
    }

    private void Reset()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }

    private void Update()
    {
        if (reset)
        {
            reset = false;
            Reset();
        }
    }
}
