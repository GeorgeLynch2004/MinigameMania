using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HealthModifier : NetworkBehaviour
{
    [SerializeField] private float m_DamageDealt;
    [SerializeField] private bool m_DestroyOnCollision;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();
            
            if (healthSystem != null)
            {
                healthSystem.changeHealth(m_DamageDealt);
            }
            if (m_DestroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();
            
            if (healthSystem != null)
            {
                healthSystem.changeHealth(m_DamageDealt);
            }
            if (m_DestroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}
