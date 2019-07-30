/*Ersteller: Rene Jokiel
    Zuletzt geändert am: 24.07.2019
    Funktion: Dieses Script legt die Z-Koordinate der Kamera der Minimap so fest, dass sie der
              Formel dafür entspricht.*/

using UnityEngine;
using UnityEngine.Tilemaps;

public class MinimapDistance : MonoBehaviour
{
    public MapManager mapManager;
    public Camera cam;      //Kamera der Minimap

    /// <summary>
    /// Berechnet die z-Koordinate der Minimap Kamera
    /// </summary>

    public void CalculateDistance()
    {
        Tilemap ground_Tilemap = MapManager.instance.currentRoomScript.groundTilemap;    // Zugriff auf die Tilemaps Ground und Collision verschaffen
        Tilemap collision_Tilemap = MapManager.instance.currentRoomScript.colliderTilemap;

        BoundsInt bounds = ground_Tilemap.cellBounds;
        TileBase[] groundTiles = ground_Tilemap.GetTilesBlock(bounds);  //Ein Array anlegen, dass alle Tiles von der Ground Tilemap speichert

        BoundsInt bounds2 = collision_Tilemap.cellBounds;
        TileBase[] collTiles = collision_Tilemap.GetTilesBlock(bounds2);    //Ein Array anlegen, dass alle Tiles von der Collision Tilemap speichert

        float averageLenght = Mathf.Sqrt(groundTiles.Length + collTiles.Length);    //Die Wurzel aus der Anzahl aller Tiles (vgl. Formel Rechtecksfläche (s. FormelBreaker))
        Debug.Log("Debug von Rene:" + averageLenght);  // Debug als Check des Zahl

        Vector3 newPosition = cam.transform.position;
        newPosition.z = averageLenght * 0.5f * -1;  // Z-Koordinate = Die Hälfte der Länge einer Tilemap Seite, wenn alles gleich lang wäre * -1
        cam.transform.position = newPosition;




    }







}
