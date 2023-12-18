using UnityEngine;

public class AudioSourceOutOfTime : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<AudioSource>().ignoreListenerPause = true;
    }
}
