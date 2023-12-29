using UnityEngine;

public class DropItemSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    public void Play()
    {
        AudioManager.Instance.PlaySound(_clip,AudioSourceType.Item,transform);
    }
}