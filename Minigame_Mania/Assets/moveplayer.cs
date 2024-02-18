using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;



public class moveplayer : MonoBehaviour
{

    public float moveSpeed = 5f;
    private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    public int result;
    public void SetDiceResult(int diceResult)
    {
        result = diceResult;
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
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        SortWaypointsByName();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, moveSpeed * Time.deltaTime);
    }


    //function that can be called on button press/dice roll when implemented
    public void MoveToNextWaypoint()
    {
        currentWaypointIndex = Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
    }

    public void DiceMove()
    {

        GameObject diceObject = GameObject.Find("dice1");
        rolldice diceScript = diceObject.GetComponent<rolldice>();
        int steps = 0;
        int result  = diceScript.GetResult();
        for (int i = 0; i < result; i++)
        {
            MoveToNextWaypoint();
            Thread.Sleep(500);
        }
    }
}
