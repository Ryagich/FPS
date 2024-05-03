using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageCallbackInfo : MonoCache
{
    public event Action<DamageCallbackInfo> Ended;

    public float showTime = .25f;
    public Transform startPlace;
    public float speed;
    public Vector3 Target;

    private Coroutine coroutine;
    private Coroutine movingCoroutine;

    protected override void Run()
    {
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void StartMove()
    {
        coroutine = StartCoroutine(ShowCallBack());
    }

    private IEnumerator ShowCallBack()
    {
        var image = GetComponent<Image>();
        var icon = GetComponentInChildren<Image>();
        var text = GetComponentInChildren<TMP_Text>();

        var pos = transform.position;
        movingCoroutine = StartCoroutine(MoveCallBack(
            pos.WithX(pos.x + Target.x / 2).WithY(pos.y + Target.y / 2),
            image, icon, text, true));
        yield return new WaitWhile(() => movingCoroutine != null);

        yield return new WaitForSeconds(showTime);

        movingCoroutine = StartCoroutine(MoveCallBack(
            pos + Target, image, icon, text, false));
        yield return new WaitWhile(() => movingCoroutine != null);

        Ended?.Invoke(this);
    }

    private IEnumerator MoveCallBack(Vector3 target,
        Image image, Image icon, TMP_Text text, bool isShow)
    {
        while (Vector3.Distance(transform.position, target) > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 
                speed * (isShow? 4 : 3));
            var lostDistance = Vector3.Distance(transform.position, target);
            var distance = Vector3.Distance(startPlace.position, target);

            var a = isShow
                ? 1 - lostDistance / distance
                : lostDistance / distance;
            if (icon)
            {
                icon.color = icon.color.WithA(a);
            }

            image.color = image.color.WithA(a);
            text.color = text.color.WithA(a);

            yield return new WaitForFixedUpdate();
        }

        movingCoroutine = null;
    }
}