
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Key zur Speicherung des Highscores
    /// </summary>
    public static readonly string KEY_HIGHSCORE = "HighscoreSaveKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";
    
    /// <summary>
    /// Key-Endung der Keys zur Speicherung der Lautstärkeeinstellungen
    /// </summary>
    public static readonly string KEY_VOLUME = "VolumeSaveKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";

    /// <summary>
    /// Speichert den Raum, der getestet werden soll.
    /// </summary>
    public int testRoomWithIndex;

    /// <summary>
    /// Speichert den Raum, der getestet werden soll.
    /// </summary>
    public static int testRoomIndex;

    void Awake()
    {
        testRoomIndex = testRoomWithIndex;
    }

    // Update is called once per frame
    void Update()
    {
        testRoomIndex = testRoomWithIndex;
    }

    //Methoden von Luca Kellermann:

    /// <summary>
    /// Speichert den übergebenen Highscore ab.
    /// </summary>
    /// <param name="highscore">Der neue Highscore</param>
    public static void SetHighscore(int highscore)
    {
        PlayerPrefs.SetInt(KEY_HIGHSCORE, highscore);
    }

    /// <summary>
    /// Gibt den momentanen Highscore zurück.
    /// </summary>
    /// <returns>Den Highscore bzw. 0 als Defaultwert</returns>
    public static int GetHighscore()
    {
        return PlayerPrefs.GetInt(KEY_HIGHSCORE, 0); //defaultValue: 0
    }

    /// <summary>
    /// Spielt einen Sound ab.
    /// </summary>
    /// <param name="name">Der Name des Sounds</param>
    public static void PlaySound(string name)
    {
        AudioManager.instance.Play(name);
    }

    /// <summary>
    /// Pausiert einen Sound.
    /// </summary>
    /// <param name="name">Der Name des Sounds</param>
    public static void PauseSound(string name)
    {
        AudioManager.instance.Pause(name);
    }

    /// <summary>
    /// Stoppt einen Sound.
    /// </summary>
    /// <param name="name">Der Name des Sounds</param>
    public static void StopSound(string name)
    {
        AudioManager.instance.Stop(name);
    }
}
