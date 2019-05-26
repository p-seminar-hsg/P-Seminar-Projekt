
/*Ersteller: Benedikt Wille und Luca Kellermann 
    Zuletzt geändert am: 26.05.2019
    Funktion: Dieses Script ist die Grundlage aller Räume.*/

using UnityEngine;

public class Room : MonoBehaviour
{
    //die Positionen des PlayerSpawns und Teleporters speichern
    public Transform playerSpawn;
    public Teleporter teleporter;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Teleporter aktivieren/deaktivieren (z.B. nachdem alle Gegner besiegt wurden)
    public void SetTeleporterActive(bool b)
    {
        teleporter.gameObject.SetActive(b);
    }
}
