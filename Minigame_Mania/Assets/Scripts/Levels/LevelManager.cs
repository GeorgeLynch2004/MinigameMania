using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    public enum LevelType
    {
        Lobby,
        Sprint,
        Platformer,
    }

    [SerializeField] private LevelType levelType;
    [SerializeField] private int m_MaximumPlayers;
    [SerializeField] private List<Transform> m_SpawnPositions;
    [SerializeField] private GameManager m_GameManager;

    private void Start() 
    {
        // Grab game manager
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // set the positions of the players ready for level start
        m_GameManager.SetAllPlayerPositions(m_SpawnPositions);

        // adjust stuff according to what the level requires
        adjustLevelSettings(levelType);

        StartCoroutine(GameStartSequence());
    }

    private void adjustLevelSettings(LevelType levelType)
    {
        foreach (var clientId in m_GameManager.connectedPlayers.Keys)
            {
                var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                GeneralPlayerUtilities utilityComponent = playerObject.GetComponent<GeneralPlayerUtilities>();
                if (playerObject != null)
                {
                    if (levelType == LevelType.Sprint)
                    {
                        utilityComponent.UpdatePlayerGravityScaleClientRpc(false);
                        utilityComponent.newLevelPreperationClientRpc(LevelType.Sprint);
                    }
                    if (levelType == LevelType.Platformer)
                    {
                        utilityComponent.UpdatePlayerGravityScaleClientRpc(true);
                        utilityComponent.newLevelPreperationClientRpc(LevelType.Platformer);                        
                    }
                }
            }
    }

    public void Update()
    {
        // get an array of dead players
        List<ulong> deadPlayersIDs = new List<ulong>();
        deadPlayersIDs = m_GameManager.GetDeadPlayers();
        Debug.Log("DeadPlayerIDs: "+ deadPlayersIDs.ToString());
        // get an array of total players
        List<ulong> totalPlayersIDs = new List<ulong>();
        Debug.Log("TotalPlayerIDs:"+ totalPlayersIDs.ToString());
        totalPlayersIDs = m_GameManager.GetTotalPlayerIDs();

        // Check if a winner is determined
        if (deadPlayersIDs.Count == totalPlayersIDs.Count -1 && deadPlayersIDs.Count != 0)
        {
            Debug.Log("Winner is Player " + deadPlayersIDs[0]);
            deadPlayersIDs.Reverse();
            m_GameManager.SetLastMinigamePositions(deadPlayersIDs);
            Debug.Log("Starting Game End Sequence");
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
        NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
    
}