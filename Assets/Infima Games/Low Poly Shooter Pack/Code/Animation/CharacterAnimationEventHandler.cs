//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class CharacterAnimationEventHandler : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour playerCharacter;

        private void OnEjectCasing()
        {
            playerCharacter.EjectCasing();
        }

        private void OnAmmunitionFill(int amount = 0)
        {
            playerCharacter.FillAmmunition(amount);
        }

        private void OnSetActiveKnife(int active)
        {
            playerCharacter.SetActiveKnife(active);
        }

        private void OnGrenade()
        {
            playerCharacter.Grenade();
        }

        private void OnSetActiveMagazine(int active)
        {
            playerCharacter.SetActiveMagazine(active);
        }

        private void OnAnimationEndedBolt()
        {
            playerCharacter.AnimationEndedBolt();
        }

        private void OnAnimationEndedReload()
        {
            playerCharacter.AnimationEndedReload();
        }

        private void OnAnimationEndedGrenadeThrow()
        {
            playerCharacter.AnimationEndedGrenadeThrow();
        }

        private void OnAnimationEndedMelee()
        {
            playerCharacter.AnimationEndedMelee();
        }

        private void OnAnimationEndedInspect()
        {
            playerCharacter.AnimationEndedInspect();
        }

        private void OnAnimationEndedHolster()
        {
            playerCharacter.AnimationEndedHolster();
        }

        private void OnSlideBack(int back)
        {
            playerCharacter.SetSlideBack(back);
        }
    }
}