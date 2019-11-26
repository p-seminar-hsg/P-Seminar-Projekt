﻿/// <summary>
/// Ersteller: Rene Jokiel
/// Mitarbeiter: Benedikt Wille, Florian Müller-Martin (Combatsystem und Animationen), Luca Kellermann (Sounds)
/// Zuletzt geändert am: 25.11.2019
/// Dieses Script kann für verschiedene Gegner verwendet werden
/// und dient somit als erstes Grundgerüst.
/// Im EnemyI Folder sind 3 Varianten des Gegners.
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy1 : Enemy
{
    private Transform playerTransform;

    [Header("Unity Stuff")]
    public Image healthBar;
    public GameObject deatheffect;
    public float currentHealthpoints;

    public float localAttackCooldown;
    public bool onTriggerStayCooldown;

    [Header("Variablen für die Animation (Flomm)")]
    public Animator animator; //Link zum Animator
    public float actualMoveX, actualMoveY; //Die Bewegungswerte des Enemies im letzten Frame
    public float stoppedActualMoveX, stoppedActualMoveY; //Die Bewegungswerte des Enemies im letzten Frame, die aber beibehalten werden, wenn der Enemy sich nicht bewegt. Nötig für die Idle Animation
    public Vector2 PositionStartOfFrame; //Die Position am Anfang des Frames

    void Awake()
    {
        // Das Ziel wird gleich dem Spieler gleichgesetzt, da es eh nur ein Ziel für die Gegner geben wird.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ScaleStats();
        currentHealthpoints = healthPoints_max;
        localAttackCooldown = attackCooldown;
        onTriggerStayCooldown = true;
        StartCoroutine("PlayRandomZombieSounds");
    }

    /// <summary>
    /// Überschriebene TakeHit-Methode aus Enemy (überarbeitet von Flomm)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    public override void TakeHit(Vector2 knockbackDirection, float damage)
    {

        // Kein Hit möglich, wenn localDamageCooldown noch nicht abgelaufen
        if (localDamageCooldown > 0)
            return;

        GameManager.PlaySound("Zombie2");

        currentHealthpoints -= damage;


        // localDamageCooldown wird "aktiviert" (und beginnt abzulaufen)
        localDamageCooldown = damageCooldown;

        // KnockbackCo -> siehe Enemy-Script
        StartCoroutine(KnockbackCo(knockbackDirection));

        if (currentHealthpoints <= 0)
            Die();
    }


    private void Update()
    {
        if (playerTransform != null)
        {
            // Dadurch wird der Cooldown so lange runtergesetzt, bis wieder Schaden genommen werden kann.
            if (localDamageCooldown > 0)
                localDamageCooldown -= Time.deltaTime;

            if (localAttackCooldown > 0)
            {
                localAttackCooldown -= Time.deltaTime;
            }
        }
        healthBar.fillAmount = currentHealthpoints / healthPoints_max;
    }

    private void FixedUpdate()
    {
        //Position wird zu Beginn gespeichert
        PositionStartOfFrame = transform.position;
    }

    private void LateUpdate() //Nach Update und LateUpdate werden dem Animator die benötigten Werte übergeben (Flomm)
    {
        actualMoveY = (this.gameObject.transform.position.y - PositionStartOfFrame.y) * 10;
        actualMoveX = (this.gameObject.transform.position.x - PositionStartOfFrame.x) * 10;

        if (this.gameObject.transform.position.y - PositionStartOfFrame.y != 0) //Wird nur aktualisiert, wenn der Enemy sich bewegt hat
        {
            stoppedActualMoveY = (this.gameObject.transform.position.y - PositionStartOfFrame.y) * 10;
        }

        if (this.gameObject.transform.position.x - PositionStartOfFrame.x != 0) //Wird nur aktualisiert, wenn der Enemy sich bewegt hat
        {
            stoppedActualMoveX = (this.gameObject.transform.position.x - PositionStartOfFrame.x) * 10;
        }

        animator.SetFloat("speed_horizontal", actualMoveX);
        animator.SetFloat("speed_vertical", actualMoveY);

        //View Direction wird als Float übergeben, Zahlenwerte parallel zur Anordnung der Idle-Animationen im BlendTreeIdle
        Direction viewDirection = getViewDirection();
        if (viewDirection == Direction.DOWN)
        {
            animator.SetFloat("viewDirection", 1);
        }
        else if (viewDirection == Direction.RIGHT)
        {
            animator.SetFloat("viewDirection", 2);
        }
        else if (viewDirection == Direction.UP)
        {
            animator.SetFloat("viewDirection", 3);
        }
        else if (viewDirection == Direction.LEFT)
        {
            animator.SetFloat("viewDirection", 4);
        }

        /*
        if (actualMoveX > 0 || actualMoveY > 0) {
            Debug.Log(actualMoveX + ", " + actualMoveY);
        }
        */
    }

    /// <summary>
    /// Diese Methode gibt die aktuelle Blickrichtung des Enemies als Direction zurück
    /// </summary>
    public Direction getViewDirection()
    {

        // Wenn der Enemy in keine Richtung schaut, dann schaut er nach unten; wichtig wenn der Enemy vorher noch nicht gelaufen ist}
        Direction viewDirection = Direction.DOWN;
        
        float absMoveX = Mathf.Abs(stoppedActualMoveX);
        float absMoveY = Mathf.Abs(stoppedActualMoveY);

        // Die Richtung in die der Enemy schaut, wird bestimmt

        if (stoppedActualMoveX > 0 && absMoveX > absMoveY)
        {
            // Enemy schaut nach rechts
            viewDirection = Direction.RIGHT;
        }

        else if (stoppedActualMoveY > 0 && absMoveY > absMoveX)
        {
            // Enemy schaut nach oben
            viewDirection = Direction.UP;
        }

        else if (stoppedActualMoveX < 0 && absMoveX > absMoveY)
        {
            // Enemy schaut nach links
            viewDirection = Direction.LEFT;
        }

        else if (stoppedActualMoveY < 0 && absMoveY > absMoveX)
        {
            // Enemy schaut nach unten
            viewDirection = Direction.DOWN;
        }

        return viewDirection;
    }

    /// <summary>
    /// Nur im Editor relevant, nicht im Spiel
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    /// <summary>
    /// Das muss ich jetzt wahrscheinlich nicht erklären.
    /// </summary>
    void Die()
    {
        // Wahrscheinlichkeit wird intern berechnet
        DropItem();

        MapManager mapManagerInstance = MapManager.instance;
        mapManagerInstance.currentRoomScript.ReduceEnemiesAlive();
        mapManagerInstance.CheckForAllEnemiesDied();
        GameManager.AddToScore(scoreForDefeat);

        GameObject effect = (GameObject)Instantiate(deatheffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(effect, 5f);

        return;
    }

    /// <summary>
    /// Wenn der Kollisionspartner der Player ist, wird dessen TakeHit-Methode aufgerufen (Flomm)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (localAttackCooldown <= 0)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player_Main>().takeHit(this.gameObject);
                //Debug.Log("Debug von Flo: " + this.name + " ist mit Player kollidiert");
                localAttackCooldown = attackCooldown;
            }
        }
    }

    private IEnumerator onTriggerStayCooldownCo()
    {
        onTriggerStayCooldown = false;
        yield return new WaitForSeconds(1);
        onTriggerStayCooldown = true;
    }

    // Wenn der Gegner in der Hitbox des Players bleibt fügt er ihm jede Sekunde Schaden zu
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (onTriggerStayCooldown)
            {
                other.GetComponent<Player_Main>().takeHit(this.gameObject);
                StartCoroutine("onTriggerStayCooldownCo");
            }
        }
    }
}
