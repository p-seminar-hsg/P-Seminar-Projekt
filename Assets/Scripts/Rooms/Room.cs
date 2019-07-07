﻿
/// <summary>
/// Ersteller: Benedikt Wille und Luca Kellermann ;
/// Zuletzt geändert am: 7.07.2019
/// 
/// Dieses Script ist die Grundlage aller Räume.
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Transform playerSpawn;
    public Enemy[] possibleEnemies;
    public Tilemap groundTilemap;
    public AStarNode[,] nodes;

    private Teleporter teleporter;
    private SpawnPoint[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        // Init
        spawnPoints = GetComponentsInChildren<SpawnPoint>(true); // including inactive
        teleporter = GetComponentInChildren<Teleporter>();

        // Der Teleporter ist am Anfang immer deaktiviert
        SetTeleporterActive(false);

        // Jeder SpawnPoint bekommt einen zufälligen Gegner aus possibleEnemies zugewiesen
        foreach (SpawnPoint sp in spawnPoints)
        {
            sp.objectToSpawn = Utility.ChooseRandom(possibleEnemies).gameObject;
        }

        // Aktiviert eine bestimmte Anzahl an SpawnPoints
        int noOfActiveSpawnpoints = CalculateNumberOfActiveSpawnpoints();
        Debug.Log("Debug von Benedikt - Aktive Spawnpoints: " + noOfActiveSpawnpoints);
        List<SpawnPoint> inactiveSpawnpoints = new List<SpawnPoint>(spawnPoints);
        for (int i = 0; i < noOfActiveSpawnpoints; i++)
        {
            int rand = Random.Range(0, inactiveSpawnpoints.Count);
            inactiveSpawnpoints[rand].gameObject.SetActive(true);
            inactiveSpawnpoints.RemoveAt(rand);
        }


        // Init nodes

        //bounds auf ausgefüllten Bereich der Tilemap beschränken
        groundTilemap.CompressBounds();
        BoundsInt bounds = groundTilemap.cellBounds;

        //nodes-Array initialisieren
        nodes = new AStarNode[bounds.size.x, bounds.size.y];

        //Array mit allen Tiles
        TileBase[] allTiles = groundTilemap.GetTilesBlock(bounds);

        //alle möglichen Positionen für Tiles durchgehen
        foreach(Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin){

            //aktuelle Tile aus allTiles bekommen
            TileBase tile = allTiles[(pos.x - bounds.xMin) + (pos.y - bounds.yMin) * bounds.size.x];

            //gibt es die Tile?
            if(tile != null){

                //AStarNode mit Koordinaten entsprechend der Tile zu nodes hinzufügen
                nodes[(pos.x-bounds.xMin), (pos.y-bounds.yMin)] = new AStarNode(pos.x, pos.y, pos.x-bounds.xMin, pos.y-bounds.yMin);
            }
            
        }

    }


    /// <summary>Sucht die AStarNode mit den angegebenen Koordinaten</summary>
    /// <param name="posX">Die x-Koordinate.</param>
    /// <param name="posY">Die y-Koordinate.</param>
    /// <returns>Die gesuchte AStarNode oder null falls sie nicht gefunden wurde.</returns>
    public AStarNode GetNodeWith(int posX, int posY){
        foreach(AStarNode node in nodes){
            if(node != null && node.posX == posX && node.posY == posY){
                return node;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Teil des Scaling-Systems
    /// Berechnet die Anzahl an aktiven Spawnpoints basierend auf dem aktuellen Highscore
    /// </summary>
    /// <returns>Anzahl der aktiven Spawnpoints in diesem Raum</returns>
    private int CalculateNumberOfActiveSpawnpoints()
    {
        int highscore = GameManager.GetHighscore();
        int noOfSpawnpoints = spawnPoints.Length;
        int randomValue = Random.Range(0, 2);

        return (int) Mathf.CeilToInt((noOfSpawnpoints / 2) + (highscore / 200) + randomValue);
    }

    /// <summary>
    /// Teleporter aktivieren / deaktivieren (z.B. nachdem alle Gegner besiegt wurden)
    /// </summary>
    public void SetTeleporterActive(bool active)
    {
        teleporter.gameObject.SetActive(active);
    }
}
