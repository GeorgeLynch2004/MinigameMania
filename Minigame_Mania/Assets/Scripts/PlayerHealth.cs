using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;


public class PlayerHealth : NetworkBehaviour
{   
    [SerializeField] public int _MaxHealth;
    [SerializeField] private NetworkVariable<int> _CurrentHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        _CurrentHealth.OnValueChanged += (int previousValue, int newValue) => 
        {
            Debug.Log(OwnerClientId + "; Current Health: " + _CurrentHealth.Value);
            
        };
    }

    private void Start() 
    {
        _CurrentHealth.Value = _MaxHealth;
    }

    private void Update() 
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            _CurrentHealth.Value -= 1;
        }
    }
}
