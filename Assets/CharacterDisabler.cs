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

    public void Activate()
    {
        character.CanPause = true;
        OnLockCursor();

        movement.enabled = true;
        bloodScreen.StartBleeding();
        heart.StartBeating();
    }

    public void Disable()
    {
        character.CanPause = false;
        character.holdingButtonFire = false;

        OnLockCursor();
        if (!movement)
            movement = GetComponent<Movement>();
        movement.enabled = false;
        bloodScreen.StopBleeding();
        heart.StopBeating();
    }

    private void OnLockCursor()
    {
        character.cursorLocked = !character.cursorLocked;
        Cursor.visible = !character.cursorLocked;
        Cursor.lockState = character.cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}