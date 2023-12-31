using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {
    
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxVelocityX;
    [SerializeField] private float _maxVelocityY;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _isGrounded;

    private void Start() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        if (!_canMove) return;

        Vector3 _velocity = _rigidbody.velocity;

        // Horizontal input
        if (Input.GetKey(KeyCode.A)){_velocity += -Vector3.right * _movementSpeed * Time.deltaTime;}
        if (Input.GetKey(KeyCode.D)){_velocity += Vector3.right * _movementSpeed * Time.deltaTime;}

        // Jump input
        if (Input.GetKey(KeyCode.Space) && _isGrounded == true)
        {
            _velocity += Vector3.up * _jumpHeight * Time.deltaTime;
        }

        // Cap velocity
        _velocity.x = Mathf.Clamp(_velocity.x, -_maxVelocityX, _maxVelocityX);

        if (_velocity.y > 0)
        {
            _velocity.y = Mathf.Clamp(_velocity.y, -_maxVelocityY, _maxVelocityY);
        }
        else
        {
            _velocity.y *= 1.1f;
        }
        
        _rigidbody.velocity = _velocity;
    }

    public void setIsGrounded(bool state)
    {
        _isGrounded = state;
    }
}
