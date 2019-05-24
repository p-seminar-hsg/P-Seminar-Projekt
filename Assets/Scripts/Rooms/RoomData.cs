using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public GameObject [] rooms;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject PickRandomRoom()
    {
        int randomIndex = Random.Range(0, rooms.Length);
        return rooms[randomIndex];
    }
}
