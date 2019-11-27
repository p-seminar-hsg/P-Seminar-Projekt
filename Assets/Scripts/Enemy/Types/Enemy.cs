﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel und Benedikt Wille
/// Mitarbeiter: Florian Müller-Martin (Combatsystem und Animationen)
/// Zuletzt geändert am: 27.11.2019
/// Die Superklasse und damit Grundlage für alle Enemies
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float healthPoints_max;
    public float speed;
    public int strength;
    public float range;

    [Header("Knockback")]
    public float knockbackLength;
    public float attackKnockback;

    [Header("Scaling")]
    public int scoreForDefeat = 10;

    [Header("Cooldowns")]
    public float damageCooldown;
    public float attackCooldown;
    public float takeDamageCooldown;

    [Header("Drops")]
    public GameObject[] drops;
    [Range(0, 1)]
    [Tooltip("Die Wahrscheinlichkeiten für die Drops. WICHTIG: Jeder Index hier gehört zum selben Index in Drops")]
    public float[] dropProbs;

    [Header("Variablen für die Animation (Flomm)")]
    public Animator animator; //Link zum Animator
    public float actualMoveX, actualMoveY; //Die Bewegungswerte des Enemies im letzten Frame
    public float stoppedActualMoveX, stoppedActualMoveY; //Die Bewegungswerte des Enemies im letzten Frame, die aber beibehalten werden, wenn der Enemy sich nicht bewegt. Nötig für die Idle Animation
    public Vector2 PositionStartOfFrame; //Die Position am Anfang des Frames

    protected Rigidbody2D rb;
    public bool movementLocked;
    private string[] idleSounds = { "Zombie1", "Zombie3", "Zombie4" };

    /* Am Anfang 0 - Sobald der Enemy gehittet wird, wird localDamageCooldown
     * damageCooldown gleich gesetzt und dann jeden Frame um die verstrichene
     * Zeit verringert (muss in den erbenden Scripts extra implementiert werden!) */
    protected float localDamageCooldown;

    /// <summary>
    /// TakeHit-Methode, die jeder Enemy implementieren muss (wird aufgerufen sobald der Enemy getroffen wird) (überarbeitet von Flomm)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    /// <param name="damage">Schaden, der zugefügt wird</param>
    public abstract void TakeHit(Vector2 knockbackDirection, float damage);

    /// <summary>
    /// Knockback-Coroutine, die man zusammen mit TakeHit()  
    /// verwenden sollte (kann optional überschrieben werden!) (überarbeitet von Flomm)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    public virtual IEnumerator KnockbackCo(Vector2 knockbackDirection)
    {
        movementLocked = true;
        rb.velocity = knockbackDirection.normalized * knockbackLength * 17; // (17 ~= 5/0,3)
        yield return new WaitForSeconds(knockbackLength);
        rb.velocity = Vector2.zero;
        movementLocked = false;
    }

    /// <summary>
    /// Dropt mit einer gewissen Wahrscheinlichkeit ein oder mehrere Item(s) aus drops.
    /// Sollte in den Unterklassen beim Tod aufgerufen werden!
    /// (Kann bei Bedarf in den Unterklassen überschrieben werden)
    /// </summary>
    protected virtual void DropItem()
    {
        // Der Gegner kann mehrere Items droppen
        for (int i = 0; i < drops.Length; i++)
        {
            // Wahrscheinlichkeit
            float random = ((float)Random.Range(0, 100)) * 0.01f;
            if (dropProbs[i] != 0 && random <= dropProbs[i])
            {
                GameObject.Instantiate(drops[i], transform.position, Quaternion.identity);
            }
        }
    }

    public virtual void ScaleStats()
    {
        // Muss noch getestet werden ! (-Benedikt)
        int score = GameManager.GetScore();

        healthPoints_max += score / 150;
        speed += score / 2500;
        strength += score / 2500;
        //Debug.Log("Health: " + healthPoints_max + " / Speed: " + speed + " / Strength: " + strength);
    }

    private IEnumerator PlayRandomZombieSounds()
    {
        yield return new WaitForSeconds(Random.Range(7, 40));
        
        GameManager.PlaySound(Utility.ChooseRandom<string>(idleSounds));

        StartCoroutine("PlayRandomZombieSounds");
    }

    #region Animationen
    /// <summary>
    /// Aktualisiert die Bewegungs- und Blickrichtungsparameter des Animators
    /// </summary>
    public void refreshAnimator()
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
    #endregion
}
