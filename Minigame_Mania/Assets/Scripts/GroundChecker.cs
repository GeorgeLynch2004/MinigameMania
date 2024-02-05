using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMovement;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Ground")
        {
            _playerMovement.setIsGrounded(true);  
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Ground")
        {
            _playerMovement.setIsGrounded(false); 
        }
    }
}
