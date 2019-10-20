
/*Ersteller: Benedikt Wille und Luca Kellermann 
  Zuletzt geändert am: 20.10.2019
  Funktion: Dieses Script ist dafür verantwortlich, Räume zu laden.
            Es muss zum GameManager hinzugefügt werden.*/

using System.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Variablen
    //Es gibt genau eine Instanz des MapManager (Singleton pattern)
    /// <summary>
    /// Die einzige Instanz des MapManager.
    /// </summary>
    public static MapManager instance;

    public GameObject currentRoom;
    public Room currentRoomScript;

    //die drei letzten Räume speichern (previousRoom1 ist dabei der neuste)
    private int previousRoom1;
    private int previousRoom2;
    private int previousRoom3;

    //zusätlich einen bool-Wert speichern, Überprüfung später schneller
    private bool previous;

    //zu diesem Array lassen sich händisch Room-Prefabs im Editor hinzufügen
    public GameObject[] rooms;
    private GameObject player;


    public MinimapDistance minimapDistance; //Von Rene Jokiel
    #endregion


    void Awake()
    {

        if (instance == null)
        {
            //Wenn es noch keinen MapManager gibt, den gerade erzeugten als die einzige Instanz festlegen
            instance = this;
        }
        else
        {
            //Sonst den gerade erzeugten MapManager direkt wieder löschen
            Destroy(gameObject);
        }

        //Player finden
        player = GameObject.FindGameObjectWithTag("Player");
        //previousRooms mit negativen Zahlen initialisieren, damit die erste zufällig ausgewählte Raum
        //in keinem Fall einem der previousRooms entspricht (zu Beginn gibt es keinen vorherigen Raum)
        previousRoom1 = -1;
        previousRoom2 = -1;
        previousRoom3 = -1;
        previous = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Den ersten Raum laden
        LoadNewRoom();
    }

    public void LoadNewRoom()
    {
        StartCoroutine(FadeToNewRoom());

        // Lässt die Kamera der Minimap der Größe der Tilemap entsprechend raus oder reinzoomen (Rene Jokiel)
        minimapDistance.CalculateDistance(); 
    }

    /// <summary>
    /// Aktiviert den Teleporter des aktuellen Rooms, wenn alle Gegner besiegt wurden.
    /// </summary>
    public void CheckForAllEnemiesDied()
    {
        if (currentRoomScript.GetEnemiesAlive() <= 0)
        {
            currentRoomScript.SetTeleporterActive(true);
        }
    }

    private IEnumerator FadeToNewRoom()
    {
        // Funktioniert schneller als: previousRoom == -1
        if (previous) // Nur false beim Laden der Scene => nur reinfaden
        {
            GetComponent<RoomFader>().FadeFromRoom();

            // Warten, bis FadeFromRoom() fertig ist
            yield return new WaitForSeconds(0.5f);
        }

        // Alle nicht gesammelten Items löschen
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
            Destroy(item);

        Destroy(currentRoom);

        // Überprüfen, ob ein bestimmter Raum getestet werden soll
        if (GameManager.instance.testRoomIndex < 0)
        {

            InstantiateRandomRoom();

        }
        else // Es wurde ein zu testender Raum gesetzt
        {
            currentRoom = Instantiate(rooms[GameManager.instance.testRoomIndex], transform.position, Quaternion.identity);
            previous = true;
        }

        currentRoomScript = currentRoom.GetComponent<Room>();

        // Den Player an den Spawnpoint des neuen Raums setzen
        player.transform.position = currentRoomScript.playerSpawn.position;

        GetComponent<RoomFader>().FadeToRoom();

        // Kurz warten, damit der Teleporter beim Erstellen des Rooms erst deaktiviert werden kann
        yield return new WaitForSeconds(0.01f);

        // Gibt es überhaupt Gegner?
        if (currentRoomScript.GetEnemiesAlive() <= 0)
        {
            currentRoomScript.SetTeleporterActive(true);
        }
    }

    private void InstantiateRandomRoom()
    {
        int randomIndex;

        //do-while Schleife, damit sichergestellt ist, dass mindestens
        //eine zufällige Zahl erzeugt wird
        do
        {
            randomIndex = Random.Range(0, rooms.Length);
        } while (randomIndex == previousRoom1 || randomIndex == previousRoom2 || randomIndex == previousRoom3); //neuer Raum soll ein anderer als die vorherigen sein

        // Den neuen Raum aus dem rooms-Array nehmen und instanziieren
        GameObject newRoom = rooms[randomIndex];
        currentRoom = Instantiate(newRoom, transform.position, Quaternion.identity);

        // Vorherige Räume verändern
        previousRoom3 = previousRoom2;
        previousRoom2 = previousRoom1;
        previousRoom1 = randomIndex;
        previous = true;
    }
}
