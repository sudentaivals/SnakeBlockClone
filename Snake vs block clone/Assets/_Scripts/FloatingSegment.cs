using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSegment : MonoBehaviour
{
    [SerializeField] float _gravityScale = 1f;
    [SerializeField] float _minForce;
    [SerializeField] float _maxForce;
    [Header("Floor collision")]
    [SerializeField] LayerMask _floorMask;
    [SerializeField] float _radius = 0.5f;
    [SerializeField] GameObject _splatVfx;

    private Vector3 _direction;
    private Vector3 _gravity;

    private Rigidbody _rb;
    private Collider[] _floorCollider = new Collider[1];
    private bool IsFalling => _rb.velocity.y < 0;
    private bool IsTouchingFloor => Physics.OverlapSphereNonAlloc(transform.position, _radius, _floorCollider, _floorMask) > 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gravity = Physics.gravity;
        _direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 0f)).normalized;
        _rb.AddForce(_direction * Random.Range(_minForce, _maxForce), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_gravityScale * _gravity, ForceMode.Acceleration);

    }

    void Update()
    {
        if (IsFalling && IsTouchingFloor)
        {
            var vfx = Instantiate(_splatVfx, transform.position, _splatVfx.transform.rotation);
            Destroy(vfx, 1f);
            Destroy(gameObject);
        }
    }
}
