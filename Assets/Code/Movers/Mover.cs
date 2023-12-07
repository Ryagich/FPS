using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Mover : MonoBehaviour
{
    public UnityEvent StartedMoved;
    public UnityEvent EndedMove;
    public bool IsMoving { get; private set; } = false;

    [SerializeField] private bool _itsSpeed = true;
    [SerializeField, Min(.0f)] private float _speed = 1f;
    [SerializeField, Min(.0f)] private float _offset = .1f;

    public void Move(Transform target)
    {
        StartCoroutine(Moving(target));
    }

    private IEnumerator Moving(Transform target)
    {
        StartedMoved?.Invoke();
        IsMoving = true;
        var distance = Vector3.Distance(transform.position, target.position);
        while (Vector3.Distance(transform.position, target.position) > _offset)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                target.position, (_itsSpeed ? _speed : distance / _speed) * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        EndedMove?.Invoke();
    }
}