using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille (14.07.2019)
/// Dieses Script ist für die Funktionalität des Health Pickups zuständig
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Heart : MonoBehaviour
{
    public float healValue;
    [Tooltip("Wie weit das Item nach dem Spawn vom Spieler weggeschleudert wird")]
    public float flightLength;
    public GameObject particleSys;

    private Rigidbody2D rb;
    private Collider2D coll;

    private void Start()
    {
        // Init
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        StartCoroutine(Fly());
    }

    /// <summary>
    /// Bewirkt, dass das Item (nach dem Spawn) ein kleines Stück vom
    /// Player wegfliegt, damit er es nicht gleich einsammelt
    /// </summary>
    private IEnumerator Fly()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 direction = transform.position - player.transform.position;
        rb.velocity = direction.normalized * 2;

        yield return new WaitForSeconds(flightLength);

        rb.velocity = Vector2.zero;
    }

    private void Effect(GameObject player)
    {
        GameObject.Instantiate(particleSys, transform.position, Quaternion.identity);
        //player.GetComponent<Player_Main>().heal(healValue);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.CompareTag("Player"))
        {
            Effect(otherGO);
        }
        else if (otherGO.CompareTag("CollisionTilemap"))
        {
            // Nichts
        }
        else
        {
            /* Wenn ein Gegner oder ein anderes Items das Item berührt, wird der Collider zum Trigger
             * Das Item kann also nicht einfach geschoben werden */
            coll.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Auch wenn der Collider ein Trigger ist, kann der Player das Item einsammeln
        if (collision.CompareTag("Player"))
        {
            Effect(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Hebt das zurücksetzen aus OnTriggerExit2D wieder auf, sofern noch etwas im Collider drin steht
        if (!coll.isTrigger && !collision.CompareTag("Player"))
            coll.isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        coll.isTrigger = false;
    }
}

