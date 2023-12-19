using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class LaptopsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _laptops;
    [SerializeField] private List<GameObject> _toDestroy;
    [SerializeField] private PlayerSpawner spawner;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
        {
            WaitPlayer();
        }
        else
        {
            YandexGame.GetDataEvent += WaitPlayer;
        }
    }

    private void WaitPlayer()
    {
        if (spawner.Player)
        {
            SetLaptops(spawner.Player);
        }
        else
        {
            spawner._playerSpawned.AddListener(SetLaptops);
        }
    }

    private void SetLaptops(GameObject player)
    {
        if (YandexGame.savesData.UsesLaptops == 0)
            return;
        for (int i = 0; i < YandexGame.savesData.UsesLaptops; i++)
        {
            if (i == YandexGame.savesData.UsesLaptops && i != _laptops.Count)
                _laptops[i].GetComponent<Interactable>().Interact(player);

            Destroy(_laptops[i]);
        }

        foreach (var td in _toDestroy)
            if (td)
                Destroy(td);
    }

    public void UpdateLaptopsSave(int index)
    {
        YandexGame.savesData.UsesLaptops = index;
    }
}