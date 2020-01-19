using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Mitarbeiter: Benedikt Wille, Florian Müller-Martin (Combatsystem und Animationen), Luca Kellermann (Sounds) <br/>
/// Zuletzt geändert am: 27.11.2019 <br/>
/// Dieses Script kann für verschiedene Gegner verwendet werden und dient somit als erstes Grundgerüst. <br/>
/// Im EnemyI Folder befinden sich 3 Varianten des Gegners.
/// </summary>
public class Enemy1 : Enemy
{
    private Transform playerTransform;

    [Header("Unity Stuff")]
    public Image healthBar;
    public GameObject deatheffect;
    public float currentHealthpoints;

    public float localAttackCooldown;
    public bool onTriggerStayCooldown;



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
        // Position wird zu Beginn gespeichert
        positionStartOfFrame = transform.position;
    }

    private void LateUpdate() //Nach Update werden dem Animator die benötigten Werte übergeben (Flomm)
    {
        RefreshAnimator(false);
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

    private IEnumerator OnTriggerStayCooldownCo()
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
                StartCoroutine("OnTriggerStayCooldownCo");
            }
        }
    }
}
