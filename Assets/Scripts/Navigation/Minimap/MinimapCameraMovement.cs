using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 24.07.2019 <br/>
/// Dieses Script soll eine Kamera ermöglichen, den Player zu verfolgen, ohne die z-Koordinate ändern zu müssen.
/// </summary>
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
