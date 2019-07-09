// Author: Rene Jokiel.     Last Update: 07.06.2019

using UnityEngine;

public class ShootingEnemy : Enemy
{
    private Transform playerTransform;
    public GameObject deatheffect;
    public GameObject projectilePrefab;     // Art des Geschütz
    public float attackCooldownPattern;
    public float chaseLimit;
   

    private void Awake()
    {
        // Das Ziel wird gleich dem Spieler gleichgesetzt, da es eh nur ein Ziel für die Gegner geben wird.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;    
        attackCooldown = attackCooldownPattern;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (playerTransform != null)    // Damit nach dem Ableben des Spielers nichts mehr passiert. Eig. irrelevant
        {
            // Dadurch wird der Cooldown so lange runtergesetzt, bis wieder Schaden genommen werden kann.
            if (takeDamageCooldown > 0)
            {
                takeDamageCooldown -= Time.deltaTime;
            }

            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    public override void TakeHit(Vector2 knockbackDirection, float strength)
    {
        // Kein Hit möglich, wenn takeDamageCooldown noch nicht abgelaufen
        if (takeDamageCooldown > 0)
            return;

        healthPoints -= strength;
        StartCoroutine(KnockbackCo(knockbackDirection));

        if (healthPoints <= 0)  //Zu wenig Health = Tod
            Die();
    }


    private void Die()
    {
        MapManager mapManagerInstance = MapManager.instance;
        mapManagerInstance.currentRoomScript.ReduceEnemiesAlive();
        mapManagerInstance.CheckForAllEnemiesDied();
        GameManager.AddToScore(10);

        GameObject effect = (GameObject)Instantiate(deatheffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(effect, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Main player = other.GetComponent<Player_Main>();
            player.takeHit(this.gameObject);    //, player.transform.position - transform.position, attackKnockback, falls man noch irgendwie Knockback haben möchte
        }
    }


    private void FixedUpdate()
    {
        // Wenn der Player in der Range ist ... 
        if (Vector3.SqrMagnitude(playerTransform.position - transform.position) <= Mathf.Pow(range, 2))     // Wenn du im Schussradius bist...
        {
            if (Vector3.SqrMagnitude(playerTransform.position - transform.position) > Mathf.Pow(chaseLimit, 2))     // ... aber nicht nicht im Limit...
            {
                if (attackCooldown <= 0)
                {
                    Shoot();        //... schießt er auf dich
                }
                return;
            }
            if (Vector3.SqrMagnitude(playerTransform.position - transform.position) < Mathf.Pow(chaseLimit, 2))
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);     // Bist du drin, geht er in den Nahkampf
            }

         transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }

    public void Shoot()
    {
        GameObject projGO = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);      //Geschütz wird "gespawnt"
        Projectile projectile = projGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Chase(playerTransform);
        }

        attackCooldown = attackCooldownPattern;
    }

    private void OnDrawGizmosSelected()     // Nur im Editor relevant, nicht im Spiel
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseLimit);
        Gizmos.DrawWireSphere(transform.position, range);
    }


}
