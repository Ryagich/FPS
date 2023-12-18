using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Helper StateMachineBehaviour that allows us to more easily play a specific weapon sound.
    /// </summary>
    public class PlaySoundCharacterBehaviour : StateMachineBehaviour
    {
        private enum SoundType
        {
            //Character Actions.
            GrenadeThrow,
            Melee,

            //Holsters.
            Holster,
            Unholster,

            //Normal Reloads.
            Reload,
            ReloadEmpty,

            //Cycled Reloads.
            ReloadOpen,
            ReloadInsert,
            ReloadClose,

            //Firing.
            Fire,
            FireEmpty,

            //Bolt.
            BoltAction
        }
        [SerializeField] private float delay;
        [SerializeField] private SoundType soundType;

        private CharacterBehaviour playerCharacter;
        private InventoryBehaviour playerInventory;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerCharacter ??= ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
            playerInventory ??= playerCharacter.GetInventory();

            //Try to get the equipped weapon's Weapon component.
            if (!(playerInventory.GetEquipped() is { } weaponBehaviour))
                return;

            var clip = soundType switch
            {
                SoundType.GrenadeThrow => playerCharacter.GetAudioClipsGrenadeThrow().GetRandom(),
                SoundType.Melee => playerCharacter.GetAudioClipsMelee().GetRandom(),
                SoundType.Holster => weaponBehaviour.GetAudioClipHolster(),
                SoundType.Unholster => weaponBehaviour.GetAudioClipUnholster(),
                SoundType.Reload => weaponBehaviour.GetAudioClipReload(),
                SoundType.ReloadEmpty => weaponBehaviour.GetAudioClipReloadEmpty(),
                SoundType.ReloadOpen => weaponBehaviour.GetAudioClipReloadOpen(),
                SoundType.ReloadInsert => weaponBehaviour.GetAudioClipReloadInsert(),
                SoundType.ReloadClose => weaponBehaviour.GetAudioClipReloadClose(),
                SoundType.Fire => weaponBehaviour.GetAudioClipFire(),
                SoundType.FireEmpty => weaponBehaviour.GetAudioClipFireEmpty(),
                SoundType.BoltAction => weaponBehaviour.GetAudioClipBoltAction(),
                _ => default
            };
            var sourceType = soundType switch
            {
                SoundType.GrenadeThrow => AudioSourceType.Weapon,
                SoundType.Melee => AudioSourceType.Weapon,
                SoundType.Holster => AudioSourceType.Player,
                SoundType.Unholster => AudioSourceType.Player,
                SoundType.Reload => AudioSourceType.Player,
                SoundType.ReloadEmpty => AudioSourceType.Player,
                SoundType.ReloadOpen => AudioSourceType.Player,
                SoundType.ReloadInsert => AudioSourceType.Player,
                SoundType.ReloadClose => AudioSourceType.Player,
                SoundType.Fire => AudioSourceType.Weapon,
                SoundType.FireEmpty => AudioSourceType.Weapon,
                SoundType.BoltAction => AudioSourceType.Weapon,
                _ => AudioSourceType.Player,
            };
            
            AudioManager.Instance.PlaySound(clip, sourceType, null, delay);
        }
    }
}