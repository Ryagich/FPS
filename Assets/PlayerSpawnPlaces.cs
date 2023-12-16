using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPlaces : MonoBehaviour
{
    public static PlayerSpawnPlaces Instance;
    [field: SerializeField] public List<Transform> Places { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
