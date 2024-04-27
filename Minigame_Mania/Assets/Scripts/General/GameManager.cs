using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.Netcode.Components;

public class GameManager : NetworkBehaviour
{
    // Define a struct to hold player data
    [Serializable]
    public struct PlayerData
    {
        public ulong clientId;
        public string playerName;
        public int health;
        public bool alive;
    }

    // Define custom events for player connection and disconnection
    public event Action<PlayerData> OnPlayerConnected;
    public event Action<ulong> OnPlayerDisconnected;

    // Track connected players using a dictionary
    public Dictionary<ulong, PlayerData> connectedPlayers = new Dictionary<ulong, PlayerData>();

    [SerializeField] private NetworkVariable<bool> gameRunning = new NetworkVariable<bool>();
    [SerializeField] private List<ulong> m_LastMinigamePositions = new List<ulong>();

    private void Start()
    {
        
        DontDestroyOnLoad(gameObject);

        // Subscribe to the client connected and disconnected events
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

        // Initialize gameRunning
        gameRunning.Value = true;
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        if (connectedPlayers.ContainsKey(clientId))
        {
            var disconnectedPlayer = connectedPlayers[clientId];
            connectedPlayers.Remove(clientId);
            OnPlayerDisconnected?.Invoke(clientId);
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (!connectedPlayers.ContainsKey(clientId))
        {
            var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            var playerName = playerObject.gameObject.name; // Example: Use appropriate way to get player name
            var playerData = new PlayerData { clientId = clientId, playerName = playerName, health = 0, alive = true};
            connectedPlayers.Add(clientId, playerData);
            OnPlayerConnected?.Invoke(playerData);
        }
    }

    public bool IsGameRunning()
    {
        return gameRunning.Value;
    }

    public void SetGameRunning(bool state)
    {
        gameRunning.Value = state;
    }

    public List<PlayerData> GetConnectedPlayers()
    {
        return new List<PlayerData>(connectedPlayers.Values);
    }

    // Method to set the position of all connected players
    public void SetAllPlayerPositions(List<Transform> newTransform)
    {
        int posIndex = 0;
        foreach (var clientId in connectedPlayers.Keys)
        {
            var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (playerObject != null)
            {
                GeneralPlayerUtilities utilityComponent = playerObject.GetComponent<GeneralPlayerUtilities>();
                utilityComponent.UpdatePositionClientRpc(newTransform[posIndex].position);
                posIndex++;
            }
        }
    }

    // method to get a list of players that are dead
    public List<ulong> GetDeadPlayers()
    {
        List<ulong> deadPlayerIDs = new List<ulong>();

        foreach (PlayerData playerData in connectedPlayers.Values)
        {
            if (playerData.alive == false && !deadPlayerIDs.Contains(playerData.clientId))
            {
                deadPlayerIDs.Add(playerData.clientId);
            }
        }
        return deadPlayerIDs;
    }

    public List<ulong> GetTotalPlayerIDs()
    {
        List<ulong> totalPlayerIDs = new List<ulong>();

        foreach (PlayerData playerData in connectedPlayers.Values)
        {
            if (!totalPlayerIDs.Contains(playerData.clientId))
            {
                totalPlayerIDs.Add(playerData.clientId);
            }
        }

        return totalPlayerIDs;
    }

    public void SetLastMinigamePositions(List<ulong> lastMinigamePositions)
    {
        m_LastMinigamePositions = lastMinigamePositions;
    }
}