using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDeathmatchDeath : MonoBehaviour
{
    private Transform deadScreen;
    private Character character;
    private StatsController stats;
    private CharacterDisabler disabler;
    
    private void Awake()
    {
        if (!character)
            character = GetComponent<Character>();

        disabler = GetComponent<CharacterDisabler>();
        stats = GetComponent<StatsController>();
        
        StatsController.Died += Disable;
    }

    public void Init(GameObject go)
    {
        if (!character)
            character = GetComponent<Character>();
        var holder = go.GetComponent<DeathUIHolder>();
        deadScreen = holder.DeadScreen;

        holder.ContinueButton.onClick.AddListener(Respawn);
    }

    private void Respawn()
    {
        disabler.Activate();
       character.GetCameraDepth().enabled = true;
        deadScreen.gameObject.SetActive(false);
        stats.Hp.AddValue(stats.Hp.Max);
        
        var inventory = character.GetInventory() as Inventory;
        inventory.FillAmmo();
        inventory.SetMaxAmmoInWeapons();
        
        EnemyController.Instance.SetCharacter(gameObject);
        
        var places = PlayerSpawnPlaces.Instance.Places;
        var randomPlace = places[Random.Range(0, places.Count)];
        transform.position = randomPlace.position;
        transform.rotation = randomPlace.rotation;
    }

    public void Disable()
    {
        disabler.Disable();
        character.GetCameraDepth().enabled = false;
        deadScreen.gameObject.SetActive(true);
        
        EnemyController.Instance.RemoveCharacter();
    }
}