using UnityEngine;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 17.07.2019 <br/>
/// Script, das in einer static Variablen speichert, ob die App gerade gestartet
/// wurde (wird z.B. verwendet, um das MainTheme zu starten).
/// </summary>
public class AppStartSchoolLike : MonoBehaviour
{
    // Speichert, ob die App gerade gestartet wurde.
    public static bool isAppStart;

    //Es gibt genau eine Instanz dieses Scripts (Singleton pattern)
    private static AppStartSchoolLike instance;

    void Awake()
    {
        if (instance == null)
        {
            //Wenn es noch keine Instanz gibt, die gerade erzeugten als die einzige Instanz festlegen
            instance = this;
        }
        else
        {
            //Sonst die gerade erzeugte Instanz direkt wieder löschen
            Destroy(gameObject);
            return;
        }

        isAppStart = true;

        //Die Instanz nicht löschen, wenn eine neue Scene geladen wird
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Lautstärken beim Appstart an die gespeicherten Werte anpassen
        AudioManager.instance.ChangeMusicVolume(PlayerPrefs.GetFloat("MusicVolumeSlider" + GameManager.KEY_VOLUME, 1f));
        AudioManager.instance.ChangeSoundVolume(PlayerPrefs.GetFloat("SoundsVolumeSlider" + GameManager.KEY_VOLUME, 1f));
        GameManager.PlaySound("MainTheme");
    }
}
