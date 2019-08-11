using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HPIncreasement : MonoBehaviour
{
    public float increasment;
    public Color newHealthBarColor;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Main player = other.GetComponent<Player_Main>();
            player.enhanceMaxHP(increasment, newHealthBarColor);
        }
    }
}
