using System;
using UnityEngine;
using YG;
using System.Collections;
using InfimaGames.LowPolyShooterPack;
using Random = UnityEngine.Random;

public class DeathmatchTypeController : MonoBehaviour
{
    [SerializeField] private string _reward = "Reward";

    private KillEnemyCallback callback;
    private CompleteUIHolder holder;
    private CharacterDisabler disabler;
    private PauseController pause;
    private Character character;

    private int Reward = 0;
    private int kills = 0;
    private bool isEnded = false;

    // -1 - не дезматч; 0 - до 100 киллов; 1 - 5 мин; 2 - 10 мин;
    public void Init(GameObject go)
    {
        callback = GetComponent<KillEnemyCallback>();
        holder = go.GetComponent<CompleteUIHolder>();
        pause = GetComponent<PauseController>();
        disabler = GetComponent<CharacterDisabler>();
        character = GetComponent<Character>();

        var type = YandexGame.savesData.DeatmatchType;
        if (type == -1)
            throw new ArgumentException();
        if (type == 0)
            Set100Kills();
        if (type == 1)
            Set5Min();
        if (type == 2)
            Set10Min();
    }

    private IEnumerator HideTimeSpeed()
    {
        character.CanPause = false;
        var delta = Time.fixedDeltaTime;
        while (Time.timeScale >= 0.05f)
        {
            yield return new WaitForSeconds(delta);
            Time.timeScale = Mathf.Clamp(Time.timeScale - delta, 0, 1);
        }

        Reward = callback.Money + Random.Range(200, 800);

        var type = YandexGame.savesData.DeatmatchType;
        if (type == 0)
            Reward += (int)(Reward / 1.4f);
        if (type == 1)
            Reward += (int)(Reward / 1.2f);
        if (type == 2)
            Reward += (int)(Reward / 1.6f);

        holder.RewardedText.text = $"{_reward}: {Reward}$";
        holder.RewardedButton.onClick.AddListener(() => YandexGame.RewVideoShow(1));
        holder.ExitToMenuButton.onClick.AddListener(() => YandexGame.savesData.SaveMoney(Reward));

        YandexGame.RewardVideoEvent += OnShowRewardedAd;

        pause.Pause();
        disabler.Disable();
        holder.Complete.gameObject.SetActive(true);
    }

    private void EndLevel()
    {
        StartCoroutine(HideTimeSpeed());
    }

    private void OnShowRewardedAd(int _)
    {
        Reward *= 3;
        holder.RewardedText.text = $"{_reward}: {Reward}$";
        holder.RewardedButton.interactable = false;
        pause.Pause();
    }


    private void Set100Kills()
    {
        callback.Callback += () =>
        {
            kills += 1;
            TaskController.Instance.ShowTask(kills + "/" + 100);
            if (kills >= 100 && !isEnded)
            {
                isEnded = true;
                EndLevel();
            }
        };
    }

    private void Set5Min()
    {
        StartCoroutine(CountDown(5));
    }

    private void Set10Min()
    {
        StartCoroutine(CountDown(10));
    }

    private IEnumerator CountDown(float time)
    {
        var timeLeft = TimeSpan.FromMinutes(time);
        while (timeLeft.TotalSeconds > 0)
        {
            TaskController.Instance.ShowTask($"{timeLeft:mm\\:ss}");

            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));

            yield return new WaitForSeconds(1);
        }

        EndLevel();
    }
}