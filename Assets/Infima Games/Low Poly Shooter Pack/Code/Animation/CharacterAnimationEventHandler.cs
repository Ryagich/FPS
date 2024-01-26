//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
	public class CharacterAnimationEventHandler : MonoBehaviour
	{
        [SerializeField]private CharacterBehaviour playerCharacter;

		/// <summary>
		/// Ejects a casing from the character's equipped weapon. This function is called from an Animation Event.
		/// </summary>
		private void OnEjectCasing()
		{
			//if(playerCharacter != null)
				playerCharacter.EjectCasing();
		}

		/// <summary>
		/// Fills the character's equipped weapon's ammunition by a certain amount, or fully if set to 0. This function is called
		/// from a Animation Event.
		/// </summary>
		private void OnAmmunitionFill(int amount = 0)
		{
			//if(playerCharacter != null)
				playerCharacter.FillAmmunition(amount);
		}
		/// <summary>
		/// Sets the character's knife active value. This function is called from an Animation Event.
		/// </summary>
		private void OnSetActiveKnife(int active)
		{
			//if(playerCharacter != null)
				playerCharacter.SetActiveKnife(active);
		}
		
		/// <summary>
		/// Spawns a grenade at the correct location. This function is called from an Animation Event.
		/// </summary>
		private void OnGrenade()
		{
			//if(playerCharacter != null)
				playerCharacter.Grenade();
		}
		/// <summary>
		/// Sets the equipped weapon's magazine to be active or inactive! This function is called from an Animation Event.
		/// </summary>
		private void OnSetActiveMagazine(int active)
		{
			//if(playerCharacter != null)
				playerCharacter.SetActiveMagazine(active);
		}

		/// <summary>
		/// Bolt Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedBolt()
		{
			//if(playerCharacter != null)
				playerCharacter.AnimationEndedBolt();
		}
		/// <summary>
		/// Reload Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedReload()
		{
			//if(playerCharacter != null)
				playerCharacter.AnimationEndedReload();
		}

		/// <summary>
		/// Grenade Throw Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedGrenadeThrow()
		{
			//if(playerCharacter != null)
				playerCharacter.AnimationEndedGrenadeThrow();
		}
		/// <summary>
		/// Melee Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedMelee()
		{
			//if(playerCharacter != null)
				playerCharacter.AnimationEndedMelee();
		}

		/// <summary>
		/// Inspect Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedInspect()
		{
			//if(playerCharacter != null)
				playerCharacter.AnimationEndedInspect();
		}
		/// <summary>
		/// Holster Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedHolster()
		{
			//if(playerCharacter != null)
				playerCharacter.AnimationEndedHolster();
		}

		/// <summary>
		/// Sets the character's equipped weapon's slide back pose. This function is called from an Animation Event.
		/// </summary>
		private void OnSlideBack(int back)
		{
			//if(playerCharacter != null)
				playerCharacter.SetSlideBack(back);
		}
	}   
}