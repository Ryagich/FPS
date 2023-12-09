using UnityEngine;
using UnityEngine.Events;

public class AfterPhrase : MonoBehaviour
{
    [SerializeField] private UnityEvent _activated;
    [SerializeField] private bool _unsubAfterSubs = true;

    public void ActivateAfterSubs()
    {
        SubsController.Instance.EndWrite += Unsub;
    }

    private void Unsub()
    {
        if (_unsubAfterSubs)
            SubsController.Instance.EndWrite -= Unsub;
        _activated?.Invoke();
    }
}