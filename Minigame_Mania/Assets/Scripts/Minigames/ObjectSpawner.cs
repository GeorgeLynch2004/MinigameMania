using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Used for Minigames like Meteor and Floor is Lava
public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float xRandomDistance;
    [SerializeField] private float yRandomDistance;
    [SerializeField] public float timeBetweenSpawns;
    [SerializeField] private float currentTimer;
    [SerializeField] private float intervalIncreaseTimer;
    [SerializeField] private float intervalIncreaseAmount;
    private float currentIntervalIncreaseTimer;
    private GameManager m_GameManager;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = 0f;
        timeBetweenSpawns = 0.8f;
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost) return;
        if (!m_GameManager.IsGameRunning()) return;

        if (currentTimer >= timeBetweenSpawns) 
        {
            // Every time there is a spawn random values are generated to determine where the meteor spawns on the x axis. This value is rounded to an integer for the sake of trying to prevent unavoidable deaths.
            float xRandom = Random.Range(-xRandomDistance, xRandomDistance);
            float yRandom = Random.Range(-yRandomDistance, yRandomDistance);
            float healthRandom = Random.value;
            float coinRandom = Random.value;

            Vector3 pos = new Vector3(Mathf.RoundToInt(transform.position.x + xRandom), transform.position.y + yRandom, 0);
            Quaternion rot = new Quaternion(0,0,0,0);

            currentTimer = 0f;
            if (objectToSpawn != null)
            {
                GameObject gameObject = Instantiate(objectToSpawn, pos, rot);
                gameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
        else
        {
            currentTimer += Time.deltaTime;
        }    

        if (timeBetweenSpawns > 0.02f) // The maximum spawn rate that is set that I consider to be tough enough to the point where the player cannot continue infinitely.
        {
            if (currentIntervalIncreaseTimer >= intervalIncreaseTimer)
            {
                timeBetweenSpawns *= intervalIncreaseAmount;
                currentIntervalIncreaseTimer = 0;
            }
            else
            {
                currentIntervalIncreaseTimer += Time.deltaTime;
            }    
        }
        
        
    }
}