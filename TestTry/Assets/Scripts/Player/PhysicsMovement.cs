using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]

public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private float _minGroundNormalY;
    private float _gravityModifier;
    private bool _grounded;
    private Rigidbody2D _rigidBody;
    private Vector2 _velocity;
    private Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer;
    private List<RaycastHit2D> _hitBufferList;

    private const float MinMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;
    private const float JumpSpeed = 5.0f;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _hitBuffer = new RaycastHit2D[16];
        _hitBufferList = new List<RaycastHit2D>(16);

        _gravityModifier = 1f;
        _minGroundNormalY = .65f; 
        _layerMask = ~0;
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_layerMask);
        _contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        _targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (Input.GetKey(KeyCode.Space) && _grounded)
        {
            _velocity.y = JumpSpeed;
        }
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;

        _grounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Move(move, false);
        move = Vector2.up * deltaPosition.y;
        Move(move, true);
    }

    private void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinMoveDistance)
        {
            UpdateHitBufferList(move, distance);

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = GetCurrentNormal(_hitBufferList[i], yMovement);
                CalculateInclinedSurface(currentNormal);
                CalculateDistance(distance, _hitBufferList[i]);
            }
        }
        _rigidBody.position += move.normalized * distance;
    }

    private void UpdateHitBufferList(Vector2 move, float distance)
    {
        int hitCount = _rigidBody.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);

        _hitBufferList.Clear();
        
        for (int i = 0; i < hitCount; i++)
        {
            _hitBufferList.Add(_hitBuffer[i]);
        }
    }

    private Vector2 GetCurrentNormal(RaycastHit2D physicObject, bool yMovement)
    {
        Vector2 currentNormal = physicObject.normal;

        if (currentNormal.y > _minGroundNormalY)
        {
            _grounded = true;

            if (yMovement)
            {
                _groundNormal = currentNormal;
                currentNormal.x = 0;
            }
        }
        return currentNormal;
    }

    private float CalculateDistance(float distance, RaycastHit2D physicObject)
    {
        float modifiedDistance = physicObject.distance - ShellRadius;
        return modifiedDistance < distance ? modifiedDistance : distance;
    }

    private void CalculateInclinedSurface(Vector2 currentNormal)
    {
        float projection = Vector2.Dot(_velocity, currentNormal);

        if (projection < 0)
        {
            _velocity -= projection * currentNormal;
        }
    }
}