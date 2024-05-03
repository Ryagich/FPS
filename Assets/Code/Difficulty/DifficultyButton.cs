using System;
using UnityEngine;
using YG;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private Difficulty _difficulty;

    public void ChangeDifficult()
    {
        YandexGame.savesData.DifficultIndex = _difficulty switch
        {
            Difficulty.Easy => 0,
            Difficulty.Normal => 1,
            Difficulty.Hard => 2,
            _ => throw new ArgumentOutOfRangeException()
        };
        YandexGame.SaveProgress();
    }
}