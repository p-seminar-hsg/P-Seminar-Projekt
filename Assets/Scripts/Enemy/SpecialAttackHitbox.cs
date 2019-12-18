using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 5.08.2019 <br/>
/// Dieses Script ermöglicht einem GameObject dem Player Schaden zuzufügen.
/// </summary>
public class SpecialAttackHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Main>().takeHit(this.gameObject);
            Destroy(this.gameObject);  //Script wird gelöscht, damit der Angriff nicht erneut ausgeführt wird
        }
    }
}
