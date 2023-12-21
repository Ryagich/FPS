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