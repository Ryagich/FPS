using UnityEngine;

public class InteractableFounder : MonoBehaviour
{
    [SerializeField] private Transform _cameraTrans;
    [SerializeField, Range(0.0f, 100.0f)] private float _distance = 1.5f;
    [SerializeField] private LayerMask _targetLayers;

    private Interactable lastIntractable;

    public void Interact()
    {
        if (lastIntractable)
        { 
            lastIntractable.Interact(gameObject);
        }
    }

    private void FixedUpdate()
    {
        var ray = new Ray(_cameraTrans.position, _cameraTrans.forward);

        if (Physics.Raycast(ray, out var hit, _distance, _targetLayers))
        {
            var interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != lastIntractable)
            {
                if (lastIntractable)
                    lastIntractable.MouseOff(gameObject);
                lastIntractable = interactable;
                lastIntractable.MouseOn(gameObject);
            }
        }
        else
        {
            if (lastIntractable)
                lastIntractable.MouseOff(gameObject);
            lastIntractable = null;
        }
    }
}