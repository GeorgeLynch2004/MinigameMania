using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerDisplay : NetworkBehaviour
{
    [SerializeField] private List<GameObject> m_Players;
    [SerializeField] private List<Transform> m_Positions;
    [SerializeField] private GameManager m_GameManager;

    private void Start() 
    {   
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // we want to get the order of the players from the game session manager
        m_Players = m_GameManager.GetLastMinigamesPositions();
        // then we want to assign the players positions to the transform positions array
        for (int i = 0; i < m_Players.Count; i++)
        {
            m_Players[i].transform.position = m_Positions[i].position;
        }
        // then we want to start the ienumerator to count down and change the level after a few seconds
        StartCoroutine(ChangeLevel());
    }

    private IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(3);

        NetworkManager.SceneManager.LoadScene("Boardgame_Map", LoadSceneMode.Single);
    }
}
