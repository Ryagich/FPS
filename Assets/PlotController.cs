using UnityEngine;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using YG;

public class PlotController : MonoBehaviour
{
    [SerializeField] private TriggerZone _zone;
    [SerializeField] private string _reward = "Reward";
    
    private CompleteUIHolder holder;
    private PauseController pause;
    private int Reward = 0;

    public void Awake()
    {
        _zone._entered.AddListener(OnEnter);
    }

    private void OnEnter(GameObject go)
    {
        var character = go.GetComponent<Character>();
        holder = character.GetComponent<CanvasSpawner>().Complete.GetComponent<CompleteUIHolder>();
        var callback = character.GetComponent<KillEnemyCallback>();
        var disabler = GetComponent<CharacterDisabler>();
        pause = GetComponent<PauseController>();

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