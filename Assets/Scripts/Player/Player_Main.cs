﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz
/// Zuletzt geändert am 01.07.2019
/// Mainklasse für den Player
/// </summary>
public class Player_Main : MonoBehaviour
{

    [Header("Stats")]
    static float HP; //Aktuelle Lebenspunkte des Players
    public float maxHP; //Maximale Lebenspunkte des Charakters
    public float strength; //Angriffswert des Players
    

  


    [Header("Unity Stuff")]
    public Image healthBarPauseMenu;//Referenz zur Healthbar im PauseMenu (Geändert von Rene Jokiel)

    private GameObject healthBar; //Referenz zur HealthBar
    private Animator animator; // Animator des Players
    public GameOver gameOver;


    private void Start()
    {
        HP = maxHP;
        healthBar = GameObject.Find("Bar"); //Referenz wird hergestellt
        animator = GameObject.Find("Player").GetComponent(typeof(Animator)) as Animator;
    }

    /// <summary>
    /// Schadensmethode des Players - Reduziert die HP des Players um den übergebenen Wert und stößt den Player in die übergebene Richtung weg
    /// </summary>
    /// <param name="damage">Der Schadenswert, der zugeführt werden soll</param>
    /// <param name="knockbackDirection">Richtung des Knockbacks</param>
    public void takeHit(GameObject enemy)
    {
        if (HP > 0)
        {
            HP -= enemy.GetComponent<Enemy>().strength;
        }

        StartCoroutine(GetComponent<Player_Movement>().KnockbackCo((transform.position - enemy.transform.position), enemy.GetComponent<Enemy>().attackKnockback));

    }

    /// <summary>
    /// Heilmethode des Players - Erhöht die HP des Players um den übergebenen Wert
    /// </summary>
    /// <param name="heal">Der Heilwert, der zugeführt werden soll</param>
    public void heal(float heal)
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
    public void strengthen(float strengthening)
    {
        strength += strengthening;
    }

    /// <summary>
    /// Sterbemethode des Players
    /// </summary>
    public void die()
    {

        gameOver.GoGameOver();  // Von Rene Jokiel
        Destroy(this);

    }


    /// <summary>
    /// Die passende Attack-Hitbox wird aktiviert
    /// </summary>
    public void attackMain()
    {

        float moveX = GameObject.Find("Player").GetComponent<Player_Movement>().moveX;
        float moveY = GameObject.Find("Player").GetComponent<Player_Movement>().moveY;

        float absMoveX = Mathf.Abs(moveX);
        float absMoveY = Mathf.Abs(moveY);

        /* Die Richtung in die der Spieler schaut, wird bestimmt
        * Ähnlich dem, was der Blend Tree macht, nur dass eine eindeutige
        * Richtung rauskommt, in der dann die AttackHitbox aktiviert wird */
        if (moveX > 0 && absMoveX > absMoveY) {
            // Spieler schaut nach rechts
            StartCoroutine("attackRight");
            Debug.Log("Debug von Flo: Angriff nach Rechts");
        }
        
        else if (moveY > 0 && absMoveY > absMoveX) {
            // Spieler schaut nach oben
            StartCoroutine("attackTop");
            Debug.Log("Debug von Flo: Angriff nach Oben");
        }
        
        else if (moveX < 0 && absMoveX > absMoveY) {
            // Spieler schaut nach links
            StartCoroutine("attackLeft");
            Debug.Log("Debug von Flo: Angriff nach Links");

        }

        else if (moveY < 0 && absMoveY > absMoveX) {
            // Spieler schaut nach unten
            StartCoroutine("attackBottom");
            Debug.Log("Debug von Flo: Angriff nach Unten");
        }
        
        else if (moveX == 0 && moveY == 0){
            // Wenn der Sp. in keine Richtung schaut, dann schaut er nach unten; wichtig wenn der Spieler vorher noch nicht gelaufen ist
            StartCoroutine("attackBottom");
            Debug.Log("Debug von Flo: Angriff nach Unten, weil nicht in Bewegung");
        }

}
    

    private IEnumerator attackBottom()
    {
        animator.SetFloat("attack", 1);
        Collider2D collider = GameObject.Find("hitboxBottom").GetComponent<Collider2D>();
        collider.enabled = true;
        yield return Utility.Wait(1);
        animator.SetFloat("attack", 0);
        collider.enabled = false;
        
    }

    private IEnumerator attackRight()
    {
        animator.SetFloat("attack", 2);        
        yield return Utility.Wait(2);
        animator.SetFloat("attack", 0);
        
    }

    private IEnumerator attackTop()
    {
        animator.SetFloat("attack", 3);        
        yield return Utility.Wait(2);
        animator.SetFloat("attack", 0);
        
    }

    private IEnumerator attackLeft()
    {
        animator.SetFloat("attack", 4);        
        yield return Utility.Wait(2);
        animator.SetFloat("attack", 0);
        
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
        healthBar.transform.localScale = new Vector3(HP / maxHP, 1f, 1f);
        healthBarPauseMenu.fillAmount = HP / maxHP;// Von Rene Jokiel

        //Selbstgeißelung auf Leertaste zu Demonstrationszwecken
        if (Input.GetKey(KeyCode.Space))
        {
            HP -= 1;
        }
    }


    //Nur für Testzwecke
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Enemy"))
        {
            //Debug.Log("Debug von Flo: Collision with Enemy");
        }
    }

    



}
