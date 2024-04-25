using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GeneralPlayerUtilities : NetworkBehaviour
{
    [ClientRpc]
    public void UpdatePositionClientRpc(Vector3 pos)
    {
        if (pos != null)
        {
            transform.position = pos;
        } 
    }

    [ClientRpc]
    public void UpdatePlayerGravityScaleClientRpc(bool state)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (state == true)
        {
            rb.gravityScale = 1;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

}
