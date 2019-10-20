using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Es gibt genau eine Instanz des GameManager (Singleton pattern)
    /// <summary>
    /// Die einzige Instanz des GameManager.
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// Key zur Speicherung des Highscores.
    /// </summary>
    public static readonly string KEY_HIGHSCORE = "HighscoreSaveKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";

    /// <summary>
    /// Key-Endung der Keys zur Speicherung der Lautstärkeeinstellungen.
    /// </summary>
    public static readonly string KEY_VOLUME = "VolumeSaveKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";

    /// <summary>
    /// Speichert, ob der Player GameOver ist.
    /// </summary>
    public static bool gameOver;


    [Header("Zum testen: Der Raum mit diesem Index wird dauerhaft verwendet (-1 = normal)")]
    public int testRoomIndex;

    [Header("Punkte pro abgeschlossenem Raum")]
    public int scorePerRoom = 100;


    /// <summary>
    /// Speichert den aktuellen Score.
    /// </summary>
    private int currentScore;


    void Awake()
    {
        if (instance == null)
        {
            //Wenn es noch keinen GameManager gibt, den gerade erzeugten als die einzige Instanz festlegen
            instance = this;
            currentScore = 0;
            gameOver = false;
        }
        else
        {
            //Sonst den gerade erzeugten GameManager direkt wieder löschen
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()      // Von Rene Jokiel (02.08.2019): Zum Testen
    {
        if (Input.GetKey("a"))
        {
            AddToScore(10);
        }
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
    /// Erhöht den Score um die übergebenen Zahl.
    /// </summary>
    /// <param name="number">Zahl, die zum Score dazugezählt werden soll.</param>
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
