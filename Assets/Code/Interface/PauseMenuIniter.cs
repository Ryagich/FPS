using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class PauseMenuIniter : MonoBehaviour
{
    [SerializeField] private Character _character;
    
    public void InitPauseMenu(GameObject pauseMenu)
    {
        pauseMenu.GetComponent<PauseMenu>().Init(_character);
    }
}
