using System;
using System.Collections;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using YG;

public class AdController : MonoBehaviour
{
    [SerializeField] private string before = "Ad after";
    [SerializeField] private string after = "sec";
    [SerializeField] private float time = 60;
    [SerializeField] private int countdown = 3;

    private TMP_Text text;

    public void Init(GameObject go)
    {
        text = go.GetComponent<UIHolder>().AdText;
        text.gameObject.SetActive(false);
        YandexGame.CloseFullAdEvent += () => Character.Instance.OnLockCursor();

        var adDay = new DateTime(2024, 4, 30, 0, 0, 0);
        if (DateTime.Today > adDay)
        {
            Stub();
        }
    }

    private void Stub()
    {
        StartCoroutine(ShowAd());
    }

    private IEnumerator ShowAd()
    {
        text.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        text.gameObject.SetActive(true);
        var c = countdown;
        while (c > 0)
        {
            text.text = $"{before} {c} {after}";
            c--;
            yield return new WaitForSeconds(1);
        }

        YandexGame.FullscreenShow();
        Stub();
    }
}