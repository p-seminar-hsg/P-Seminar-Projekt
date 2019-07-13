using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private GameObject player;

    private void OnStart()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy gegner = other.GetComponentInParent<Enemy>();
            gegner.TakeHit(other.transform.position-player.transform.position, player.GetComponent<Player_Main>().strength);
            
        }
    }

}
