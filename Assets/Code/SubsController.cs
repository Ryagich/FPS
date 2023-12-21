using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class SubsController : MonoBehaviour
{
    public event Action EndWrite;
    public event Action CantEndedWrite;
    public event Action CantEndedHide;

    public static SubsController Instance;

    [SerializeField, Min(.0f)] private float _time = .1f;
    [SerializeField, Min(.0f)] private float _showTime = 1f;
    [SerializeField, Min(.0f)] private float _outTime = 1f;
    [SerializeField] private AudioClip _knopkaSound;
    [SerializeField] private AudioSource source;

    private string currText;
    private string toWrite;
    private Coroutine coroutine;
    private bool isWriting;
    private TMP_Text text;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GameObject go)
    {
        text = go.GetComponent<UIHolder>().Subs;
    }

    public void WriteText(string str)
    {
        toWrite = str;
        currText = "";
        if (coroutine != null)
        {
            if (isWriting)
            {
                CantEndedWrite?.Invoke();
            }
            else
            {
                CantEndedHide?.Invoke();
            }

            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(Writing());
    }

    public void HideNow()
    {
        if (coroutine !=null)
            StopCoroutine(coroutine);
        text.text = "";
    }

    private IEnumerator Writing()
    {
        isWriting = true;
        foreach (var c in toWrite)
        {
            currText += c;
            text.text = currText;
            if (currText != toWrite)
            {
                text.text += '_';
            }

            if (c != ' ' || c != '.' || c != ',' || c != '-')
            {
                source.PlayOneShot(_knopkaSound);
                yield return new WaitForSeconds(_time);
            }
        }

        coroutine = StartCoroutine(Hide());

        EndWrite?.Invoke();
    }

    private IEnumerator Hide()
    {
        isWriting = false;
        yield return new WaitForSeconds(_showTime);
        foreach (var c in currText)
        {
            currText = currText.Substring(0, currText.Length - 1);
            text.text = currText;
            if (c != ' ' || c != '.' || c != ',')
            {
                yield return new WaitForSeconds(_outTime);
            }
        }
    }
}