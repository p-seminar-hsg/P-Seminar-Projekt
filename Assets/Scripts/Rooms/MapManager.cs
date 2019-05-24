using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GameObject currentRoom;

    private RoomData roomData;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        roomData = GetComponent<RoomData>();
        player = GameObject.FindGameObjectWithTag("Player");

        LoadNewRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNewRoom()
    {
        Destroy(currentRoom);
        GameObject newRoom = roomData.PickRandomRoom();
        currentRoom = Instantiate(newRoom, transform.position, Quaternion.identity);
        player.transform.position = currentRoom.GetComponent<Room>().playerSpawn.position;
    }
}

