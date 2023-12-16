using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class CharacterDisabler : MonoBehaviour
{
    private Transform deadScreen;
    private Character character;
    private BloodScreen bloodScreen;
    private Movement movement;
    private BeatingHeart heart;

    private void Awake()
    {
        character = GetComponent<Character>();
        bloodScreen = GetComponent<BloodScreen>();
        movement = GetComponent<Movement>();
        heart = GetComponent<BeatingHeart>();
    }

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
        OnLockCursor();

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