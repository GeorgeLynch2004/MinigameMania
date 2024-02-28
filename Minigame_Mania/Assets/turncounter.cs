using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class turncounter : MonoBehaviour
{

    public int turn = 1;
    public int player_count;

    public int getTurn()
    {
        return turn;
    }

    public void nextTurn()
    {
        turn++;
        if(turn > player_count)
        {
            turn = 1;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        player_count = players.Length;
        turn = 1; // Move this line here after setting player_count
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
