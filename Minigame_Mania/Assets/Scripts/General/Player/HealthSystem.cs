using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HealthSystem : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> m_CurrentHealth = new();
    [SerializeField] private NetworkVariable<bool> m_IsAlive = new();
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private float m_MaxHealth;


    // Start is called before the first frame update
    void Start()
    {
        m_CurrentHealth.Value = m_MaxHealth;   
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_IsAlive.Value = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (m_CurrentHealth.Value <= 0)
        {
            m_CurrentHealth.Value = 0;
            m_PlayerMovement.setCanMove(false);
            m_IsAlive.Value = false;
        }
        if (m_CurrentHealth.Value > m_MaxHealth)
        {
            m_CurrentHealth.Value = m_MaxHealth;
        }
    }


    public void changeHealth(float healthChange)
    {   
        m_CurrentHealth.Value += healthChange;
    }

    public float getHealth()
    {
        return m_CurrentHealth.Value;
    }

    public bool isAlive()
    {
        return m_IsAlive.Value;
    }
}
