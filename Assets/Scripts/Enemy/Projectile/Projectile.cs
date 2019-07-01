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
}
