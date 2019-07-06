/*Ersteller: Benedikt Wille und Luca Kellermann 
    Zuletzt geändert am: 26.05.2019
    Funktion: Dieses Script ist die Grundlage aller Räume.*/

using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform playerSpawn;
    public Enemy[] possibleEnemies;

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
    /// <param name="b"></param>
    public void SetTeleporterActive(bool active)
    {
        teleporter.gameObject.SetActive(active);
    }
}
