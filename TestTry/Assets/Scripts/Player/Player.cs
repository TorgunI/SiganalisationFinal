using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    private Rigidbody2D _playerRigidBody;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();

        _playerRigidBody.freezeRotation = true;
    }
}
