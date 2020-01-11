using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 07.07.2019 <br/>
/// Dieses Script dient dazu, den in den PlayerPrefs gespeicherten Highscore im MainMenu anzuzeigen.
/// </summary>
public class HighscoreControl : MonoBehaviour
{

    //Die Text-Component des GameObjects
    Text text;

    void Awake()
    {
        //Text-Component finden
        text = GetComponent<Text>();

        //Highscore-Text formatieren
        string highscore = "" + GameManager.GetHighscore(); //defaultValue: 0    (Überarbeitet von Rene Jokiel)
        //Text des Highscore GameObjects ändern
        text.text = highscore;
    }
}
