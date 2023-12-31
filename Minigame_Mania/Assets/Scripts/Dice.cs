using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private int m_numberOfSides;
    [SerializeField] private Animation m_animation;

    public int rollDice()
    {
        int result = Random.Range(1, m_numberOfSides);
        m_animation.Play(); 
        return result;
    }
}
