using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyLook : MonoBehaviour
{
    [SerializeField] private List<GameObject> _all;
    [SerializeField] private LookList _look;

    private void Awake()
    {
        foreach (var go in _all)
        {
            go.SetActive(false);
        }

        var i = Random.Range(0,_look.list.Count);
        foreach (var go in _look.list[i].list)
        {
            go.SetActive(true);
        }
    }

    [Serializable]
    public class Look
    {
        public List<GameObject> list;
    }
 
    [Serializable]
    public class LookList
    {
        public List<Look> list;
    }
}
