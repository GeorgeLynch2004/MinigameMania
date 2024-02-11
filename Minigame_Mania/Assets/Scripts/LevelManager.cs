using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LevelManager : NetworkBehaviour
{
    [SerializeField] private List<Transform> m_PlayerSpawnPositions;
    
    public List<Transform> GetSpawnPositions()
    {
        return m_PlayerSpawnPositions;
    }

    
}
