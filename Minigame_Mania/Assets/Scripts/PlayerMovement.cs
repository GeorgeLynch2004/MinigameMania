using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {
    
    [SerializeField] private Rigidbody2D _Rigidbody;
    [SerializeField] private float _MovementSpeed;


    private void FixedUpdate() 
    {
        if (!IsOwner) return;

        if (Input.GetKey(KeyCode.A)){_Rigidbody.velocity = Vector2.left * _MovementSpeed;}
        if (Input.GetKey(KeyCode.D)){_Rigidbody.velocity = Vector2.right * _MovementSpeed;}
    }
}
