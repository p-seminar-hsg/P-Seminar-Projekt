using System.Collections;
using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille (27.07.2019)
/// Dieses Enum beeinhaltet alle verfügbaren Item-Effekte - Teil der Item-API
/// </summary>
public enum ItemEffect
{
    HEAL, SPEED, STRENGTH
}

/// <summary>
/// Ersteller: Benedikt Wille (27.07.2019)
/// Zuletzt geändert am 11.11.2019
/// Mitarbeiter: Luca Kellermann (Sound bei Effektvergabe)
/// Dieses Script ist für die Funktionalität der Items zuständig
/// und dient als generische Vorlage für alle Items - Teil der Item-API
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    public ItemEffect effect;
    [Tooltip("Der Wert des Effekts (im Fall von Speed und Protection die Länge des Effekts)")]
    public float value;

    [Space()]
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
        player.GetComponent<Player_Main>().UseItem(effect, value);
        playPowerUpSound();
        Destroy(this.gameObject);
    }

    private void playPowerUpSound()
    {
        int randomNumber = Random.Range(1, 5);
        GameManager.PlaySound("PowerUp" + randomNumber);
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
