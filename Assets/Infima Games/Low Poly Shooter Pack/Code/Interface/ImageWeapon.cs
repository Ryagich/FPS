using UnityEngine;
using UnityEngine.UI;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    public class ImageWeapon : Element
    {
        [Tooltip("Color applied to all images.")] 
        [SerializeField] private Color imageColor = Color.white;
        [SerializeField] private Image imageWeaponBody;
        [SerializeField] private Image imageWeaponGrip;
        [SerializeField] private Image imageWeaponLaser;
        [SerializeField] private Image imageWeaponMuzzle;
        [SerializeField] private Image imageWeaponMagazine;
        [SerializeField] private Image imageWeaponScope;
        [SerializeField] private Image imageWeaponScopeDefault;

        private WeaponAttachmentManagerBehaviour attachmentManagerBehaviour;

        protected override void Tick()
        {
            var toAssign = imageColor;
            foreach (var image in GetComponents<Image>())
                image.color = toAssign;

            attachmentManagerBehaviour = equippedWeaponBehaviour.GetAttachmentManager();
            imageWeaponBody.sprite = equippedWeaponBehaviour.GetSpriteBody();

            Sprite sprite = default;

            var scopeDefaultBehaviour = attachmentManagerBehaviour.GetEquippedScopeDefault();
            if (scopeDefaultBehaviour != null)
                sprite = scopeDefaultBehaviour.GetSprite();
            AssignSprite(imageWeaponScopeDefault, sprite, scopeDefaultBehaviour == null);
            var scopeBehaviour = attachmentManagerBehaviour.GetEquippedScope();
            if (scopeBehaviour != null)
                sprite = scopeBehaviour.GetSprite();
            AssignSprite(imageWeaponScope, sprite, scopeBehaviour == null || scopeBehaviour == scopeDefaultBehaviour);

            var magazineBehaviour = attachmentManagerBehaviour.GetEquippedMagazine();
            if (magazineBehaviour != null)
                sprite = magazineBehaviour.GetSprite();
            AssignSprite(imageWeaponMagazine, sprite, magazineBehaviour == null);

            var laserBehaviour = attachmentManagerBehaviour.GetEquippedLaser();
            if (laserBehaviour != null)
                sprite = laserBehaviour.GetSprite();
            AssignSprite(imageWeaponLaser, sprite, laserBehaviour == null);

            var gripBehaviour = attachmentManagerBehaviour.GetEquippedGrip();
            if (gripBehaviour != null)
                sprite = gripBehaviour.GetSprite();
            AssignSprite(imageWeaponGrip, sprite, gripBehaviour == null);

            var muzzleBehaviour = attachmentManagerBehaviour.GetEquippedMuzzle();
            if (muzzleBehaviour != null)
                sprite = muzzleBehaviour.GetSprite();
            AssignSprite(imageWeaponMuzzle, sprite, muzzleBehaviour == null);
        }

        private static void AssignSprite(Image image, Sprite sprite, bool forceHide = false)
        {
            image.sprite = sprite;
            image.enabled = sprite != null && !forceHide;
        }
    }
}