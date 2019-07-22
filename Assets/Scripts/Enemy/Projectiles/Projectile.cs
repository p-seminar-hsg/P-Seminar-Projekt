//Authors: Joshua B.; Patrick M.    Main Author: Patrick M.      Last Update: 18.06.2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetimePattern;       //Gibt an, wie lange das Projektil leben soll
    public float movementSpeed;
    public GameObject deatheffect;      //ParticleSystem empfelenswert (Added by Rene Jokiel)
    public float lifetime;
    public Vector2 direction;
    public Transform player;
    public float strength;

    public void SelfDestruct()
    {
        GameObject effect = (GameObject)Instantiate(deatheffect, transform.position, Quaternion.identity);  //Deatheffect wird gespawnt
        Destroy(this.gameObject);   //Projektil wird zerstört
        Destroy(effect, 5f);    //Der Deatheffect wird auch zerstört
    }

    public void TakeHit()
    {
        SelfDestruct();     //Nach einem Treffer wird das P. zerstört. Abhängig vom Playerscript(Aufruf der Methode über den Player). Methode kann erweitert werden, je nachdem, was passieren soll,
                            // wenn man ein Pro. zerstört.
    }

    public void Chase(Transform target)
    {
        lifetime = lifetimePattern;
        direction = target.position - transform.position;       //Richtung, in die es fliegen soll, wird gesetzt
        player = target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {  
      if (other.CompareTag("Player"))
        {
          Player_Main player = other.GetComponent<Player_Main>();
          player.takeHit(this.gameObject);    //, player.transform.position - transform.position, attackKnockback, falls man noch irgendwie Knockback haben möchte   
            SelfDestruct();
        }
    }
}
