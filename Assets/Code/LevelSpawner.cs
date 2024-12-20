using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

public class LevelSpawner : MonoBehaviour
{
    private const int count = 5;
    [SerializeField] private Levels _levels;
    //Хочу хранить текущую стадию и индексы уровней которые должен пройти игрок
    //После прохождения каждого уровня вычеркивать его индекс и кидать новый уровень из списка индексов
    //Если все уровни пройдены - заходить на новую стадию, если стадии кончились - идти на второй круг.
    
    private Dictionary<int, GameObject> GetRandomLevels(List<GameObject> stage)
    {
        var result = GetLevels(stage);
        while (result.Count() > count)
        {
            if (result.Count == 0)
                throw new ArgumentException($"Уровней меньше ожидаемого");
            var index = Random.Range(0, result.Count);
            result.Remove(index);
        }
        // for (var i = 0; i < count; i++)
        // {
        //     if (result.Count == 0)
        //         throw new ArgumentException($"Уровней меньше ожидаемого");
        //     var index = Random.Range(0, result.Count);
        //     result.Remove(index);
        // }
        return result;
    }

    private Dictionary<int, GameObject> GetLevels(List<GameObject> stage)
    {
        var result = new Dictionary<int,GameObject>();
        for (var i = 0; i < stage.Count; i++)
            result.Add(i,stage[i]);    
        return result;
    }

    public void UpdateInfo(bool isZero)
    {
        if (isZero)
        {
            var randomLevels = GetRandomLevels(_levels.GetStage(1));
            var indices = randomLevels.Select(level => level.Key).ToList();
            var i = Random.Range(0, indices.Count - 1);
            
            YandexGame.savesData.Stage = 1;
            YandexGame.savesData.levelIndeces = randomLevels.Select(a => a.Key).ToArray();
            YandexGame.savesData.currentLevelIndex = indices[i];
        }
        else
        {
            if (YandexGame.savesData.Stage == 0)
                throw new ArgumentException("С нулевым индексом стадии - зашли в логику ненулевых стадий");

            //удалили текущую стадию
            var indeces = YandexGame.savesData.levelIndeces.ToList();
            indeces.Remove(YandexGame.savesData.currentLevelIndex);
            YandexGame.savesData.levelIndeces = indeces.ToArray();
            
            //если уровней в стадии не осталось - начали следующую
            if (YandexGame.savesData.levelIndeces.Length == 0)
            {
                YandexGame.savesData.Stage++;
                if (YandexGame.savesData.Stage > 2)
                    YandexGame.savesData.Stage = 1;
                
                var randomLevels = GetRandomLevels(_levels.GetStage(1));
                var levelIndeces = randomLevels.Select(level => level.Key).ToList();
                var i = Random.Range(0, levelIndeces.Count - 1);
        
                YandexGame.savesData.levelIndeces = randomLevels.Select(a => a.Key).ToArray();
                YandexGame.savesData.currentLevelIndex = levelIndeces[i];
            }
            //Если уровни остались, то берем случайный из оставшихся
            else
            {
                var i = Random.Range(0, YandexGame.savesData.levelIndeces.Length - 1);
                YandexGame.savesData.currentLevelIndex = YandexGame.savesData.levelIndeces[i];
            }
        }
    }
    
    public void CreateLevel()
    {
        switch (YandexGame.savesData.Stage)
        {
            case 0:
                Instantiate(_levels.Stage_0);
                break;
            case 1 or 2:
                var allLevels = _levels.GetStage(YandexGame.savesData.Stage);
                var a = Instantiate(allLevels[YandexGame.savesData.currentLevelIndex]);
                a.GetComponentInChildren<PlayerSpawner>();
                break;
        }
        GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>().Spawn();
    }
}