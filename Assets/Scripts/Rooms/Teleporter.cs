
/*Ersteller: Benedikt Wille und Luca Kellermann 
    Zuletzt geändert am: 25.05.2019
    Funktion: Dieses Script ist für die Funktionalität der Spieler-Teleporter verantwortlich.*/

using UnityEngine;

public class Teleporter : MonoBehaviour
{
    //Referenz zum MapManager-Script des GameManagers
    private MapManager mapManager;

    // Start is called before the first frame update
    void Start()
    {
        //das MapManager-Script des GameManagers suchen
        mapManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //nur den Player teleportieren
        if(other.tag.Equals("Player")){
            //neuen Raum laden und Player dorthin teleportieren
            mapManager.LoadNewRoom();
        }
    }
}
