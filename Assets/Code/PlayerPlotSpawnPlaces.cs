using System.Collections.Generic;
using UnityEngine;
using YG;

public class PlayerPlotSpawnPlaces : MonoBehaviour, SpawnPlaces
{
    [SerializeField] private int _lvlIndex;
    [SerializeField] private Transform _spawnPlace;
    
    private void Awake()
    {
        SpawnPlaces.Instance = this;
    }
    
    public void UpdateSpawnPlace(Transform newPlace)
    {
        _spawnPlace.position = newPlace.position;
        var p = _spawnPlace.position;

        YandexGame.savesData.levelsSpawnPlaces[_lvlIndex][0] = p.x;
        YandexGame.savesData.levelsSpawnPlaces[_lvlIndex][1] = p.y;
        YandexGame.savesData.levelsSpawnPlaces[_lvlIndex][2] = p.z;
    }

    public Transform GetSpawnPlace()
    {
        var place = YandexGame.savesData.levelsSpawnPlaces[_lvlIndex];
        if (place[0] != -1000 && place[1] != -1000 && place[2] != -1000)
        {
            _spawnPlace.position = new Vector3(place[0],place[1],place[2]);
        }

        return _spawnPlace;
    }

    public void ResetSaves()
    {
        YandexGame.savesData.levelsSpawnPlaces[_lvlIndex][0] = -1000;
        YandexGame.savesData.levelsSpawnPlaces[_lvlIndex][1] = -1000;
        YandexGame.savesData.levelsSpawnPlaces[_lvlIndex][2] = -1000;
    }
}