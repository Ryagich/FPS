using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Transform _progressBarTransform;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private string _loading = "Loading";
    [SerializeField] private string _completeText = "Press any key to start";

    private bool isComplete = false;

    private void Awake()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        var asyncOperation =
            SceneManager.LoadSceneAsync(YandexGame.savesData.SceneIndex, LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            UpdateBar(asyncOperation.progress);
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        Complete();
        isComplete = true;
    }

    private void Update()
    {
        if (isComplete)
        {
            if (Input.anyKeyDown)
            {
                var unloader = SceneManager.UnloadSceneAsync(1);
                unloader.completed += operation =>
                {
                    GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>().Spawn();
                };
            }
        }
    }

    public void UpdateBar(float value)
    {
        var loadingProgress = Mathf.Clamp01(value);
        _progressText.text = $"{_loading} {(Math.Round(loadingProgress, 1) * 100)}%";
        _progressBar.fillAmount = loadingProgress;
    }

    public void Complete()
    {
        _progressText.text = _completeText;
        _progressBar.fillAmount = 1;
    }
}