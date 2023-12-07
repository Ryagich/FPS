using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;
    public GameObject Player { get; private set; }
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _playerPref;

    void Awake()
    {
        Instance = this;
    }

    public void Spawn()
    {
        Player = Instantiate(_playerPref, _parent);
    }
}