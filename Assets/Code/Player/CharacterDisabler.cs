using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDisabler : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private BloodScreen bloodScreen;
    [SerializeField] private Movement movement;
    [SerializeField] private BeatingHeart heart;
    [SerializeField] private CameraLook _cameraLook;
    
    public void Activate()
    {
        character.CanPause = true;
        character.enabled = true;
        movement.enabled = true;
        _cameraLook.enabled = true;
        
        character.OnLockCursor();
        bloodScreen.StartBleeding();
        heart.StartBeating();
    }

    public void Disable()   
    {
        character.OnLockCursor();

        character.CanPause = false;
        character.enabled = false;
        movement.enabled = false;
        _cameraLook.enabled = false;
        character.holdingButtonFire = false;
        
        bloodScreen.StopBleeding();
        heart.StopBeating();
    }

    public void OnLockCursor()
    {
        //character.OnLockCursor();
        character.cursorLocked = !character.cursorLocked;
        Cursor.visible = !character.cursorLocked;
        Cursor.lockState = character.cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}