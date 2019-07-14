
/// <summary>
/// Erstellt von Rene Jokiel
/// Dieses Script kann für verschiedene Gegner verwendet werden
/// und dient somit als erstes Grundgerüst.
/// Im EnemyI Folder sind 3 Varianten des Gegners.
/// </summary>

using UnityEngine;

public class Enemy1 : Enemy {

    private Transform playerTransform;

    void Awake()
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
        DropHeart();

        MapManager mapManagerInstance = MapManager.instance;
        mapManagerInstance.currentRoomScript.ReduceEnemiesAlive();
        mapManagerInstance.CheckForAllEnemiesDied();
        GameManager.AddToScore(10);

        Destroy(this.gameObject);
        return;
    }

    // Bei Collision mit dem Player wird dessen TakeHit-Methode aufgerufen und dieses GameObject übergeben
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Main>().takeHit(this.gameObject);
            //Debug.Log("Debug von Flo: " + this.name + " ist mit Player kollidiert");
        }
    }
}

