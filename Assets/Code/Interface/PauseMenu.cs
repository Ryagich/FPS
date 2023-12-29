using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _continueButton;

    public void Init(Character character)
    {
        _continueButton.onClick.AddListener(character.OnLockCursor);
    }
}
