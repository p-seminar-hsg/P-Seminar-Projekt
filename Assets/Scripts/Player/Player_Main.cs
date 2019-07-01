using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mainklasse für den Player
/// </summary>
public class Player_Main : MonoBehaviour
{
    /* Ersteller: Florian Müller-Martin und Tobias Schwarz
     * Zuletzt geändert am 23.05.2019
     * Funktion: Dieses Skript steuert die Hauptfunktionen des Players
     */

    private float HP; //Aktuelle Lebenspunkte des Players
    public float maxHP; //Maximale Lebenspunkte des Charakters
    private float strength; //Angriffswert des Players
    private GameObject healthBar; //Referenz zur HealthBar

    private void Start()
    {
        HP = maxHP; 
        healthBar = GameObject.Find("Bar"); //Referenz wird hergestellt
    }

    /// <summary>
    /// Schadensmethode des Players - Reduziert die HP des Players um den übergebenen Wert
    /// </summary>
    /// <param name="damage">Der Schadenswert, der zugeführt werden soll</param>
    public void takeDamage  (float damage) {
        if (HP > 0)
        {
            HP -= damage;
        }
        Debug.Log("Health reduced to: " + HP);
        
    }

    /// <summary>
    /// Heilmethode des Players - Erhöht die HP des Players um den übergebenen Wert
    /// </summary>
    /// <param name="heal">Der Heilwert, der zugeführt werden soll</param>
    public void heal (float heal)
    {
        HP += heal;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        Debug.Log("Health set to: " + HP);
    }

    /// <summary>
    /// Stärkemethode des Players - Erhöht den Schadenswert des Players um den übergebenen Wert
    /// </summary>
    /// <param name="strengthening">Der Schadenswert, der zugeführt werden soll</param>
    public void strengthen (float strengthening)
    {
        strength += strengthening;
    }

    /// <summary>
    /// Sterbemethode des Players - Das GameObject wird zerstört
    /// </summary>
    public void die()
    {
        Destroy(this);
    }

    /// <summary>
    /// Angriffsmethode des Players
    /// </summary>
    public void attack(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().TakeHit(enemy.transform.position - transform.position);
    }

    // FixedUpdate wird einmal pro Frame aufgerufen
    private void FixedUpdate()
    {
        //Wenn der Player keine Leben mehr hat wird die Methode die() aufgerufen
        if (HP <= 0)
        {
            die();
        }

        //Die Health Bar wird aktualisiert
        healthBar.transform.localScale = new Vector3(HP/maxHP, 1f, 1f);

       
    }

    void OnCollisionEnter2D(Collision2D collsion)
    {
        if( collsion.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision with Enemy");
            attack(collsion.gameObject);
        }
    }




}
