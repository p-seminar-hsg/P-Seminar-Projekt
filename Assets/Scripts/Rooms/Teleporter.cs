using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille und Luca Kellermann <br/>
/// Zuletzt geändert am: 25.05.2019 <br/>
/// Dieses Script ist für die Funktionalität der Spieler-Teleporter verantwortlich.
/// </summary>
public class Teleporter : MonoBehaviour
{
    // Referenz zum MapManager-Script des GameManagers
    private MapManager mapManager;

    // Start is called before the first frame update
    void Start()
    {
        //das MapManager-Script des GameManagers suchen
        mapManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nur den Player teleportieren
        if (other.tag.Equals("Player") && !other.isTrigger)
        {
            // Score erhöhen
            GameManager.AddToScore(GameManager.instance.scorePerRoom);
            // Neuen Raum laden und Player dorthin teleportieren
            mapManager.LoadNewRoom();
        }
    }
}
