using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> playerCount = new();
    [SerializeField] private NetworkVariable<bool> gameRunning = new();
    [SerializeField] private List<GameObject> playersArray = new List<GameObject>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (IsHost) gameRunning.Value = true;

        // Subscribe to the client connected and disconnected events
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

        // Add all currently connected clients to the players array
        foreach (var clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            playersArray.Add(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the events
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        playersArray.Remove(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject);
        playerCount.Value--;
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log("Player Connected!");
        playersArray.Add(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject);
        playerCount.Value++;
    }

    public List<GameObject> GetPlayersArray()
    {
        return playersArray;
    }

    public bool GetGameRunning()
    {
        return gameRunning.Value;
    }

    public void SetGameRunning(bool state)
    {
        gameRunning.Value = state;
    }
}