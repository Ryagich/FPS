using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonExitToMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        var asyncOperation = SceneManager.LoadSceneAsync(0);
    }
}
