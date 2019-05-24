using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private MapManager mapManager;

    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        mapManager.LoadNewRoom();
    }
}
