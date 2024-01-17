using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField] private string m_Username;

    //[SyncVar(hook=nameof(OnUsernameChanged))] WHERE I WAS AT
    private string m_SyncedUsername;

    public string Username => m_SyncedUsername;

    private void OnUsernameChanged(string oldValue, string newValue)
    {
        m_Username = newValue;
    }

    public override void OnNetworkSpawn()
    {
        GameManager.RegisterPlayer(this);
    }
}
