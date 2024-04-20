using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour
{
    public UnityEvent<GameObject> _playerSpawned;
    public UnityEvent<GameObject> OnPlayerSpawnWithDelay;

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
        Player = Instantiate(_playerPref, _parent.position,_parent.rotation);
        _playerSpawned?.Invoke(Player);
        Invoke(nameof(SendDelayedMessage), .5f);
    }

    public void SendDelayedMessage()
    {
        OnPlayerSpawnWithDelay?.Invoke(Player);
    }
}