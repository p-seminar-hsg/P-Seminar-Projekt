
using UnityEngine;


/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz
/// Zuletzt geändert am 14.07.2019
/// Angriffsklasse des Players
/// </summary>
public class Player_Attack : MonoBehaviour
{

    //Bei einer Collision einer der Hitboxen mit einem Gegner wird dessen TakeHit-Methode aufgerufen.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))

            
        {
            GameObject player = GameObject.Find("Player");
            Enemy gegner = other.GetComponentInParent<Enemy>();
            gegner.TakeHit(other.transform.position-player.transform.position, player.GetComponent<Player_Main>().strength);
            
            
        }
    }

}
