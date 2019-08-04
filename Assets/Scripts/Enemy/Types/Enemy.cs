using System.Collections;
using UnityEngine;

/// <summary>
/// Erstellt von Rene Jokiel und Benedikt Wille
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
    public float knockbackStrength; // knockbackStrength ist eig. nur für die Entwicklung public
    public float knockbackLength;   // bzw. sollte vielleicht i.wann mit knockbackLength zusammengelegt werden
    public float attackKnockback;

    [Header("Cooldowns")]
    public float damageCooldown;
    public float attackCooldown;
    public float takeDamageCooldown;

    [Header("Drops")]
    public GameObject[] drops;
    [Range(0, 1)]
    [Tooltip("Die Wahrscheinlichkeiten für die Drops. WICHTIG: Jeder Index hier gehört zum selben Index in Drops")]
    public float[] dropProbs;

    protected Rigidbody2D rb;
    public bool movementLocked;

    /* Am Anfang 0 - Sobald der Enemy gehittet wird, wird localDamageCooldown
     * damageCooldown gleich gesetzt und dann jeden Frame um die verstrichene
     * Zeit verringert (muss in den erbenden Scripts extra implementiert werden!) */
    protected float localDamageCooldown;

    /// <summary>
    /// TakeHit-Methode, die jeder Enemy implementieren muss 
    /// (wird aufgerufen sobald der Enemy getroffen wird)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    public abstract void TakeHit(Vector2 knockbackDirection, float damage);

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

    public virtual void IncreaseStats()
    {
        int Score = GameManager.GetHighscore();

        healthPoints_max += Score / 100;
        speed += Score / 2000;
        strength += Score / 2000;

    }

}
