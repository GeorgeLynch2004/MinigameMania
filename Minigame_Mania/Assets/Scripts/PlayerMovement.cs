using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {
    
    [SerializeField] private Rigidbody2D _Rigidbody;
    [SerializeField] private float _MovementSpeed;

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void FixedUpdate() 
    {
        Debug.Log(OwnerClientId + ": " + randomNumber.Value);
        if (!IsOwner) return;

        if (Input.GetKey(KeyCode.A)){_Rigidbody.velocity = Vector2.left * _MovementSpeed;}
        if (Input.GetKey(KeyCode.D)){_Rigidbody.velocity = Vector2.right * _MovementSpeed;}

        if (Input.GetKeyDown(KeyCode.T)){randomNumber.Value = Random.Range(0,100);}
    }
}
