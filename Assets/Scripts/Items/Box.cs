using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 29.07.2019 <br/>
/// Script für eine Box, die bei Zerstörung ein Item fallen lässt.
/// </summary>
public class Box : MonoBehaviour
{
    public Item itemToSpawn;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("AttackHitbox"))
        {
            GameManager.PlaySound("HitPowerUpBox");
            Item object_spawn = (Item)Instantiate(itemToSpawn, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
