using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rotater : MonoBehaviour
{
    public UnityEvent StartMoved;
    public UnityEvent EndMoved;
    public bool IsMoving { get; private set; } = false;

    [SerializeField] private bool _itsSpeed = true;
    [SerializeField, Min(.0f)] private float _speed = 1f;

    private Coroutine coroutine;

    public void Rotate(Transform target)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Moving(target));
    }

    private IEnumerator Moving(Transform target)
    {
        StartMoved?.Invoke();
        IsMoving = true;
        var distance = Quaternion.Angle(transform.rotation, target.rotation);
        while (transform.rotation != target.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                target.rotation, (_itsSpeed ? _speed : distance / _speed) * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        EndMoved?.Invoke();
    }
}