using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main : MonoBehaviour
{
    /* Ersteller: Florian Müller-Martin und Tobias Schwarz
     * Zuletzt geändert am 23.05.2019
     * Funktion: Dieses Skript steuert die Hauptfunktionen des Players
     */

    private int HP;
    private int strength;

    //Die HP des Players werden um den übergebenen Wert reduziert
    public void takeDamage  (int damage) {
        HP -= damage;
    }

    //Die HP des Players werden um den übergebenen Wert erhöht
    public void heal (int heal)
    {
        HP += heal;
    }

    //Die Stärke des Players wird um den übergebenen Wert erhöht
    public void strengthen (int strengthening)
    {
        strength += strengthening;
    }

    //Der Player stirbt und zerstört sich selbst
    void die()
    {
        Destroy(this);
    }


    // FixedUpdate wird einmal pro Frame aufgerufen
    private void FixedUpdate()
    {
        //Wenn der Player keine Leben mehr hat wird die Methode die() aufgerufen
        if (HP <= 0)
        {
            die();
        }
    }




}
