using System.Collections;
using UnityEngine;

public class rolldice : MonoBehaviour
{
    private SpriteRenderer render;
    private Sprite[] diceSides;
    public int sortingOrder = 10;



    private void Start()
    {

        
        render = GetComponent<SpriteRenderer>();
        render.sortingOrder = sortingOrder;
        diceSides = Resources.LoadAll<Sprite>("Dice/");
    }

    
    private void OnMouseDown()
    {
        StartCoroutine("RollDice");
    }

   
    private IEnumerator RollDice()
    {
        
        int randomDiceSide = 0;

        
        

        
        for (int i = 0; i <= 15; i++)
        {

            randomDiceSide = Random.Range(0, diceSides.Length);
            render.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
        int result = randomDiceSide + 1;

        
    }
}
