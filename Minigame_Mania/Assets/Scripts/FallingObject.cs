using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float _FallingSpeed;
    [SerializeField] private float m_TimeTillDestroy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyCountdown(m_TimeTillDestroy));
    }

    // Update is called once per frame
    private void FixedUpdate() {
        transform.position = new Vector2(transform.position.x, transform.position.y - _FallingSpeed);
    }

    private IEnumerator DestroyCountdown(float m_TimeTillDestroy)
    {
        yield return new WaitForSeconds(m_TimeTillDestroy);

        Destroy(gameObject);
    }
}
