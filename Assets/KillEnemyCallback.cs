using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KillEnemyCallback : MonoBehaviour
{
    public event Action Callback;
    
    public int Money { get; private set; } = 0;

    [SerializeField, Min(.0f)] private float _time = 1f;
    [SerializeField, Min(.0f)] private float _showTime = .25f;
    [SerializeField, Min(1)] private int _minCost = 15;
    [SerializeField, Min(1)] private int _maxCost = 40;

    [SerializeField] private GameObject _callBack;

    private UIHolder holder;

    public void Init(GameObject go)
    {
        holder = go.GetComponent<UIHolder>();
    }

    public void GetKillCallback()
    {
        var point = holder.StartKillCallbackPoint;
        var callback = Instantiate(_callBack, holder.transform);
        callback.transform.position = point.position;
        callback.GetComponentInChildren<TMP_Text>().text = GetCost() +"$";
        
        StartCoroutine(ShowCallBack(callback));
        Callback?.Invoke();
    }

    private int GetCost()
    {
        var cost = Random.Range(_minCost, _maxCost);
        Money += cost;
        return cost;
    }

    private IEnumerator ShowCallBack(GameObject callback)
    {
        var image = callback.GetComponent<Image>();
        var text = callback.GetComponentInChildren<TMP_Text>();
        image.color.WithA(0);
        text.color.WithA(0);

        var showPlace = holder.ShowKillCallbackPoint.position;
        var distance = Vector3.Distance(callback.transform.position, showPlace);
        while (Vector3.Distance(callback.transform.position, showPlace) > .1f)
        {
            callback.transform.position =
                Vector3.MoveTowards(callback.transform.position, showPlace,
                    distance / _time * Time.fixedDeltaTime);

            var lostDistance = Vector3.Distance(callback.transform.position, showPlace);

            var a = 1 - lostDistance / distance;
            image.color = image.color.WithA(a);
            text.color = text.color.WithA(a);

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(_showTime);

        var endPlace = holder.EndKillCallbackPoint.position;
        distance = Vector3.Distance(callback.transform.position, endPlace);
        while (Vector3.Distance(callback.transform.position, endPlace) > .1f)
        {
            callback.transform.position =
                Vector3.MoveTowards(callback.transform.position, endPlace,
                    distance / _time * Time.fixedDeltaTime);
            var lostDistance = Vector3.Distance(callback.transform.position, endPlace);

            var a = lostDistance / distance;
            image.color = image.color.WithA(a);
            text.color = text.color.WithA(a);

            yield return new WaitForFixedUpdate();
        }

        Destroy(callback);
    }
}