using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private int m_numberOfSides;
    [SerializeField] private Animation m_animation;

    // Method to roll dice and play animation for the dice GameObject
    public int rollDice()
    {
        int result = Random.Range(1, m_numberOfSides);
        m_animation.Play(); 
        return result;
    }
}
