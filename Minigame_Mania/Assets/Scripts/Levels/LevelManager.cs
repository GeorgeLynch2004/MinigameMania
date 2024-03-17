using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    [SerializeField] private int m_MaximumPlayers;
    [SerializeField] private List<Transform> m_SpawnPositions;
    [SerializeField] private GameManager m_GameManager;

    private void Start() 
    {
        // Grab game manager
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // set the positions of the players ready for level start
        m_GameManager.SetAllPlayerPositions(m_SpawnPositions);

        StartCoroutine(GameStartSequence());
    }

    public void Update()
    {
        // get an array of dead players
        List<ulong> deadPlayersIDs = new List<ulong>();
        deadPlayersIDs = m_GameManager.GetDeadPlayers();
        // get an array of total players
        List<ulong> totalPlayersIDs = new List<ulong>();
        totalPlayersIDs = m_GameManager.GetTotalPlayerIDs();

        // Check if a winner is determined
        if (deadPlayersIDs.Count == totalPlayersIDs.Count -1 && deadPlayersIDs.Count != 0)
        {
            deadPlayersIDs.Reverse();
            m_GameManager.SetLastMinigamePositions(deadPlayersIDs);
            StartCoroutine(GameEndSequence());
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

    private IEnumerator GameEndSequence()
    {
        m_GameManager.SetGameRunning(false);
        yield return new WaitForSeconds(3);
        NetworkManager.SceneManager.LoadScene("Boardgame_Map", LoadSceneMode.Single);
    }
    
}