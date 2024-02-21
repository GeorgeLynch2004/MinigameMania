using System.Collections;
using UnityEngine;
using System.Threading;

public class rolldice : MonoBehaviour
{
    private SpriteRenderer render;
    private Sprite[] diceSides;
    public int sortingOrder = 10;
    public int result;


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

   public  void roll()
    {
        StartCoroutine("RollDice");
    }
    public IEnumerator RollDice()
    {
        
        int randomDiceSide = 0;
        
        for (int i = 0; i <= 15; i++)
        {

            randomDiceSide = Random.Range(0, diceSides.Length);
            render.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
        result = randomDiceSide + 1;

        
    }
    public int GetResult()
    {
        return result;
    }
}
