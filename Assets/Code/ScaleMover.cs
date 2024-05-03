using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleMover : MonoBehaviour
{
    public UnityEvent OnStartMove;
    public UnityEvent OnEndMove;
    [field: SerializeField] public bool IsMoving { get; private set; } = false;
    [SerializeField] private bool _itsSpeed = true;
    [SerializeField, Min(.0f)] private float _speed = 1f;
    [SerializeField, Min(.0f)] private float _offset = .1f;
    [SerializeField] private float _targetSize;
    [SerializeField] private Side _side = Side.Y;

    private Coroutine coroutine;
    private Vector3 defScale;

    private void Start()
    {
        defScale = transform.localScale;
    }

    public void Move()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Moving());
    }

    public void SetDefaultScale()
    {
        switch (_side)
        {
            case Side.X:
                transform.localScale = transform.localScale.WithX(defScale.x);
                break;
            case Side.Y:
                transform.localScale = transform.localScale.WithY(defScale.y);
                break;
            case Side.Z:
                transform.localScale = transform.localScale.WithZ(defScale.z);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private IEnumerator Moving()
    {
        OnStartMove?.Invoke();
        IsMoving = true;
        
        var maxDelta = (_itsSpeed ? _speed : GetDistance() / _speed) * Time.fixedDeltaTime;
        while (GetDistance() > _offset)
        {
            switch (_side)
            {
                case Side.X:
                    transform.localScale = transform.localScale.WithX(Mathf.MoveTowards(transform.localScale.x, _targetSize, maxDelta));
                    break;
                case Side.Y:
                    transform.localScale = transform.localScale.WithY(Mathf.MoveTowards(transform.localScale.y, _targetSize, maxDelta));
                    break;
                case Side.Z:
                    transform.localScale = transform.localScale.WithZ(Mathf.MoveTowards(transform.localScale.z, _targetSize, maxDelta));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        OnEndMove?.Invoke();
    }

    private float GetDistance()
    {
        return _side switch
        {
            Side.X => Mathf.Abs(transform.localScale.x - _targetSize),
            Side.Y => Mathf.Abs(transform.localScale.y - _targetSize),
            Side.Z => Mathf.Abs(transform.localScale.z - _targetSize),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum Side
{
    X,
    Y,
    Z
}