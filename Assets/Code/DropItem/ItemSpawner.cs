using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using YG;

public class ItemSpawner : MonoBehaviour
{
    [Space, SerializeField] private float _yPower;
    [SerializeField] private float _anotherPower;
    [SerializeField] private float _torque;
    [SerializeField] private BrokenItem _itemPref;
    [SerializeField] private int _count = 1;
    
    [Space, SerializeField, Range(.0f, 1f)]
    private float _crystalChange = .2f;
    [Space, SerializeField] private bool _isEnemy = false;
    
    public void DropItem()
    {
        for (int i = 0; i < _count; i++)
        {
            var type = GetItemType();
            if (type is BrokenItemType.Empty)
                return;
            var item = Instantiate(_itemPref, transform.position, transform.rotation);
            item.Activate(type);
            item.GetComponent<AcceleratingMover>().Move(Character.Instance.GetComponent<TargetHolder>().Target);

            var rb = item.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(
                    Random.Range(-_anotherPower, _anotherPower),
                    _yPower,
                    Random.Range(-_anotherPower, _anotherPower)),
                ForceMode.Impulse);

            rb.AddTorque(new Vector3(
                    Random.Range(-_torque, _torque),
                    Random.Range(-_torque, _torque),
                    Random.Range(-_torque, _torque)),
                ForceMode.Impulse);
        }
    }

    private BrokenItemType GetItemType()
    {
        if (Random.Range(.0f, 1f) <= .4)
        {
            return Random.Range(.0f, 1f) <= GetCurrencyChance()
                ? Random.Range(.0f, 1f) <= _crystalChange
                    ? BrokenItemType.Crystal
                    : BrokenItemType.Money
                : BrokenItemType.Empty;
        }

        var random = Random.Range(.0f, 1f);
        return random switch
        {
            <= .15f => BrokenItemType.AddSpell,
            <= .25f => YandexGame.savesData.Upgrades[3][1][1] == -1 ? BrokenItemType.Empty : BrokenItemType.Armor,
            <= .35f => YandexGame.savesData.Upgrades[3][1][0] == -1 ? BrokenItemType.Empty : BrokenItemType.Health,
            <= .5f => BrokenItemType.Grenade,
            _ => BrokenItemType.Ammo
        };
    }

    private float GetCurrencyChance()
    {
        if (_isEnemy)
        {
            return .8f;
        }
        
        return YandexGame.savesData.Upgrades[0][1][1] switch
        {
            0 => .2f,
            1 => .3f,
            2 => .45f,
            3 => .6f,
            4 => .8f,
            _ => .0f
        };
    }
}