using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundObject : MonoBehaviour
{
    [SerializeField, Tooltip("Add tags into this array that you want to be destroyed on collision.")] 
    private string[] _TagsToDestroy;

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (_TagsToDestroy.Contains(other.gameObject.tag))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (_TagsToDestroy.Contains(other.gameObject.tag))
        {
            Destroy(other.gameObject);
        } 
    }
}
