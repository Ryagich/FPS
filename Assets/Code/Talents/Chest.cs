using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    [SerializeField] private UnityEvent _onOpen;
    [SerializeField] private Interactable _interactable;

    [SerializeField] private AudioClip _doorsOpen;
    [SerializeField] private AudioSource _source;
    
    public void ChangeState(bool state)
    {
        _interactable.enabled = state;
    }
    
    [Button]
    public void Open()
    {
        _source.PlayOneShot(_doorsOpen);
        _onOpen?.Invoke();
    }
}