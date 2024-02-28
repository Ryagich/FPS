using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Mover : MonoBehaviour
{
    public UnityEvent StartedMoved;
    public UnityEvent EndedMove;
    [field: SerializeField] public bool IsMoving { get; private set; } = false;

    [SerializeField] private bool _itsSpeed = true;
    [SerializeField, Min(.0f)] private float _speed = 1f;
    [SerializeField, Min(.0f)] private float _offset = .1f;

    private Coroutine coroutine;

    public void Move(Transform target)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Moving(target));
    }

    private IEnumerator Moving(Transform target)
    {
        StartedMoved?.Invoke();
        IsMoving = true;
        var distance = Vector3.Distance(transform.position, target.position);
        var maxDistanceDelta = (_itsSpeed ? _speed : distance / _speed) * Time.fixedDeltaTime;
        while (Vector3.Distance(transform.position, target.position) > _offset)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, maxDistanceDelta);

            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        EndedMove?.Invoke();
    }
}