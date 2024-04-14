using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEditor;
using UnityEngine.Networking;

using Unity.Netcode;




public class moveplayer : NetworkBehaviour
{

    public float moveSpeed = 5f;
    private GameObject[] waypoints;
    public NetworkVariable<int> currentWaypointIndex = new NetworkVariable<int>(0);
    public NetworkVariable<int> result;
    public bool moving;
    public int turncount = 0;
    public NetworkVariable<int> playerturn;
    public int thisplayer;
    public int player_count;
    private turncounter turnCounterScript;
    public void SetDiceResult(int diceResult)
    {
        result = new NetworkVariable<int>(diceResult);
    }
        // Start is called before the first frame update

   
        private void SortWaypointsByName()
    {
        bool swapped;
        do
        {
            swapped = false;
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                if (string.Compare(waypoints[i].name, waypoints[i + 1].name) > 0)
                {
                    GameObject temp = waypoints[i];
                    waypoints[i] = waypoints[i + 1];
                    waypoints[i + 1] = temp;
                    swapped = true;
                }
            }
        } while (swapped);
    }

    void Start()
    {
        thisplayer = int.Parse(gameObject.name);
        GameObject boardObject = GameObject.Find("Board");
        turnCounterScript = boardObject.GetComponent<turncounter>();

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        SortWaypointsByName();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypointIndex.Value == 14)
        {
            EditorUtility.DisplayDialog("Game over", "Game over (scene switch to lobby)", "ok");
        }
        moving = true;
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex.Value].transform.position, moveSpeed * Time.deltaTime);
        moving = false;
    }


    //function that can be called on button press/dice roll when implemented
    public void MoveToNextWaypoint()
    {
        if (currentWaypointIndex.Value == 14)
        {
            EditorUtility.DisplayDialog("Game over", "Game over (scene switch to lobby)", "ok");
        }
        currentWaypointIndex = new NetworkVariable<int>(Mathf.Min((currentWaypointIndex.Value) + 1, waypoints.Length - 1));
    }
    
    public void DiceMove()

    {
        if (currentWaypointIndex.Value == 14)
        {
            EditorUtility.DisplayDialog("Game over", "Game over (scene switch to lobby)", "ok");
        }
        playerturn = new NetworkVariable<int>(turnCounterScript.getTurn());

        if (playerturn.Value == thisplayer)
        {
            StartCoroutine(DelayedMovePlayer());
            

        }
        else
        {
            EditorUtility.DisplayDialog("Turns", "Its not your turn!", "ok");
        }
    }

    private IEnumerator DelayedMovePlayer()
    {
        if (currentWaypointIndex.Value == 14)
        {
            EditorUtility.DisplayDialog("Game over", "Game over (scene switch to lobby)", "ok");
        }
        yield return new WaitForSeconds(1.0f); // Wait for 1 second

        GameObject diceObject = GameObject.Find("dice1");
        rolldice diceScript = diceObject.GetComponent<rolldice>();
        int steps = diceScript.GetResult();
        StartCoroutine(MovePlayer(steps));
    }
    private IEnumerator MovePlayer(int steps)
    {
        if (currentWaypointIndex.Value == 14)
        {
            EditorUtility.DisplayDialog("Game over", "Game over (scene switch to lobby)", "ok");
        }
        for (int i = 0; i < steps; i++)
        {
            while (moving)
            {
                yield return null;
            }

            MoveToNextWaypoint();

            yield return new WaitForSeconds(0.7f);
        }
    }

}
