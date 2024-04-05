using UnityEngine;
using TMPro;

public class FinishLine : MonoBehaviour
{
    public TMP_Text finishText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowFinishText(); 
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableMovement(); 
            }
        }
    }

    private void ShowFinishText()
    {
        finishText.gameObject.SetActive(true);
        Debug.Log("Finish text set active.");
    }
}