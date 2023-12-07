using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public PlayerControls _PlayerControls;
    [SerializeField] private Rigidbody2D _Rigidbody;
    [SerializeField] private float _MovementSpeed;
    private InputAction _Move;
    private Vector2 _MoveDirection = Vector2.zero;

    private void Awake() 
    {
        _PlayerControls = new PlayerControls();
    }

    private void OnEnable() 
    {
        _Move = _PlayerControls.Player.Move;
        _Move.Enable();
    }

    private void OnDisable() 
    {
        _Move.Disable();  
    }

    private void Update() 
    {
        _MoveDirection = _Move.ReadValue<Vector2>();
    }

    private void FixedUpdate() 
    {
        _Rigidbody.velocity = new Vector2(_MoveDirection.x * _MovementSpeed, _MoveDirection.y * _MovementSpeed);  
    }
}
