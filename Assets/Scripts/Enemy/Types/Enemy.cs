using System.Collections;
using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel und Benedikt Wille
/// Mitarbeiter: Florian Müller-Martin (Combatsystem)
/// Zuletzt geändert am: 19.11.2019
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

    protected Rigidbody2D rb;
    public bool movementLocked;

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

}
