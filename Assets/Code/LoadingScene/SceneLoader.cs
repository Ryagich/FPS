using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLoadingScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}