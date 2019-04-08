using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy {       //Dieses Script kann für verschiedene Gegner verwendet werden und dient somit als erstes Grundgerüst. Im EnemyI Folder sind 3 Varianten des Gegners.

    public Transform target;

    // Use this for initialization
    void Start () {

        GetTarget();    //Das Ziel wird gleich dem Spieler gleichgesetzt, da es eh nur ein Ziel für die Gegner geben wird.
	}

    public void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update () {
        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) <= range)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);  //Der Gegner verfolgt den Spieler mittels Vektoraddition,
            }                                                                                                          //wenn der Spieler in seinem Verfolgungsradius ist.


            if (Vector2.Distance(target.position, transform.position) <= attackRadius)
            {
               HitTarget();     //Wenn der Spieler im Angriffsradius ist, wird der Spieler angegriffen.

            }
        if(this == this)    //Totaler Schwachsinn. Nur ein Platzhalter, bis die Player Menschen aufzeigen können, wann der Gegner Schaden nimmt.
            {               // Der Dmg. Cooldown kann entweder in diesem Script oder im Player Script eingebaut werden. Hier ist er jetzt Mal eingebaut.
                if (takeDamageCooldown <= 0f)
                {
                    TakeDamage();
                    takeDamageCooldown = 2f;
                }
            }
            takeDamageCooldown -= Time.deltaTime;   //Dadurch wird der Cooldown so lange runtergesetzt, bis wieder Schaden genommen werden kann.
        if(healthPoints <= 0)   //Das kleiner ist in sofern wichtig, dass wenn der Player mehr Schaden macht, als der Geg. Leben hat, der Geg. nicht unsterblich umherwandert
            {
                Die();
            }
        }

    }


    void HitTarget()
    {
        Damage(target); //Die Schadensmethode wird aufgerufen, der Player hat Aua
        Wait(0.5f); // Cooldown von 0.5s

    }

    void Damage(Transform enemy)
    {
        Destroy(enemy.gameObject);  //Die Zeile muss durch ein "Player.Live -= strength;" ersetzt werden, oder der gleichen. (Grüße an Flomm)
        target = null;      // Die Zeile kann gelöscht werden, wenn es PlayerStats geben. Das Ziel muss nur zerstört werden, wenn der Spieler stirbt (Nullpointer), wird aber wg. GameOver nicht
                            // benötigt.
    }

    private void OnDrawGizmosSelected()     //Nur in der Scene relevant, nicht im Spiel
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    IEnumerator Wait(float time)    //Ein IEnumerator, um Verzögerungen zu erzeugen. Ich dachte, ich würde ihn brauchen, aber naja.. Kommt vielleicht noch.
    {
        yield return new WaitForSeconds(time);
    }

    void TakeDamage()
    {
        healthPoints -= 1; // healthPoints =-1 * Player.Atk * Item.Boost, oder so. Hängt von den Player und Item Menschen ab.
    }

    void Die()  //Das muss ich jetzt wahrscheinlich nicht erklären.
    {
        Destroy(this.gameObject);
        return;
    }
}

