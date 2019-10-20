using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Ersteller: Benedikt Wille und Luca Kellermann
/// Zuletzt geändert am: 20.10.2019
/// Dieses Script ist die Grundlage aller Räume.
/// </summary>
public class Room : MonoBehaviour
{
    public Transform playerSpawn;
    [Header("Die möglichen Enemies, die im Raum spawnen können")]
    public Enemy[] possibleEnemies;
    [Space(4)]
    [Header("Scaling")]
    public int minimumActiveSpawnpoints = -1;
    [Space(4)]
    [Header("Tilemaps")]
    public Tilemap groundTilemap;
    public Tilemap colliderTilemap;
    [Space(4)]
    public AStarNode[,] nodes;
    public GameObject[] enemies;

    private Teleporter teleporter;
    private SpawnPoint[] spawnPoints;
    private int enemiesAlive;

    void Awake()
    {
        // Init
        spawnPoints = GetComponentsInChildren<SpawnPoint>(true); // including inactive
        teleporter = GetComponentInChildren<Teleporter>(true); // including inactive
    }

    // Start is called before the first frame update
    void Start()
    {
        // Der Teleporter ist am Anfang immer deaktiviert
        //SetTeleporterActive(false);

        // Jeder SpawnPoint bekommt einen zufälligen Gegner aus possibleEnemies zugewiesen
        foreach (SpawnPoint sp in spawnPoints)
        {
            sp.objectToSpawn = Utility.ChooseRandom(possibleEnemies).gameObject;
        }

        // Wenn minAcSPs. negativ ist, wird es auf die Hälfte aller SPs gesetzt, ansonsten alles gut :D
        minimumActiveSpawnpoints = minimumActiveSpawnpoints < 0 ? spawnPoints.Length / 2 : minimumActiveSpawnpoints;

        // Aktiviert eine bestimmte Anzahl an SpawnPoints
        int noOfActiveSpawnpoints = CalculateNumberOfActiveSpawnpoints();

        List<SpawnPoint> inactiveSpawnpoints = new List<SpawnPoint>(spawnPoints);

        Debug.Log("Debug von Benedikt - Aktive Spawnpoints: " + noOfActiveSpawnpoints);

        for (int i = 0; i < noOfActiveSpawnpoints; i++)
        {
            int rand = Random.Range(0, inactiveSpawnpoints.Count);
            inactiveSpawnpoints[rand].gameObject.SetActive(true);
            inactiveSpawnpoints.RemoveAt(rand);
        }

        enemiesAlive = noOfActiveSpawnpoints;

        // FindEnemies muss später ausgeführt werden, sonst werden die Gegner nicht gefunden
        StartCoroutine(FindEnemies());

        #region A*-Zeug (Luca Kellermann)
        // Init nodes

        //bounds auf ausgefüllten Bereich der Tilemap beschränken
        groundTilemap.CompressBounds();
        BoundsInt bounds = groundTilemap.cellBounds;

        //nodes-Array initialisieren
        nodes = new AStarNode[bounds.size.x, bounds.size.y];

        //Array mit allen Tiles
        TileBase[] allTiles = groundTilemap.GetTilesBlock(bounds);

        //alle möglichen Positionen für Tiles durchgehen
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin)
        {

            //aktuelle Tile aus allTiles bekommen
            TileBase tile = allTiles[(pos.x - bounds.xMin) + (pos.y - bounds.yMin) * bounds.size.x];

            //gibt es die Tile?
            if (tile != null)
            {

                //AStarNode mit Koordinaten entsprechend der Tile zu nodes hinzufügen
                nodes[(pos.x - bounds.xMin), (pos.y - bounds.yMin)] = new AStarNode(pos.x, pos.y, pos.x - bounds.xMin, pos.y - bounds.yMin);
            }

        }
        #endregion

    }


    private IEnumerator FindEnemies()
    {
        yield return new WaitForSeconds(0.005f);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    /// <summary>Sucht die AStarNode mit den angegebenen Koordinaten</summary>
    /// <param name="posX">Die x-Koordinate.</param>
    /// <param name="posY">Die y-Koordinate.</param>
    /// <returns>Die gesuchte AStarNode oder null falls sie nicht gefunden wurde.</returns>
    public AStarNode GetNodeWith(int posX, int posY)
    {
        foreach (AStarNode node in nodes)
        {
            if (node != null && node.posX == posX && node.posY == posY)
            {
                return node;
            }
        }
        return null;
    }


    /// <summary>
    /// Teil des Scaling-Systems (Benedikt Wille)
    /// Berechnet die Anzahl an aktiven Spawnpoints basierend auf dem aktuellen Highscore
    /// </summary>
    /// <returns>Anzahl der aktiven Spawnpoints in diesem Raum</returns>
    private int CalculateNumberOfActiveSpawnpoints()
    {
        int score = GameManager.GetScore();
        int randomValue = Random.Range(0, 2);

        int noOfActiveSpawnpoints = (int)Mathf.CeilToInt((minimumActiveSpawnpoints) + (score / 200) + randomValue);

        // Wenn die berechnete Zahl größer als die maximale Anzahl an SPs ist, wird einfach das Maximum zurückgegeben
        return noOfActiveSpawnpoints >= spawnPoints.Length ? spawnPoints.Length : noOfActiveSpawnpoints;
    }

    /// <summary>
    /// Teleporter aktivieren / deaktivieren (z.B. nachdem alle Gegner besiegt wurden)
    /// </summary>
    public void SetTeleporterActive(bool active)
    {
        teleporter.gameObject.SetActive(active);
    }

    /// <summary>
    /// Reduziert die Nummer von lebenden Gegnern um 1.
    /// </summary>
    public void ReduceEnemiesAlive()
    {
        enemiesAlive--;
        //Find Enemies muss später ausgeführt werden, sonst werden die Gegner nicht gefunden
        StartCoroutine(FindEnemies());
    }

    /// <summary>
    /// Gibt die Anzahl der lebenden Gegner zurück.
    /// </summary>
    /// <returns>Die Anzahl der lebenden Gegner.</returns>
    public int GetEnemiesAlive()
    {
        return enemiesAlive;
    }

    /// <summary>
    /// Gibt die Position des Teleporters zurück.
    /// </summary>
    /// <returns>Die Position des Teleporters.</returns>
    public Vector3 GetTeleporterPosition()
    {
        return teleporter.transform.position;
    }
}
