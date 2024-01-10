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
    [SerializeField] private float _distance = 70;
    [SerializeField] private GameObject _callBack;

    private int count = 0;
    private float modifier;
    private float speed;

    private UIHolder holder;
    private Transform parent;
    private Coroutine coroutine;
    private Coroutine movingCoroutine;
    private Transform startPlace;
    
    public void Init(GameObject go)
    {
        holder = go.GetComponent<UIHolder>();
        parent = holder.CallbackParent;
        startPlace = holder.StartKillCallbackPoint;
    }

    public void ChangeState(bool state)
    {
        parent.gameObject.SetActive(state);
    }

    public void GetKillCallback()
    {
        count++;
        UpdateSpeed();
        if (coroutine == null)
            coroutine = StartCoroutine(ShowCallBack());
        Callback?.Invoke();
    }

    private int GetCost()
    {
        var cost = Random.Range(_minCost, _maxCost);
        Money += cost;
        return cost;
    }

    private void UpdateSpeed()
    {
        var defSpeed = _distance / _time * Time.fixedDeltaTime;
        modifier = Mathf.Clamp(1f * count, 1f, 3f);
        speed = defSpeed * modifier;
    }

    private GameObject GetCallBack(Transform point)
    {
        var callback = Instantiate(_callBack, parent);
        callback.transform.position = point.position;
        callback.GetComponentInChildren<TMP_Text>().text = GetCost() + "$";
        return callback;
    }

    private IEnumerator ShowCallBack()
    {
        while (count > 0)
        {
            var callback = GetCallBack(startPlace);
            var ct = callback.transform;
            var image = callback.GetComponent<Image>();
            var text = callback.GetComponentInChildren<TMP_Text>();

            movingCoroutine = StartCoroutine( MoveCallBack(
                    callback, ct.position.WithY(ct.position.y + 70), image, text, true));
            yield return new WaitWhile(() => movingCoroutine != null);

            yield return new WaitForSeconds(_showTime / modifier);
            
            movingCoroutine = StartCoroutine( MoveCallBack(
                callback, ct.position.WithY(ct.position.y + 70), image, text, false));
            yield return new WaitWhile(() => movingCoroutine != null);

            Destroy(callback);
            count--;
            UpdateSpeed();
        }

        coroutine = null;
    }

    private IEnumerator MoveCallBack(GameObject callback, Vector3 target,
        Image image, TMP_Text text, bool isShow)
    {
        while (Vector3.Distance(callback.transform.position, target) > .1f)
        {
            callback.transform.position =
                Vector3.MoveTowards(callback.transform.position, target, speed);
            var lostDistance = Vector3.Distance(callback.transform.position, target);

            var a = isShow
                ? 1 - lostDistance / _distance
                : lostDistance / _distance;
            image.color = image.color.WithA(a);
            text.color = text.color.WithA(a);

            yield return new WaitForFixedUpdate();
        }

        movingCoroutine = null;
    }
}