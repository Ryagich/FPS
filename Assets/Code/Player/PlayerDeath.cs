using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;
using YG;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerDeath : MonoBehaviour
{
    public UnityEvent Deaded;
    public UnityEvent Respawned;

    [SerializeField] private Character character;
    [SerializeField] private StatsController stats;
    [SerializeField] private CharacterDisabler disabler;
    [SerializeField] private Saver _saver;

    [SerializeField] private StatsController _statsC;
    private Transform deadScreen;

    private void Awake()
    {
        stats.Died += Disable;
    }

    public void Init(GameObject go)
    {
        var holder = go.GetComponent<DeathUIHolder>();
        deadScreen = holder.DeadScreen;

        holder.ContinueButton.onClick.AddListener(ShowAd);
        holder.ExitButton.onClick.AddListener(EndGame);
    }
    
    public void EndGame()
    {
        _saver.Reset();
        var asyncOperation = SceneManager.LoadSceneAsync(0);
    }
    
    public void ShowAd()
    {
        YandexGame.RewVideoShow(1);
        YandexGame.RewardVideoEvent += Respawn;
        YandexGame.ErrorVideoEvent += UnLuck;
    }

    private void UnLuck()
    {
        deadScreen.gameObject.SetActive(true);
    }
    
    private void Respawn(int _)
    {
        var place = transform;
        transform.position = place.position;
        transform.rotation = place.rotation;

        _statsC.Respawn();
        disabler.Activate();
        character.GetCameraDepth().enabled = true;
        deadScreen.gameObject.SetActive(false);
        stats.Hp.AddValue(stats.Hp.Max);
        stats.Armor.AddValue(stats.Armor.Max / 2);

        var inventory = character.GetInventory() as Inventory;
        inventory.FillAmmo();
        inventory.SetMaxAmmoInWeapons();

        EnemyController.Instance.SetCharacter(gameObject);
        //disabler.OnLockCursor(); //??
        Respawned?.Invoke();
    }

    public void Disable()
    {
        disabler.Disable();
        character.GetCameraDepth().enabled = false;
        deadScreen.gameObject.SetActive(true);

        EnemyController.Instance.RemoveCharacter();

        Deaded?.Invoke();
    }
}