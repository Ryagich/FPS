using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using YG;

public class LevelEnder : MonoBehaviour
{
    [SerializeField] private TriggerZone _zone;
    [SerializeField] private string _reward = "Reward";

    private CompleteUIHolder holder;
    private PauseController pause;
    private Character character;

    private int Reward = 0;

    private void Awake()
    {
        _zone._entered.AddListener(Init);
    }

    private void Init(GameObject go)
    {
        character = go.GetComponent<Character>();
    }

    public void End()
    {
        StartCoroutine(HideTimeSpeed());
    }

    public void OpenNextLevel(int index)
    {
        YandexGame.savesData.OpenedLevels[index] = true;
        YandexGame.SaveProgress();
        Debug.Log( YandexGame.savesData.OpenedLevels[index]);
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

        holder = character.GetComponent<CanvasSpawner>().Complete.GetComponent<CompleteUIHolder>();
        var callback = character.GetComponent<KillEnemyCallback>();
        var disabler = character.GetComponent<CharacterDisabler>();
        pause = character.GetComponent<PauseController>();

        Reward = callback.Money + Random.Range(200, 800);

        holder.RewardedText.text = $"{_reward}: {Reward}$";
        holder.RewardedButton.onClick.AddListener(() => YandexGame.RewVideoShow(1));
        holder.ExitToMenuButton.onClick.AddListener(() => YandexGame.savesData.Money += Reward);

        YandexGame.RewardVideoEvent += OnShowRewardedAd;

        pause.Pause();
        disabler.Disable();
        holder.Complete.gameObject.SetActive(true);
    }

    private void OnShowRewardedAd(int _)
    {
        Reward *= 3;
        holder.RewardedText.text = $"{_reward}: {Reward}$";
        holder.RewardedButton.interactable = false;
        pause.Pause();
    }
}