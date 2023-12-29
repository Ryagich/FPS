using System;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Weapon2DView : MonoBehaviour
{
    [SerializeField] private Image _imageBody;
    [SerializeField] private Image _imageGrip;
    [SerializeField] private Image _imageLaser;
    [SerializeField] private Image _imageMuzzle;
    [SerializeField] private Image _imageMagazine;
    [SerializeField] private Image _imageScope;
    [SerializeField] private Image _imageScopeDefault;

    private WeaponAttachmentManagerBehaviour am;

    public void Show(GameObject weaponObject, Sprite body)
    {
        if (am)
            return;
        am = weaponObject.GetComponent<WeaponAttachmentManagerBehaviour>();
        gameObject.SetActive(true);
        _imageBody.sprite = body;

        var scopeDef = am.GetEquippedScopeDefault();
        if (scopeDef)
            AssignSprite(_imageScope,scopeDef.GetSprite(),scopeDef);
        
        var scope = am.GetEquippedScope();
        if (scope)
            AssignSprite(_imageScope,scope.GetSprite(),scope || scopeDef ==scope);

        var magazine = am.GetEquippedMagazine();
        if (magazine != null)
            AssignSprite(_imageMagazine, magazine.GetSprite(), magazine );

        var laser = am.GetEquippedLaser();
        if (laser != null)
            AssignSprite(_imageLaser, laser.GetSprite(), laser );

        var grip = am.GetEquippedGrip();
        if (grip != null)
            AssignSprite(_imageGrip, grip.GetSprite(), grip );

        var muzzle = am.GetEquippedMuzzle();
        if (muzzle != null)
            AssignSprite(_imageMuzzle, muzzle.GetSprite(), muzzle );
    }

    private static void AssignSprite(Image image,Sprite sprite,bool haveAtt)
    {
        image.sprite = sprite;
        image.enabled = sprite != null && haveAtt;
    }
    
    public void Hide()
    {
        am = null;
    }
}