using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SpawnPlaces
{
    public static SpawnPlaces Instance;
    public Transform GetSpawnPlace();
}
