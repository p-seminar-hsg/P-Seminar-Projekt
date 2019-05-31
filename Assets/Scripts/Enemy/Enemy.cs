using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Grundlage für alle Enemies
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    
    [Header("Stats")]
    public float healthPoints;
    public float speed;
    public int strength;
    public float range;

    [Header("Knockback")]
    public float knockbackStrength; // knockbackStrength ist eig. nur für die Entwicklung public
    public float knockbackLength;   // bzw. sollte vielleicht i.wann mit knockbackLength zusammengelegt werden
    public float attackKnockback;

    [Header("Cooldowns")]
    public float damageCooldown;

    protected Rigidbody2D rb;
    protected bool movementLocked;

    /* Am Anfang 0 - Sobald der Enemy gehittet wird, wird localDamageCooldown
     * damageCooldown gleich gesetzt und dann jeden Frame um die verstrichene
     * Zeit verringert (muss in den erbenden Scripts extra implementiert werden!) */
    protected float localDamageCooldown;

    /// <summary>
    /// TakeHit-Methode, die jeder Enemy implementieren muss 
    /// (wird aufgerufen sobald der Enemy getroffen wird)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    public abstract void TakeHit(Vector2 knockbackDirection);

    /// <summary>
    /// Knockback-Coroutine, die man zusammen mit TakeHit()  
    /// verwenden sollte (kann optional überschrieben werden!)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    public virtual IEnumerator KnockbackCo(Vector2 knockbackDirection)
    {
        movementLocked = true;
        rb.velocity = knockbackDirection.normalized * knockbackStrength;
        yield return new WaitForSeconds(knockbackLength);
        rb.velocity = Vector2.zero;
        movementLocked = false;
    }

}




