using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Ersteller: Florian Müller-Martin und Florian Ganser <br/>
/// Zuletzt geändert am: 7.12.2019 <br/>
/// Klasse zum Abspielen der Step-Sounds je nach Untergrund.
/// </summary>
public class Player_StepSounds : MonoBehaviour
{
    #region Variablen
    [Header("Internal stuff to find the current Tile")]
    private Tilemap groundTilemap;
    private Tile currentTile;

    [Header("Please insert every Ground Tile here, depending on its hardness (earth, gras: soft; stone, tiles: hard)")]
    public Tile[] softTiles;
    public Tile[] hardTiles;

    //Variablen, um die Sounds richtig zu starten und zu stoppen
    private Tile lastTile;
    private bool isPlaying;

    #endregion
    void FixedUpdate()
    {
        //Aktuelle Tilemap finden
        GameObject currentRoomGO;
        Room currentRoom;

        currentRoomGO = GameObject.FindGameObjectWithTag("Room");
        currentRoom = currentRoomGO.GetComponent(typeof(Room)) as Room;
        groundTilemap = currentRoom.groundTilemap;


        //Das Tile unter den Füßen des Players finden
        Vector3Int playerPosition = new Vector3Int((int)gameObject.transform.position.x, (int)(gameObject.transform.position.y - 0.76f), 0);  //-0.76 bei der y-Koordinate, weil der Anker des Players in seiner Mitte liegt, man aber das Tile unter seinen Füßen benötigt.
        lastTile = currentTile;
        currentTile = (Tile)groundTilemap.GetTile(playerPosition);
        //Debug.Log(currentTile.sprite.name);

        playSound();

    }

    /// <summary>
    /// Diese Methode startet und stoppt die entsprechenden Laufsounds.
    /// </summary>
    private void playSound()
    {
        //Wenn der Spieler sich bewegt hat und entweder gerade kein Sound läuft oder der Tiletyp sich geändert hat, wird der entsprechende Sound abgespielt
        if ((!isPlaying || getTypeOfTile(lastTile) != getTypeOfTile(currentTile)) && GameObject.Find("Player").GetComponent<Player_Movement>().actualMoveX != 0 && GameObject.Find("Player").GetComponent<Player_Movement>().actualMoveY != 0)
        {
            if (getTypeOfTile(currentTile) == "soft")
            {
                GameManager.StopSound("Steps2");
                //Debug.Log("Stopped Steps 2");
                GameManager.PlaySound("Steps1");
                //Debug.Log("Started Steps 1");
                isPlaying = true;

            }
            else if (getTypeOfTile(currentTile) == "hard")
            {
                GameManager.StopSound("Steps1");
                //Debug.Log("Stopped Steps 1");
                GameManager.PlaySound("Steps2");
                //Debug.Log("Started Steps 2");
                isPlaying = true;
            }
            else if (getTypeOfTile(currentTile) == null)
            {
                GameManager.StopSound("Steps1");
                //Debug.Log("Stopped Steps 1");
                GameManager.PlaySound("Steps2");
                //Debug.Log("Started Steps 2");
                isPlaying = true;
            }
        }
        //Wenn der Spieler sich nicht bewegt hat werden alle Sounds gestoppt
        else if (GameObject.Find("Player").GetComponent<Player_Movement>().actualMoveX == 0 && GameObject.Find("Player").GetComponent<Player_Movement>().actualMoveY == 0)
        {
            GameManager.StopSound("Steps1");
            //Debug.Log("Stopped Steps 1");
            GameManager.StopSound("Steps2");
            //Debug.Log("Stopped Steps 2");
            isPlaying = false;
        }
    }

    /// <summary>
    /// Gibt den Typ einer Tiles zurück (davor in Arrays zugeordnet).
    /// </summary>
    /// <param name="tile">Die Tile, deren Typ zurückgegeben werden soll.</param>

    private string getTypeOfTile(Tile tile)
    {
        try
        {
            foreach (Tile Tile in softTiles)
            {
                if (Tile.sprite.name == tile.sprite.name)
                {
                    return "soft";
                }
            }

            foreach (Tile Tile in hardTiles)
            {
                if (Tile.sprite.name == tile.sprite.name)
                {
                    return "hard";
                }
            }
        }
        catch
        {
            return null;
        }
        return null;
    }
}
