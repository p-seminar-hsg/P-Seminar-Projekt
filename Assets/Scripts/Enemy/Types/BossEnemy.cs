using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 11.01.2020 <br/>
/// Mitarbeiter: Florian Müller-Martin (Animationen), Luca Kellermann (Sounds) <br/>
/// Dieses Script ist für das Verhalten des Bossgegners zuständig.
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
    public float meleeRadius;
    public float nearShootingRadius;
    public float snipingRadius;

    [Header("Cooldowns - Mehr Angriffe, mehr Cooldowns")]
    public float meleeCooldownPattern;
    public float nearShootingCooldownPattern;
    public float snipingCooldownPattern;

    public float meleeCooldown;
    private float nearShootingCooldown;
    private float snipingCooldown;

    [Header("Angriffsprefabs - GOs, die gebraucht werden")]
    public GameObject projectileFar;
    public GameObject projectileNear;
    public GameObject attackBoxMelee;

    [Header("Variablen für die Animation (Flomm)")]
    public float attackType;
    public bool isAttack = false;

    private bool onWall;


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

        meleeCooldown = meleeCooldownPattern;
        nearShootingCooldown = nearShootingCooldownPattern;
        snipingCooldown = snipingCooldownPattern;
    }

    void Die()
    {
        PlayDeathSound();

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

        currentHealthpoints -= damage;

        if (currentHealthpoints >= 0)
            PlayTakeDamageSound();


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
            if (meleeCooldown > 0)
            {
                meleeCooldown -= Time.deltaTime;
            }
            if (nearShootingCooldown > 0)
            {
                nearShootingCooldown -= Time.deltaTime;
            }
            if (snipingCooldown > 0)
            {
                snipingCooldown -= Time.deltaTime;
            }
            if (currentHealthpoints <= 0)
            {
                Die();
            }
        }
        healthBar.fillAmount = currentHealthpoints / healthPoints_max;
    }

    private void FixedUpdate()  //Je nach den Spähren, in denen sich der Player befindet, wird ein anderer Angriff ausgeführt
    {
        //Position wird zu Beginn gespeichert
        positionStartOfFrame = transform.position;

        if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(snipingRadius, 2))     // Wenn du im snipingRadius bist...
        {
            if (Vector3.SqrMagnitude(playerTransform.position - transform.position) > Mathf.Pow(nearShootingRadius, 2))     // ... aber nicht nicht im nearShootingRadius...
            {
                if (snipingCooldown <= 0)
                {
                    if (onWall == false)
                    {
                        ShootFar();     // ... wird geschossen
                        attackType = 1;  // ... und der Angriffstyp für die Animation aktualisiert (Flomm)
                        isAttack = true;
                    }

                }
            }

            if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(nearShootingRadius, 2))
            {
                if (Vector3.SqrMagnitude(playerTransform.position - transform.position) > Mathf.Pow(meleeRadius, 2))
                {
                    if (nearShootingCooldown <= 0)
                    {
                        if (onWall == false)
                        {
                            ShootMedium(Random.Range(1, 7));
                            attackType = 2;
                            isAttack = true;
                        }
                       
                    }
                }

                if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(meleeRadius, 2))
                {
                    if (meleeCooldown <= 0)
                    {
                        MeleeAttack();
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
        RefreshAnimator(false);
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

    private void MeleeAttack()
    {
        PlayJumpSound();
        attackBoxMelee.SetActive(true);
        meleeCooldown = meleeCooldownPattern * 1000000;     //Cooldown wird unglaublich hoch gesetzt, so dass der Angriff nur ein Mal ausgeführt werden kann
    }

    private void ShootMedium(int amount)
    {
        PlayBurstSound();
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
        PlayHomingSound();
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
                PlayMeleeSound();
                other.GetComponent<Player_Main>().takeHit(this.gameObject);
                //Debug.Log("Debug von Flo: " + this.name + " ist mit Player kollidiert");
                localAttackCooldown = attackCooldown;
            }
        }
        if(other.CompareTag("CollisionTilemap"))
        {
            onWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CollisionTilemap"))
        {
            onWall = false;
        }
    }

    private void OnDrawGizmosSelected() //Damit man im Inspector die Radien sehen kann
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, snipingRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nearShootingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleeRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }


    #region Soundmethods

    /// <summary>
    /// Einer von 3 möglichen Burst Sounds wird abgespielt.
    /// </summary>
    private void PlayBurstSound()
    {
        int randomNumber = Random.Range(1, 4);
        GameManager.PlaySound("BossBurst" + randomNumber);
    }

    /// <summary>
    /// Einer von 4 möglichen Death Sounds wird abgespielt.
    /// </summary>
    private void PlayDeathSound()
    {
        int randomNumber = Random.Range(1, 5);
        GameManager.PlaySound("BossDeath" + randomNumber);
    }

    /// <summary>
    /// Einer von 3 möglichen Homing Sounds wird abgespielt.
    /// </summary>
    private void PlayHomingSound()
    {
        int randomNumber = Random.Range(1, 4);
        GameManager.PlaySound("BossHoming" + randomNumber);
    }

    /// <summary>
    /// Einer von 3 möglichen Jump Sounds wird abgespielt.
    /// </summary>
    private void PlayJumpSound()
    {
        int randomNumber = Random.Range(1, 4);
        GameManager.PlaySound("BossJump" + randomNumber);
    }

    /// <summary>
    /// Einer von 3 möglichen Melee Sounds wird abgespielt.
    /// </summary>
    private void PlayMeleeSound()
    {
        if (Random.Range(0, 100) < 25)
        {
            int randomNumber = Random.Range(1, 4);
            GameManager.PlaySound("BossMelee" + randomNumber);
        }
    }

    /// <summary>
    /// Einer von 5 möglichen TakeDamage Sounds wird abgespielt.
    /// </summary>
    private void PlayTakeDamageSound()
    {
        if (Random.Range(0, 100) < 25)
        {
            int randomNumber = Random.Range(1, 6);
            GameManager.PlaySound("BossTakeDamage" + randomNumber);
        }
    }

    #endregion
}
