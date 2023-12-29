using UnityEngine;

public class PauseController : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0;
        AudioManager.Instance.Pause();
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        AudioManager.Instance.UnPause();
    }
}
