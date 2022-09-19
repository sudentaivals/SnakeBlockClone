using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _verticalAcceleration = 5f;
    [SerializeField] float _horizontalAcceleration = 5f;

    [SerializeField][Range(1, 25)] float _maxVerticalSpeed = 10f;
    [SerializeField] [Range(1, 25)] float _maxHorizontalSpeed = 10f;

    //movement
    private float _horizontalAxis;
    private bool _isInteractable = true;

    //cashed
    private Rigidbody _rb;

    private void OnEnable()
    {
        EventBus.Subscribe(EventBusEvent.GameOver, DisableMovement);
        EventBus.Subscribe(EventBusEvent.Victory, DisableMovement);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventBusEvent.GameOver, DisableMovement);
        EventBus.Unsubscribe(EventBusEvent.Victory, DisableMovement);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (_isInteractable)
        {
            ApplyHorizontalSpeed();
            ApplyVerticalSpeed();
        }
    }

    private void DisableMovement(UnityEngine.Object obj, EventArgs args)
    {
        DisableMovement();
    }

    private void ApplyVerticalSpeed()
    {
        _rb.AddForce(Vector3.forward * _verticalAcceleration, ForceMode.Acceleration);
        var clampedSpeed = Mathf.Clamp(_rb.velocity.z, 0, _maxVerticalSpeed);
        Vector3 clampedVelocity = new Vector3(_rb.velocity.x, _rb.velocity.y, clampedSpeed);
        _rb.velocity = clampedVelocity;
    }

    private void DisableMovement()
    {
        _rb.isKinematic = true;
        _isInteractable = false;
    }
    private void ApplyHorizontalSpeed()
    {
        if(_horizontalAxis == 0)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, _rb.velocity.z);
        }
        _rb.AddForce(Vector3.right * _horizontalAcceleration * _horizontalAxis, ForceMode.Acceleration);
        var clampedSpeed = Mathf.Clamp(_rb.velocity.x, -_maxHorizontalSpeed, _maxHorizontalSpeed);
        Vector3 clampedVelocity = new Vector3(clampedSpeed, _rb.velocity.y, _rb.velocity.z);
        _rb.velocity = clampedVelocity;

    }
}
