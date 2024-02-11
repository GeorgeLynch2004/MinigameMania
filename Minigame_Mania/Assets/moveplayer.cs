using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class moveplayer : MonoBehaviour
{

    public float moveSpeed = 5f;
    private GameObject[] waypoints;
    private int currentWaypointIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    // Update is called once per frame
    void Update()
    {
        //increment current wypoint once movement has finished
       

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, moveSpeed * Time.deltaTime);
    }


    //function that can be called on button press/dice roll when implemented
    public void MoveToNextWaypoint()
    {
        currentWaypointIndex = Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
    }
}
