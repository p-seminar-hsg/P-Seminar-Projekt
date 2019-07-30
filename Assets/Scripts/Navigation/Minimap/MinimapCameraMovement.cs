/*Ersteller: Rene Jokiel
    Zuletzt geändert am: 24.07.2019
    Funktion: Dieses Script soll eine Kamera ermöglichen, den Player zu verfolgen, ohne die z-Koordinate ändern zu müssen.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraMovement : MonoBehaviour
{
    public Transform player;    //Der Spieler (Gangolf?)

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.z = transform.position.z;   //Dadurch wir die Z.Koordinate erhalten.
        transform.position = newPosition;
    }
}
