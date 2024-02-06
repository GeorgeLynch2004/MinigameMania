using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private List<Player> m_Players;
    public static event System.Action<List<Player>> OnPlayerListChanged;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        m_Players = new List<Player>();
    }

    public void RegisterPlayer(Player player)
    {
        m_Players.Add(player);
        OnPlayerListChanged?.Invoke(m_Players);
    }

    public int GetLobbySize()
    {
        return m_Players.Count;
    }

    public List<Player> GetPlayersJoinedList()
    {
        return m_Players;
    }
}
