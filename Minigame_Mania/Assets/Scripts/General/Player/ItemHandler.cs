using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class ItemHandler : NetworkBehaviour
{
    /*
        Necessary Variables:

            Transform attach point
            NetworkObject objectInHand
            Bool handsFull
            String[] permittedPickupTags
            NetworkObject[] collidedObjects

        Necessary Methods:

            void attachItem(GameObject objectToAttach)
            void detachItem()
            NetworkObject getObjectInHand

    */

    [SerializeField] private Transform m_AttachPoint;
    [SerializeField] private GameObject m_ObjectInHand;
    [SerializeField] private bool m_HandsFull;
    [SerializeField] private string[] m_PermittedPickupTags;
    [SerializeField] private List<GameObject> m_CollidedObjects;

    private void Update() 
    {
        // if pickup button is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            // if hands are empty
            if (!m_HandsFull)
            {
                // search the current collisions
                foreach (GameObject gameObject in m_CollidedObjects)
                {
                    // if the game object has a permitted tag for picking up
                    if (m_PermittedPickupTags.Contains(gameObject.tag))
                    {
                        // call the attach function to pick it up
                        attachItem(gameObject);
                        return;
                    }
                }
            }
            // if hands are full
            else
            {
                // detach the current object
                detachItem();
            }
            
        }    
    }

    public void attachItem(GameObject objectToAttach)
    {
        // Set the object as a child of the attach point
        objectToAttach.transform.SetParent(m_AttachPoint);
        // reset the local position of the object so it fits snug
        objectToAttach.transform.localPosition = Vector3.zero;
        // set the object in hand
        m_ObjectInHand = objectToAttach;
        // set hands full to true
        m_HandsFull = true;
    }

    public void detachItem()
    {
        // detach the child from the attach point
        m_AttachPoint.DetachChildren();
        // set object in hand to null
        m_ObjectInHand = null;
        // set hands full to false
        m_HandsFull = false;
    }

    public GameObject getObjectInHand()
    {
        return m_ObjectInHand;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!m_CollidedObjects.Contains(other.gameObject) && m_PermittedPickupTags.Contains(other.gameObject.tag))
        {
            m_CollidedObjects.Add(other.gameObject);
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (m_CollidedObjects.Contains(other.gameObject))
        {
            m_CollidedObjects.Remove(other.gameObject);
        }    
    }
}
