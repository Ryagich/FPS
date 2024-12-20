using System;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using YG;

public class GateAdShower : MonoBehaviour
{
    [SerializeField] private bool _showAd = true;
    private Interactable interactable;
    private CharacterDisabler disabler;
    
    private void Awake()
    {
        if (DateTime.Now <= new DateTime(2024,12,11,0,0,0,0))
            return;
        if (_showAd)
        {
            GetComponent<Interactable>()._interact.AddListener(OnInteract);
        }
    }

    private void OnInteract(GameObject player, GameObject gate)
    {
        if (!YandexGame.nowAdsShow && YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
        {
            // disabler = player.GetComponent<CharacterDisabler>();
            // YandexGame.OpenFullAdEvent += Open;
            // YandexGame.ErrorFullAdEvent += Error;
            YandexGame.CloseFullAdEvent += Complete;
            YandexGame.FullscreenShow();
        }
    }

    // private void Open()
    // {
    //     YandexGame.OpenFullAdEvent -= Open;
    //     YandexGame.ErrorFullAdEvent -= Error;
    //     YandexGame.CloseFullAdEvent -= Complete;
    //     
    //     disabler.Disable();
    //     Character.Instance.ToggleCursor();
    // }
    //
    // private void Error()
    // {
    //     YandexGame.OpenFullAdEvent -= Open;
    //     YandexGame.ErrorFullAdEvent -= Error;
    //     YandexGame.CloseFullAdEvent -= Complete;
    //     
    //     disabler.Activate();
    //     Character.Instance.ToggleCursor();
    //
    //     //Character.Instance.ToggleCursor();
    //     //Character.Instance.LockCursor(true);
    // }

    private void Complete()
    {
        // YandexGame.OpenFullAdEvent -= Open;
        // YandexGame.ErrorFullAdEvent -= Error;
        YandexGame.CloseFullAdEvent -= Complete;
        Character.Instance.ToggleCursor();

        // Character.Instance.LockCursor(true);
        // Character.Instance.ToggleCursor();
    }
}