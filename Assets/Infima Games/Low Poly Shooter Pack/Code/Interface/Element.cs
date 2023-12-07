using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    public abstract class Element : MonoBehaviour
    {
        protected IGameModeService gameModeService;
        protected CharacterBehaviour characterBehaviour;
        protected InventoryBehaviour inventoryBehaviour;
        protected WeaponBehaviour equippedWeaponBehaviour;
        #region UNITY

        protected virtual void Awake()
        {
            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            characterBehaviour = gameModeService.GetPlayerCharacter();
            inventoryBehaviour = characterBehaviour.GetInventory();
        }
        
        private void Update()
        {
            if (Equals(inventoryBehaviour, null))
                return;

            equippedWeaponBehaviour = inventoryBehaviour.GetEquipped();
            
            Tick();
        }
        #endregion

        protected virtual void Tick() {}
    }
}