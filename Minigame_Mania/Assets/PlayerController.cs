using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 3f;
    private bool canMove = false;
    public TMP_Text countdownText;
    public TMP_Text timerText;

    private float timer = 0f;

    void Start()
    {
        StartCoroutine(StartGameCountdown());
    }

    void Update()
    {
        if (canMove)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }

        if (canMove && Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(Vector2.right * moveDistance);
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    IEnumerator StartGameCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        canMove = true;
        countdownText.gameObject.SetActive(false);
        timer = 0f;
    }

    void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(timer);
        int milliseconds = Mathf.FloorToInt((timer - seconds) * 1000);
        string timerString = string.Format("{0:00}:{1:000}", seconds, milliseconds);

        timerText.text = timerString;
    }
}

