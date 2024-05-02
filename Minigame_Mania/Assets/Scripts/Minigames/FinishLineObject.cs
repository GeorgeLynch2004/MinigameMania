using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FinishLineObject : NetworkBehaviour
{
    public NetworkVariable<List<GameObject>> playersFinished;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersFinished.Value.Add(other.gameObject);
        }
    }
}
