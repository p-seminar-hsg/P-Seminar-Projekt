using UnityEngine;

/// <summary>
/// Dieses Script kann für verschiedene Gegner verwendet werden
/// und dient somit als erstes Grundgerüst.
/// Im EnemyI Folder sind 3 Varianten des Gegners.
/// </summary>
public class Enemy1 : Enemy {

    private Transform playerTransform;

    private void Start()
    {
        // Das Ziel wird gleich dem Spieler gleichgesetzt, da es eh nur ein Ziel für die Gegner geben wird.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Überschriebene TakeHit-Methode aus Enemy
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    public override void TakeHit(Vector2 knockbackDirection, float damage)
    {
        // Kein Hit möglich, wenn localDamageCooldown noch nicht abgelaufen
        if (localDamageCooldown > 0)
            return;

        healthPoints -= damage;

        // localDamageCooldown wird "aktiviert" (und beginnt abzulaufen)
        localDamageCooldown = damageCooldown;

        // KnockbackCo -> siehe Enemy-Script
        StartCoroutine(KnockbackCo(knockbackDirection));

        if (healthPoints <= 0)
            Die();
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // Dadurch wird der Cooldown so lange runtergesetzt, bis wieder Schaden genommen werden kann.
            if (localDamageCooldown > 0)
                localDamageCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!movementLocked)
        {
            // Wenn der Player in der Range ist ... 
            if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(range, 2))
            {
                // ... bewegt sich der Gegner auf ihn zu
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            }
        }
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
        Destroy(this.gameObject);
        return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Main player = other.GetComponent<Player_Main>();
            player.takeDamage(strength);    //, player.transform.position - transform.position, attackKnockback, falls man noch irgendwie Knockback haben möchte
        }
    }
}

