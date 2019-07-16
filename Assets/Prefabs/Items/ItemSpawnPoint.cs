using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel
/// Zuletzt geändert am: 16.07.2019
/// 
/// Dieses Script dient zum Spawnen von GameObjects. V.a. Items. Dieses Script kann gelöscht werden, nachdem die Raum - Item Beziehung geregelt wurde
/// </summary>

public class ItemSpawnPoint : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 position;
    public bool roomIsActiv;

    // Use this for initialization
    void Start()
    {

        position = GetPosition();
    }

    // Update is called once per frame
    void Update()
    {

        if (roomIsActiv == true)     //Wenn der Raum betreten wird bzw. geladen ist...
        {
            if (objectToSpawn != null)  //... und der Spawner ein Prefab ausgewählt hat...
            {
                GameObject object_spawn = (GameObject)Instantiate(objectToSpawn, position, Quaternion.identity);   //... wird das ausgewählte Prefab einmal gespawnt...
                Destroy(gameObject);    //... und der Spawner wird zerstört, weil wir nur ein GameObject spawnen möchten.
                return;     //Das return erfüllt keine "richtige" Funktion. Es wird nur Zeit geschindet, da Destroy manchmal nicht unbedingt zeitlich geschieht, sondern manchmal zu früh.
            }
        }
    }

    Vector3 GetPosition()       // Einziger Sinn dahinter ist, dass ich die Z - Koord. auf 0 setzen will, da ich nicht wussste, wie die Raumstruktur dimensional umsetzen werden.
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y);
        return this.transform.position;
    }
}
