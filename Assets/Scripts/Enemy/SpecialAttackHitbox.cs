using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Erstellt von Rene Jokiel am 5.8.2019
/// Dieses Script ermöglicht einem GO dem Player Schaden zuzufügen
/// </summary>


public class SpecialAttackHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Main>().takeHit(this.gameObject);
        }
    }
}
