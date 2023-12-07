using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoverForTime : MonoBehaviour
{
    public UnityEvent StartedMoved;
    public UnityEvent EndedMove;
    public bool IsMoving { get; private set; } = false;

    [SerializeField, Min(.0f)] private float _time = 1f;
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
        while (distance > _offset)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                target.position,
                distance / _time * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        EndedMove?.Invoke();
    }
}