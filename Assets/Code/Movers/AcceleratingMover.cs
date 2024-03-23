using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AcceleratingMover : MonoBehaviour
{
    public UnityEvent StartedMoved;
    public UnityEvent EndedMove;
    [field: SerializeField] public bool IsMoving { get; private set; } = false;

    [SerializeField, Min(.0f)] private float _startSpeed = 1f;
    [SerializeField, Min(.0f)] private float _velocity = .1f;
    [SerializeField, Min(.0f)] private float _offset = .1f;

    private Coroutine coroutine;

    public void Move(Transform target)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Moving(target));
    }

    [SerializeField] private float speed;
    [SerializeField] private float maxDistanceDelta;
    [SerializeField] private float distance;

    private IEnumerator Moving(Transform target)
    {
        StartedMoved?.Invoke();
        IsMoving = true;
        speed = _startSpeed;

        while (Vector3.Distance(transform.position, target.position) > _offset)
        {
            distance = Vector3.Distance(transform.position, target.position);
            maxDistanceDelta = speed * Time.fixedDeltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.position, maxDistanceDelta);
            speed += _velocity;

            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        EndedMove?.Invoke();
    }
}