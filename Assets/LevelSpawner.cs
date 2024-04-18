using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private Levels _levels;
    
    public void CreateLevel()
    {
        Instantiate(_levels.Stage_1[Random.Range(0, _levels.Stage_1.Count-1)]);
        GameObject.Find("NavMesh").GetComponent<NavMeshSurface>().BuildNavMesh();
        GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>().Spawn();
    }
}
