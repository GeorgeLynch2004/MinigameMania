using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour
{

    public void ToggleObject()
    {
        if (gameObject.activeSelf != true)
        {
            gameObject.SetActive(true); 
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
