using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    public float speed = 0.5f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
