using UnityEngine;
using UnityEngine.Events;

public class DropArmor : MonoBehaviour
{
    [SerializeField] private UnityEvent _emptyValue;
    [SerializeField] private float _value = 100;

    public void Use(GameObject hero, GameObject _)
    {
        var sc = hero.GetComponent<StatsController>();
        if (_value <= sc.NeedArmor)
        {
            sc.TakeArmor(_value);
            _emptyValue?.Invoke();
        }
        else
        {
            _value -= sc.NeedArmor;
            sc.TakeArmor(sc.NeedArmor);
        }
    }
}