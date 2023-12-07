using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject, GameObject> _interact;
    [SerializeField] private UnityEvent<GameObject, GameObject> _mouseOn;
    [SerializeField] private UnityEvent<GameObject, GameObject> _mouseOff;
    [SerializeField] private bool _manyInteract = false;
    [SerializeField] private bool _simpleInteractable = true;

    private bool hasInteract = false;

    public void Interact(GameObject hero)
    {
        if (_manyInteract || !hasInteract)
        {
            if (_simpleInteractable)
                InteractableInterface.instance.Hide();
            _interact?.Invoke(hero, gameObject);
            hasInteract = true;
        }
    }
    
    public void MouseOn(GameObject hero)
    {
        if (_simpleInteractable)
            InteractableInterface.instance.Show();
        _mouseOn?.Invoke(hero, gameObject);
    }
    
    public void MouseOff(GameObject hero)
    {
        if (_simpleInteractable)
            InteractableInterface.instance.Hide();
        _mouseOff?.Invoke(hero, gameObject);
    }
}