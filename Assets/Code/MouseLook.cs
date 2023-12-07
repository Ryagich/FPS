using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private InputAction pressed, axis;
    [SerializeField] private float _speed = 1;
    private bool  isRotating = false;
    private Transform camera;
    
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
        transform.Rotate(camera.right, -delta.y, Space.World);
        transform.Rotate(camera.up, delta.x, Space.World);
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