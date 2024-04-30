using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LevelManager : NetworkBehaviour
{
    public enum LevelType
    {
        Lobby,
        Sprint,
        Platformer,
    }

    public enum FinishingCondition
    {
        Elimination,
        FinishLine,
    }

    [SerializeField] private LevelType levelType;
    [SerializeField] private FinishingCondition finishingCondition;
    [SerializeField] private int m_MaximumPlayers;
    [SerializeField] private List<Transform> m_SpawnPositions;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private FinishLineObject m_FinishLine;

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
        // check for finishing condition being reached
        ulong winner = isFinishingConditionReached();

        // if a winner is determined
        if (winner != 1000)
        {
            StartCoroutine(GameEndSequence());
            Debug.Log("The winner of the game is: Player " + winner);
        }
    }

    private ulong isFinishingConditionReached()
    {
        // if the finishing condition is process of elimination
        if (finishingCondition == FinishingCondition.Elimination)
        {
            // establish a dictionary to keep track of players and status, and total number of players in the game
            Dictionary<ulong, bool> playersAndAliveStatus = new Dictionary<ulong, bool>();
            int playersLeftAlive = m_GameManager.connectedPlayers.Keys.Count;

            // get the alive status of all players and update the playersLeftAlive accordingly
            foreach (var clientId in m_GameManager.connectedPlayers.Keys)
            {
                var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                HealthSystem healthSystemComponent = playerObject.GetComponent<HealthSystem>();
                bool aliveStatus = healthSystemComponent.isAlive();
                playersAndAliveStatus.Add(clientId, aliveStatus);
                if (!aliveStatus){playersLeftAlive--;}
            }

            // check if there is one player remaining
            if (playersLeftAlive == 1)
            {
                // search through the dictionary to find which player is last alive
                foreach (ulong clientId in playersAndAliveStatus.Keys)
                {
                    // find the last alive player and return their ID
                    if (playersAndAliveStatus[clientId] == true)
                    {
                        return clientId;
                    }
                }
            } 
        }
        else if (finishingCondition == FinishingCondition.FinishLine)
        {
            List<GameObject> finishedPlayers = m_FinishLine.playersFinished.Value;
            if (finishedPlayers.Count > 0)
            {
                ulong firstPlaceID = finishedPlayers[0].gameObject.GetComponent<GeneralPlayerUtilities>().id.Value;
                return firstPlaceID;
            }
        }


        // if no player has won
        return 1000;
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