using System.Collections;
using System;
using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille <br/>
/// Mitarbeiter: Luca Kellermann (Sounds und Score Management) <br/>
/// Zuletzt geändert am: 11.01.2020 <br/>
/// Dieses Script ist für die Verwaltung des Scores, das Abspielen von Sounds und für andere grundlegende
/// Funktionen des Spiels verantwortlich.
/// </summary>
public class GameManager : MonoBehaviour
{
    //Es gibt genau eine Instanz des GameManager (Singleton pattern)
    public static GameManager instance;

    //Key zur Speicherung des Highscores.
    public static readonly string KEY_HIGHSCORE = "HighscoreSaveKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";

    //Key-Endung der Keys zur Speicherung der Lautstärkeeinstellungen.
    public static readonly string KEY_VOLUME = "VolumeSaveKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";

    //Speichert, ob der Player GameOver ist.
    public static bool gameOver;


    [Header("Always load Room with this Index (normal = -1)")]
    public int testRoomIndex;

    [Header("Points per cleared Room")]
    public int scorePerRoom = 100;

    // Startet bei -1, weil es bereits beim Laden des allerersten Raums erhöht wird
    [HideInInspector]
    public int roomsCleared = -1;

    //Speichert den aktuellen Score.
    private int currentScore;

    //Namen der Zombie Idle Sounds 
    private string[] zombieIdleSounds = { "Zombie1", "Zombie3", "Zombie4" };


    void Awake()
    {
        if (instance == null)
        {
            // Wenn es noch keinen GameManager gibt, den gerade erzeugten als die einzige Instanz festlegen
            instance = this;
            currentScore = 0;
            gameOver = false;
        }
        else
        {
            // Sonst den gerade erzeugten GameManager direkt wieder löschen
            Destroy(gameObject);
            return;
        }
        StartCoroutine(PlayRandomZombieSounds());
    }

    /// <summary>
    /// Von Benedikt Wille. <br/>
    /// OnApplicationQuit wird immer beim Beenden der App
    /// aufgerufen (nicht beim Pausieren).
    /// </summary>
    private void OnApplicationQuit()
    {
        // Der Highscore wird auch gespeichert wenn man das Spiel mittendrin beendet
        if (currentScore > GetHighscore())
            SetHighscore(currentScore);
    }

    /// <summary>
    /// Speichert den übergebenen Highscore ab.
    /// </summary>
    /// <param name="highscore">Der neue Highscore.</param>
    public static void SetHighscore(int highscore)
    {
        PlayerPrefs.SetInt(KEY_HIGHSCORE, highscore);
    }

    /// <summary>
    /// Gibt den momentanen Highscore zurück.
    /// </summary>
    /// <returns>Den Highscore bzw. 0 als Defaultwert.</returns>
    public static int GetHighscore()
    {
        return PlayerPrefs.GetInt(KEY_HIGHSCORE, 0); //defaultValue: 0
    }

    /// <summary>
    /// Erhöht den Score um die übergebenen Zahl.
    /// </summary>
    /// <param name="number">Zahl, die zum Score addiert werden soll.</param>
    public static void AddToScore(int number)
    {
        instance.currentScore += number;
    }

    /// <summary>
    /// Gibt den momentanen Score zurück.
    /// </summary>
    /// <returns>Den Score.</returns>
    public static int GetScore()
    {
        return instance.currentScore;
    }

    /// <summary>
    /// Spielt einen Sound ab.
    /// </summary>
    /// <param name="name">Der Name des Sounds.</param>
    public static void PlaySound(string name)
    {
        try
        {
            AudioManager.instance.Play(name);
        }
        catch (Exception e)
        {
            //wenn AudioManager GO nicht existiert nichts tun (Konsolenspam vermeiden)
            e.ToString();
        }
    }

    /// <summary>
    /// Spielt einen Sound ab.
    /// </summary>
    /// <param name="name">Der Name des Sounds.</param>
    public void PlaySoundNonStatic(string name)
    {
        PlaySound(name);
    }

    /// <summary>
    /// Pausiert einen Sound.
    /// </summary>
    /// <param name="name">Der Name des Sounds.</param>
    public static void PauseSound(string name)
    {
        try
        {
            AudioManager.instance.Pause(name);
        }
        catch (Exception e)
        {
            //wenn AudioManager GO nicht existiert nichts tun (Konsolenspam vermeiden)
            e.ToString();
        }
    }

    /// <summary>
    /// Stoppt einen Sound.
    /// </summary>
    /// <param name="name">Der Name des Sounds.</param>
    public static void StopSound(string name)
    {
        try
        {
            AudioManager.instance.Stop(name);
        }
        catch (Exception e)
        {
            //wenn AudioManager GO nicht existiert nichts tun (Konsolenspam vermeiden)
            e.ToString();
        }
    }

    /// <summary>
    /// Coroutine, die für zufällige Zombie Sounds mit einem Abstand von 7 bis 20 Sekunden sorgt. <br/>
    /// Die Coroutine ist rekursiv und ruft sich am Ende immer wieder selbst auf, sie muss nur einmal gestartet werden.
    /// </summary>
    private IEnumerator PlayRandomZombieSounds()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(7, 20));

        if (!gameOver && MapManager.instance.currentRoom != null && !MapManager.instance.CurrentRoomIsBossRoom() && MapManager.instance.currentRoomScript.GetEnemiesAlive() > 0)
        {
            GameManager.PlaySound(Utility.ChooseRandom<string>(zombieIdleSounds));
        }

        StartCoroutine(PlayRandomZombieSounds());
    }
}
