using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille (14.07.2019)
/// Dieses Script ist für die Funktionalität des Health Pickups zuständig
/// </summary>
public class Heart : MonoBehaviour
{
    public float healValue;
    [Tooltip("Wie weit das Item nach dem Spawn vom Spieler weggeschleudert wird")]
    public float flightLength;
    public GameObject particleSys;

    //private bool canHeal;

    private Rigidbody2D rb;

    private void Start()
    {
        // Init
        rb = GetComponent<Rigidbody2D>();
        //canHeal = false;

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

        //canHeal = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (canHeal)
        //{
            GameObject otherGO = other.gameObject;
            if (otherGO.CompareTag("Player"))
            {
                GameObject.Instantiate(particleSys, transform.position, Quaternion.identity);
                otherGO.GetComponent<Player_Main>().heal(healValue);
                Destroy(this.gameObject);
            }
        //}
    }
}

