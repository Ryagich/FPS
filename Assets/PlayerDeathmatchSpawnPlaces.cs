using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDeathmatchSpawnPlaces : MonoBehaviour, SpawnPlaces
{
    [field: SerializeField] protected List<Transform> Places { get; private set; }

    private void Awake()
    {
        SpawnPlaces.Instance = this;
    }

    public Transform GetSpawnPlace()
    {
        return Places[Random.Range(0, Places.Count)];
    }
}