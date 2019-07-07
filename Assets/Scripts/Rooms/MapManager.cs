
/*Ersteller: Benedikt Wille und Luca Kellermann 
    Zuletzt geändert am: 26.05.2019
    Funktion: Dieses Script ist dafür verantwortlich, Räume zu laden.
                Es muss zum GameManager hinzugefügt werden.*/

using System.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject currentRoom;

    //die drei letzten Räume speichern (previousRoom1 ist dabei der neuste)
    private int previousRoom1;
    private int previousRoom2;
    private int previousRoom3;

    //zusätlich einen bool-Wert speichern, Überprüfung später schneller
    private bool previous;
    
    //zu diesem Array lassen sich händisch Room-Prefabs im Editor hinzufügen
    public GameObject [] rooms;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //Player finden
        player = GameObject.FindGameObjectWithTag("Player");
        //previousRooms mit negativen Zahlen initialisieren, damit die erste zufällig ausgewählte Raum
        //in keinem Fall einem der previousRooms entspricht (zu Beginn gibt es keinen vorherigen Raum)
        previousRoom1 = -1;
        previousRoom2 = -1;
        previousRoom3 = -1;
        previous = false;

        //den ersten Raum laden
        LoadNewRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNewRoom()
    {
        StartCoroutine(FadeToNewRoom());
    }

    IEnumerator FadeToNewRoom()
    {
        //funktioniert schneller als: previousRoom == -1
        if(previous) //nur false beim Laden der Scene => nur reinfaden
        {
        GetComponent<RoomFader>().FadeFromRoom();
        
        //warten, bis  FadeFromRoom() fertig ist
        yield return new WaitForSeconds(0.5f);
        }
        
        Destroy(currentRoom);
        int randomIndex;
        
        //do-while Schleife, damit sichergestellt ist, dass mindestens
        //eine zufällige Zahl erzeugt wird
        do
        {
            randomIndex = Random.Range(0, rooms.Length);
        } while(randomIndex == previousRoom1 || randomIndex == previousRoom2 || randomIndex == previousRoom3); //neuer Raum soll ein anderer als die vorherigen sein
        
        //den neuen Raum aus den rooms-Array nehmen und instanziieren
        GameObject newRoom = rooms[randomIndex];
        currentRoom = Instantiate(newRoom, transform.position, Quaternion.identity);

        //vorherige Räume verändern
        previousRoom3 = previousRoom2;
        previousRoom2 = previousRoom1;
        previousRoom1 = randomIndex;
        previous = true;

        //den Player an den Spawnpoint des neuen Raums setzen
        player.transform.position = currentRoom.GetComponent<Room>().playerSpawn.position;


        GetComponent<RoomFader>().FadeToRoom();
    }
}
