using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallbackController : MonoBehaviour
{
    public static CallbackController Instance;
    public int Money { get; private set; } = 0;

    [SerializeField, Min(.0f)] private float _time = 1f;
    [SerializeField, Min(.0f)] private float _showTime = .25f;
    [SerializeField] private float _distance = 70;

    [SerializeField] private GameObject _moneyCallBack;
    [SerializeField] private GameObject _textCallback;
    [SerializeField] private GameObject _healthCallback;
    [SerializeField] private GameObject _armorCallback;
    [SerializeField] private GameObject _crystalCallback;
    [SerializeField] private GameObject _mainSpellCallBack;
    [SerializeField] private GameObject _addSpellCallBack;
    [SerializeField] private GameObject _ammoCallBack;
    [SerializeField] private GameObject _grenadeCallBack;
    [SerializeField] private GameObject _headshotCallBack;
    [SerializeField] private GameObject _damageCallBack;

    [Space] [SerializeField] private float damageSpeed = 5f;
    [SerializeField] private float _damageShowTime = .25f;
    
    private List<CallbackInfo> infoList = new();
    private Dictionary<CallbackInfo, Ammo> AmmoTypes = new();
    private float modifier;
    private float speed;
    private UIHolder holder;
    private Transform parent;
    private Coroutine coroutine;
    private Coroutine movingCoroutine;
    private Transform startPlace;

    private DamageCallBackController damageCB = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GameObject go)
    {
        holder = go.GetComponent<UIHolder>();
        parent = holder.CallbackParent;
        startPlace = holder.StartKillCallbackPoint;
        damageCB.Init(holder.StartDamageCallbackPoint,damageSpeed,_damageShowTime);
    }

    public void ChangeState(bool state)
    {
        parent.gameObject.SetActive(state);
    }

    public void AddDamageCallBack(CallbackInfo info)
    {
        GameObject callback = null;
        switch (info.Type)
        {
            case CallbackTypes.Damage:
                callback = Instantiate(_damageCallBack, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
            case CallbackTypes.Headshot:
                callback = Instantiate(_headshotCallBack, parent);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        damageCB.AddCallBack(callback);
    }

    public void AddCallBack(CallbackInfo info)
    {
        infoList.Add(info);
        UpdateSpeed();
        if (coroutine == null)
        {
            coroutine = StartCoroutine(ShowCallBack());
        }
    }

    public void AddCallBack(CallbackInfo info, Ammo ammoType)
    {
        infoList.Add(info);
        AmmoTypes.Add(info, ammoType);
        UpdateSpeed();
        if (coroutine == null)
        {
            coroutine = StartCoroutine(ShowCallBack());
        }
    }

    private GameObject GetCallBack(Transform point)
    {
        var info = infoList[0];
        GameObject callback = null;
        switch (info.Type)
        {
            case CallbackTypes.Text:
                callback = Instantiate(_textCallback, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
            case CallbackTypes.Health:
                callback = Instantiate(_healthCallback, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
            case CallbackTypes.Armor:
                callback = Instantiate(_armorCallback, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
            case CallbackTypes.Money:
                callback = Instantiate(_moneyCallBack, parent);
                callback.GetComponentInChildren<TMP_Text>().text = "$" + info.Text;
                break;
            case CallbackTypes.Crystal:
                callback = Instantiate(_crystalCallback, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
            case CallbackTypes.MainSpell:
                callback = Instantiate(_mainSpellCallBack, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
            case CallbackTypes.AddSpell:
                callback = Instantiate(_addSpellCallBack, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
            case CallbackTypes.Ammo:
                callback = Instantiate(_ammoCallBack, parent);
                SetAmmoText(info, callback.GetComponentInChildren<TMP_Text>());
                break;
            case CallbackTypes.Grenade:
                callback = Instantiate(_grenadeCallBack, parent);
                callback.GetComponentInChildren<TMP_Text>().text = info.Text;
                break;
        }

        callback.transform.position = point.position;
        return callback;
    }

    private void SetAmmoText(CallbackInfo info, TMP_Text text)
    {
        switch (AmmoTypes[info])
        {
            case Ammo.Eleven:
                text.text = $"{info.Text} 11mm";
                break;
            case Ammo.Five:
                text.text = $"{info.Text} 5,56mm";
                break;
            case Ammo.Nine:
                text.text = $"{info.Text} 9mm";
                break;
            case Ammo.Seven:
                text.text = $"{info.Text} 7,62mm";
                break;
            case Ammo.Twelve:
                text.text = $"{info.Text} 12mm";
                break;
        }
    }

    private void UpdateSpeed()
    {
        var defSpeed = _distance / _time * Time.fixedDeltaTime;
        modifier = Mathf.Clamp(1f * infoList.Count, 1f, 3f);
        speed = defSpeed * modifier;
    }

    private IEnumerator ShowCallBack()
    {
        while (infoList.Count > 0)
        {
            var callback = GetCallBack(startPlace);
            var ct = callback.transform;
            var image = callback.GetComponent<Image>();
            var icon = callback.GetComponentInChildren<Image>();
            var text = callback.GetComponentInChildren<TMP_Text>();

            movingCoroutine = StartCoroutine(MoveCallBack(
                callback, ct.position.WithY(ct.position.y + 70), image, icon, text, true));
            yield return new WaitWhile(() => movingCoroutine != null);

            yield return new WaitForSeconds(_showTime / modifier);

            movingCoroutine = StartCoroutine(MoveCallBack(
                callback, ct.position.WithY(ct.position.y + 70), image, icon, text, false));
            yield return new WaitWhile(() => movingCoroutine != null);

            Destroy(callback);
            AmmoTypes.Remove(infoList[0]);
            infoList.Remove(infoList[0]);
            UpdateSpeed();
        }

        coroutine = null;
    }

    private IEnumerator MoveCallBack(GameObject callback, Vector3 target,
        Image image, Image icon, TMP_Text text, bool isShow)
    {
        while (Vector3.Distance(callback.transform.position, target) > .1f)
        {
            callback.transform.position =
                Vector3.MoveTowards(callback.transform.position, target, speed);
            var lostDistance = Vector3.Distance(callback.transform.position, target);

            var a = isShow
                ? 1 - lostDistance / _distance
                : lostDistance / _distance;
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