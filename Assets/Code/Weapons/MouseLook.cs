using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private InputAction pressed, axis;
    [SerializeField] private float _speed = 1;
    [SerializeField] private bool _x = true, _y = true;
    [SerializeField] private RotateRelativeTo _relative = RotateRelativeTo.Camera;
    private bool isRotating = false;
    private Transform camera;

    enum RotateRelativeTo
    {
        Camera,
        Transform
    }
    private void Awake()
    {
        pressed.Enable();
        axis.Enable();

        camera = Camera.main.transform;
        pressed.performed += SetTrueRotation;
        pressed.canceled += SetFalseRotation;
        axis.performed += Rotate;
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if (!isRotating)
            return;
        var delta = -context.ReadValue<Vector2>() * _speed;
        var rotateRelative = _relative == RotateRelativeTo.Camera ? camera : transform;
        if (_x)
            transform.Rotate(rotateRelative.right, -delta.y, Space.World);
        if (_y)
            transform.Rotate(rotateRelative.up, delta.x, Space.World);
    }

    private void SetTrueRotation(InputAction.CallbackContext _)
    {
        isRotating = true;
    }

    private void SetFalseRotation(InputAction.CallbackContext _)
    {
        isRotating = false;
    }

    private void OnDestroy()
    {
        pressed.performed -= SetTrueRotation;
        pressed.canceled -= SetFalseRotation;
        axis.performed -= Rotate;
    }

    private void OnDisable()
    {
        pressed.performed -= SetTrueRotation;
        pressed.canceled -= SetFalseRotation;
        axis.performed -= Rotate;
    }
}