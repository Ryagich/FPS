using UnityEngine;
using UnityEngine.Events;

public class ActivatorAfterMove : MonoBehaviour
{
    public UnityEvent EndedMove;
    [SerializeField] private Mover _mover;

    public void Activate()
    {
        _mover.EndedMove.AddListener(OnPlace);
    }

    private void OnPlace()
    {
        EndedMove?.Invoke();
        _mover.EndedMove.RemoveListener(OnPlace);
    }
}