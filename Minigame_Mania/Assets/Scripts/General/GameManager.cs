using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private NetworkVariable<int> playerCount = new NetworkVariable<int>();
    [SerializeField] private NetworkVariable<bool> gameRunning = new NetworkVariable<bool>();
    [SerializeField] private NetworkVariable<List<GameObject>> playersArray = new NetworkVariable<List<GameObject>>();
    [SerializeField] private NetworkVariable<List<GameObject>> lastMinigamesPositions = new NetworkVariable<List<GameObject>>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        if (IsHost) gameRunning.Value = true;

        // Initialize playersArray with an empty list
        playersArray.Value = new List<GameObject>();

        // Subscribe to the client connected and disconnected events
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

        if (IsHost)
        {
            // Add all currently connected clients to the players array
            foreach (var clientId in NetworkManager.Singleton.ConnectedClients.Keys)
            {
                playersArray.Value.Add(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject);
            }
        }
        
    }


    private void OnClientDisconnectCallback(ulong clientId)
    {
        // Remove the disconnected player from the players array
        playersArray.Value.Remove(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject);
        // Decrement player count
        playerCount.Value--;
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log("Player Connected!");
        // Add the connected player to the players array
        playersArray.Value.Add(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject);
        // Increment player count
        playerCount.Value++;
    }

    public List<GameObject> GetPlayersArray()
    {
        return playersArray.Value;
    }

    public bool GetGameRunning()
    {
        return gameRunning.Value;
    }

    public void SetGameRunning(bool state)
    {
        gameRunning.Value = state;
    }

    public List<GameObject> GetLastMinigamesPositions()
    {
        return lastMinigamesPositions.Value;
    }

    public void SetLastMinigamesPositions(List<GameObject> players)
    {
        lastMinigamesPositions.Value = players;
    }
}