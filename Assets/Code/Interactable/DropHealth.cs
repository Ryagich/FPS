using UnityEngine;
using UnityEngine.Events;

public class DropHealth : MonoBehaviour
{
    [SerializeField] private UnityEvent _emptyValue;
    [SerializeField] private float _value = 20;

    public void Use(GameObject hero, GameObject _)
    {
        var sc = hero.GetComponent<StatsController>();

        sc.TakeHealth(_value);
        _value = 0;
        _emptyValue?.Invoke();
    }
}