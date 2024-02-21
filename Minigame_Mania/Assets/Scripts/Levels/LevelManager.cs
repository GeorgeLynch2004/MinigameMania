using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class LevelManager : NetworkBehaviour
{
    [SerializeField] private int m_MaximumPlayers;
    [SerializeField] private List<Transform> m_SpawnPositions;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private NetworkVariable<int> m_NextFreePositionIndex;

    private void Start() 
    {
        m_NextFreePositionIndex.Value = 0;
        // Grab game manager
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // get the players array
        List<GameObject> playersArray = m_GameManager.GetPlayersArray();

        // set the positions of the players ready for level start
        for (int i = 0; i < playersArray.Count; i++)
        {
            playersArray[i].transform.position = m_SpawnPositions[i].position;
        }

        StartCoroutine(GameStartSequence());
    }

    public void Update()
    {
        // get an array of dead players
        List<GameObject> playersArray = m_GameManager.GetPlayersArray();
        List<GameObject> deadPlayers = new List<GameObject>();
        foreach (GameObject player in playersArray)
        {
            // if the player is dead and not already in the dead players array
            if (!player.GetComponent<HealthSystem>().isAlive() && !deadPlayers.Contains(player))
            {
                deadPlayers.Add(player);
            }
        }

        if (deadPlayers.Count == playersArray.Count)
        {
            deadPlayers.Reverse();
            m_GameManager.SetLastMinigamesPositions(deadPlayers);
        }
    }

    public int getMaximumPlayers()
    {
        return m_MaximumPlayers;
    }

    private IEnumerator GameStartSequence()
    {
        m_GameManager.SetGameRunning(false);

        yield return new WaitForSeconds(3);
        m_GameManager.SetGameRunning(true);
    }
    
}