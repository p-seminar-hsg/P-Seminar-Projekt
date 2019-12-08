using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Erstellt von Rene Jokiel am 5.8.2019
/// Mitarbeiter: Florian Müller-Martin (Animationen), Luca Kellermann (Sounds)
/// Dieses Script ist für das Verhalten des Bossgegners zuständig
/// Zuletzt bearbeitet: 25.11.2019
/// </summary>

public class BossEnemy : Enemy
{

    private Transform playerTransform;

    [Header("Unity Stuff")]
    public Image healthBar;
    public GameObject deatheffect;
    public float currentHealthpoints;
    public GameObject firePoint;

    public float localAttackCooldown;
    public bool onTriggerStayCooldown;

    [Header("Radien - Relevant für die AI")]
    public float meleRadius;
    public float nearShootingRadius;
    public float snipingRadius;

    [Header("Cooldowns - Mehr Angriffe, mehr Cooldowns")]
    public float meleCooldownPattern;
    public float nearShootingCooldownPattern;
    public float snipingCooldownPattern;

    public float meleCooldown;
    private float nearShootingCooldown;
    private float snipingCooldown;

    [Header("Angriffsprefabs - GOs, die gebraucht werden")]
    public GameObject projectileFar;
    public GameObject projectileNear;
    public GameObject attackBoxMele;

    [Header("Variablen für die Animation (Flomm)")]
    public float attackType;
    public bool isAttack = false;



    void Awake()
    {
        // Das Ziel wird gleich dem Spieler gleichgesetzt, da es eh nur ein Ziel für die Gegner geben wird.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        currentHealthpoints = healthPoints_max;
        localAttackCooldown = attackCooldown;
        onTriggerStayCooldown = true;

        meleCooldown = meleCooldownPattern;
        nearShootingCooldown = nearShootingCooldownPattern;
        snipingCooldown = snipingCooldownPattern;
    }

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
    /// Überschriebene TakeHit-Methode aus Enemy
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
        {
            //TODO
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // Dadurch werden alle Cooldowns runtergezählt, so dass der Gegner wieder was machen kann
            if (localDamageCooldown > 0)
                localDamageCooldown -= Time.deltaTime;

            if (localAttackCooldown > 0)
            {
                localAttackCooldown -= Time.deltaTime;
            }
            if (meleCooldown > 0)
            {
                meleCooldown -= Time.deltaTime;
            }
            if (nearShootingCooldown > 0)
            {
                nearShootingCooldown -= Time.deltaTime;
            }
            if (snipingCooldown > 0)
            {
                snipingCooldown -= Time.deltaTime;
            }
            if(currentHealthpoints <= 0)
            {
                Die();
            }
        }
        healthBar.fillAmount = currentHealthpoints / healthPoints_max;
    }

    private void FixedUpdate()  //Je nach den Spähren, in denen sich der Player befindet, wird ein anderer Angriff ausgeführt
    {
        //Position wird zu Beginn gespeichert
        PositionStartOfFrame = transform.position;

        if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(snipingRadius, 2))     // Wenn du im snipingRadius bist...
        {
            if (Vector3.SqrMagnitude(playerTransform.position - transform.position) > Mathf.Pow(nearShootingRadius, 2))     // ... aber nicht nicht im nearShootingRadius...
            {
                if (snipingCooldown <= 0)
                {
                    ShootFar();     // ... wird geschossen
                    attackType = 1;  // ... und der Angriffstyp für die Animation aktualisiert (Flomm)
                    isAttack = true;
                    
                    
                }
            }

            if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(nearShootingRadius, 2))
            {
                if (Vector3.SqrMagnitude(playerTransform.position - transform.position) > Mathf.Pow(meleRadius, 2))
                {
                    if (nearShootingCooldown <= 0)
                    {
                        ShootMedium(Random.Range(1, 7));
                        attackType = 2;
                        isAttack = true;
                        
                        ;
                    }
                }

                if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(meleRadius, 2))
                {
                    if (meleCooldown <= 0)
                    {
                        MeleAttack();
                        attackType = 3;
                        isAttack = true;
                        
                        
                    }
                }
            }
        }
        if (Vector3.SqrMagnitude(playerTransform.position - transform.position) < Mathf.Pow(range, 2))
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);     // Bist du drin, geht er in den Nahkampf
        }

        animator.SetFloat("attack_type", attackType); //Animator wird aktualisiert (Flomm)

    }

    private void LateUpdate() //Nach Update und LateUpdate werden dem Animator die benötigten Werte übergeben (Flomm)
    {
        refreshAnimator(false);
        if (isAttack)
        {
            StartCoroutine("resetAnimationAttack");
        }        
    }
    /// <summary>
    /// Setzt die Angriffsanimation zurück (Flomm)
    /// </summary>
    private IEnumerator resetAnimationAttack()
    {
        yield return new WaitForSeconds(0.21f);
        attackType = 0;
        isAttack = false;
    }

    private void MeleAttack()   
    {
        attackBoxMele.SetActive(true);
        meleCooldown = meleCooldownPattern * 1000000;     //Cooldown wird unglaublich hoch gesetzt, so dass der Angriff nur ein Mal ausgeführt werden kann
    }

    private void ShootMedium(int amount)
    {
        for (int i = 0; i <= amount; i++)   // Eine zufällige Anzahl an Projektilen werden abgefeuert
        {
            GameObject projGO = (GameObject)Instantiate(projectileNear, firePoint.transform.position, firePoint.transform.rotation);      //Geschütz wird "gespawnt"
            Projectile projectile = projGO.GetComponent<Projectile>();

            projectile.Chase(playerTransform);
        }

        nearShootingCooldown = nearShootingCooldownPattern;
    }

    private void ShootFar()
    {
        GameObject projGO = (GameObject)Instantiate(projectileFar, firePoint.transform.position, firePoint.transform.rotation);      //Geschütz wird "gespawnt"
        Projectile projectile = projGO.GetComponent<Projectile>();

        projectile.Chase(playerTransform);

        snipingCooldown = snipingCooldownPattern;

    }

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

    private void OnDrawGizmosSelected() //Damit man im Inspector die Radien sehen kann
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, snipingRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nearShootingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
