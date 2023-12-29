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
    [SerializeField] private PauseController _pause;

    public void Activate()
    {
        character.CanPause = true;
        movement.enabled = true;
        OnLockCursor();

        bloodScreen.StartBleeding();
        heart.StartBeating();
    }

    public void Disable()
    {
        character.CanPause = false;
        movement.enabled = false;
        character.holdingButtonFire = false;
        OnLockCursor();

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