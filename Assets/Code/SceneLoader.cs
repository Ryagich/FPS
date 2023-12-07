using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene()
    {
        var asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.completed += operation =>
        {
            
        };
    }
    
}