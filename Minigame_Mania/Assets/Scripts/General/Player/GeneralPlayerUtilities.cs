using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using JetBrains.Annotations;

public struct playerData
{
    public int id;
    public bool isAlive;
}

public class GeneralPlayerUtilities : NetworkBehaviour
{
    [SerializeField] public NetworkVariable<ulong> id = new NetworkVariable<ulong>();


    [ClientRpc]
    public void SetIdClientRpc(ulong ID)
    {
        id.Value = ID;
    }

    [ClientRpc]
    public void UpdatePlayerColourClientRpc(Color colour)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = colour;
    }

    [ClientRpc]
    public void revivePlayerClientRpc()
    {
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        healthSystem.reviveToFullHealth();
    }

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

    [ClientRpc]
    public void newLevelPreperationClientRpc(LevelManager.LevelType levelType)
    {
        PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm != null)
            {
                if (levelType == LevelManager.LevelType.Sprint)
                {
                    pm.setControlMode(ControlMode.ButtonMash);
                }
                else if (levelType == LevelManager.LevelType.Platformer)
                {
                    pm.setControlMode(ControlMode.FreeMovement);
                }
            }
    }
}
