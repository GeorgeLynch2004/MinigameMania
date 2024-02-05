using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private static List<Player> m_Players = new List<Player>();
    public static event System.Action<List<Player>> OnPlayerListChanged;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public static void RegisterPlayer(Player player)
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
