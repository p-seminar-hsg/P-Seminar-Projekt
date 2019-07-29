/// <summary>
/// Ersteller: Rene Jokiel
/// Zuletzt geändert am 29.07.2019
/// Script für eine Box, die bei Zerstörung ein Item fallen lässt
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Item itemToSpawn;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("AttackHitbox"))
        {
            Item object_spawn = (Item)Instantiate(itemToSpawn, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
