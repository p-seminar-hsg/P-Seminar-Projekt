
/// <summary>
/// Ersteller: Luca Kellermann;
/// Zuletzt geändert am: 11.07.2019
/// 
/// Script, das in einer static Variablen speichert, ob die App gerade gestartet
//  wurde (wird verwendet um die Farbe des Scene Faders zu beginn zu ändern).
/// </summary>

using UnityEngine;

public class AppStartSchoolLike : MonoBehaviour
{
    /// <summary>
    /// Speichert, ob die App gerade gestartet wurde.
    /// </summar>
    public static bool isAppStart;

    //Es gibt genau eine Instanz dieses Scripts (Singleton pattern)
    private static AppStartSchoolLike instance;

    void Awake()
    {
        if(instance == null){
            //Wenn es noch keine Instanz gibt, die gerade erzeugten als die einzige Instanz festlegen
            instance = this;
        } else{
            //Sonst die gerade erzeugte Instanz direkt wieder löschen und die Methode Awake beenden,
            //damit keine weiteren unerwünschten Methoden aufgerufen werden
            Destroy(gameObject);
            return;
        }

        isAppStart = true;
        DontDestroyOnLoad(gameObject);
    }
}
