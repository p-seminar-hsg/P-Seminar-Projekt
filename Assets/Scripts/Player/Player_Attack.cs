using System;
using UnityEngine;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz
/// Mitarbeiter: keine
/// Zuletzt geändert am 16.07.2019
/// Angriffsklasse des Players
/// </summary>
public class Player_Attack : MonoBehaviour
{
    //Bei einer Collision einer der Hitboxen mit einem Gegner wird dessen TakeHit-Methode aufgerufen.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyHitbox"))
        {
            GameObject player = GameObject.Find("Player");
            Enemy gegner = other.GetComponentInParent<Enemy>();
            try {
                gegner.TakeHit(other.transform.position - player.transform.position, player.GetComponent<Player_Main>().strength);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
        }
    }
}
