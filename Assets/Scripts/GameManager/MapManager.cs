using System.Collections;
using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille und Luca Kellermann <br/>
/// Zuletzt geändert am: 11.01.2020 <br/>
/// Dieses Script ist dafür verantwortlich, Räume zu laden. Es muss zum GameManager hinzugefügt werden.
/// </summary>
public class MapManager : MonoBehaviour
{
    #region Variablen
    //Es gibt genau eine Instanz des MapManager (Singleton pattern)
    public static MapManager instance;

    public GameObject currentRoom;
    public Room currentRoomScript;

    [Header("Scaling")]
    public int bossRoomAfterXScore;
    public int bossRoomAfterXRooms;

    // Der Score und die Anzahl der bisher geclearten Räume beim letzen BossRoom.
    private int lastBossRoomScore = 0;
    private int lastBossRoomRoomsCleared = 0;

    //die drei letzten Räume speichern (previousRoom1 ist dabei der neueste)
    private int previousRoom1;
    private int previousRoom2;
    private int previousRoom3;

    //zusätlich einen bool-Wert speichern, Überprüfung später schneller
    private bool previous;

    private bool currentRoomIsBossRoom;

    // Zu diesen Arrays lassen sich händisch Room-Prefabs im Editor hinzufügen
    public GameObject[] rooms;
    public GameObject[] bossRooms;

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
            return;
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

    private void Update()
    {
        /*
        // Zum testen
        if (Input.GetKeyDown(KeyCode.C))
            currentRoomScript.ClearRoom();
        else if (Input.GetKeyDown(KeyCode.T))
        {
            currentRoomScript.ClearRoom();
            GameManager.AddToScore(100);
            LoadNewRoom();
        }
        */
    }

    /// <summary>
    /// Startet die Coroutine zum Laden des nächsten Raums.
    /// </summary>
    public void LoadNewRoom()
    {
        Debug.Log("Debug von Benedikt: Score = " + GameManager.GetScore() +
                  " / RoomsCleared = " + GameManager.instance.roomsCleared);

        GameManager.instance.roomsCleared++;

        GameManager.PlaySound("Teleporter");
        StartCoroutine(FadeToNewRoom());

        // Lässt die Kamera der Minimap der Größe der Tilemap entsprechend raus oder reinzoomen (Rene Jokiel)
        minimapDistance.CalculateDistance();
    }

    /// <summary>
    /// Lädt den nächsten Raum. <br/>
    /// Dazu werden folgende Schritte ausgeführt: <br/>
    /// 1. Fade-Effekt aus altem Raum heraus starten. <br/>
    /// 2. Alle übriggebliebenen Items und Gegner und den alten Raum entfernen. <br/>
    /// 3. Entscheiden, welcher Raum geladen werden soll (Testraum, Bossraum oder zufälliger Raum?) und diesen dann laden. <br/>
    /// 4. Fade-Effekt in neuen Raum hinein starten.
    /// </summary>
    private IEnumerator FadeToNewRoom()
    {
        if (previous) // Nur false beim Laden der Scene => nur reinfaden
        {
            GetComponent<RoomFader>().FadeFromRoom();

            // Warten, bis FadeFromRoom() fertig ist
            yield return new WaitForSeconds(0.5f);
        }

        ClearItemsAndEnemies();

        Destroy(currentRoom);

        // Überprüfen, ob ein bestimmter Raum getestet werden soll
        if (GameManager.instance.testRoomIndex < 0)
        {
            // Überprüfen ob ein Bossraum kommen soll
            if (!GetIfBossRoom())
            {
                currentRoomIsBossRoom = false;
                InstantiateRandomRoom();
            }
            else  // Es soll ein Bossraum kommen
            {
                currentRoomIsBossRoom = true;
                InstantiateRandomBossRoom();
            }
        }
        else // Es wurde ein zu testender Raum gesetzt
        {
            currentRoomIsBossRoom = false;
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
        CheckForAllEnemiesDied();
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

    /// <summary>
    /// Überprüft, ob gerade ein BossRoom vorliegt.
    /// </summary>
    /// <returns>True, wenn der aktuelle Room ein BossRoom ist, sonst false.</returns>
    public bool CurrentRoomIsBossRoom()
    {
        return currentRoomIsBossRoom;
    }

    /// <summary>
    /// Entfernt alle Items und Gegner aus der Scene.
    /// </summary>
    private void ClearItemsAndEnemies()
    {
        //Alle aus welchem Grund auch immer noch existierenden Enemies löschen um Fehler im nächsten Room zu vermeiden
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);

        // Alle nicht gesammelten Items löschen
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
            Destroy(item);
    }

    /// <summary>
    /// Bestimmt, ob als nächstes ein BossRoom kommen soll,
    /// d.h. ob seit dem letzen BossRoom genug Score dazu kam
    /// UND ob seit dem letzen BossRoom genug Räume vergangen sind.
    /// </summary>
    /// <returns>True, wenn als nächstes ein BossRoom kommen soll, sonst false.</returns>
    private bool GetIfBossRoom()
    {
        /* Seit dem letzen BossRoom kam genug Score dazu
         * UND seit dem letzen BossRoom sind genug Räume vergangen */
        return (GameManager.GetScore() - lastBossRoomScore > bossRoomAfterXScore)
                && (GameManager.instance.roomsCleared - lastBossRoomRoomsCleared > bossRoomAfterXRooms);
    }

    /// <summary>
    /// Lädt einen zufälligen Bossraum.
    /// </summary>
    private void InstantiateRandomBossRoom()
    {
        GameObject bossRoom = Utility.ChooseRandom<GameObject>(bossRooms);

        currentRoom = Instantiate(bossRoom, transform.position, Quaternion.identity);

        lastBossRoomScore = GameManager.GetScore();
        lastBossRoomRoomsCleared = GameManager.instance.roomsCleared;

        // Vorherige Räume verändern
        previousRoom3 = previousRoom2;
        previousRoom2 = previousRoom1;
        previousRoom1 = -1; // Der RandomIndex in InstantiateRandomRoom ist immer > -1
        previous = true;
    }

    /// <summary>
    /// Lädt einen zufälligen Raum (keinen der letzten 3).
    /// </summary>
    private void InstantiateRandomRoom()
    {
        int randomIndex;

        //do-while Schleife, damit sichergestellt ist, dass mindestens eine zufällige Zahl erzeugt wird
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
