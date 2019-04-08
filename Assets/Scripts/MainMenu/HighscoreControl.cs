
/*Ersteller: Luca Kellermann
    Zuletzt geändert am: 5.04.2019
    Funktion: Dieses Script dient dazu, den in den PlayerPrefs gespeicherten
                Highscore im MainMenu anzuzeigen.
                
                Um den Highscore in einem anderen Script zu ändern,
                PlayerPrefs.SetInt("HighscoreSave", <neuerHighscore>);  aufrufen.*/

using UnityEngine;
using UnityEngine.UI;

public class HighscoreControl : MonoBehaviour{

    //Die Text-Component des GameObjects
    Text text;

    void Awake(){
        //Text-Component finden
        text = GetComponent<Text>();

        //Highscore-Text formatieren
        string highscore = "Highscore: " + PlayerPrefs.GetInt("HighscoreSave", 0); //defaultValue: 0
        //Text des Highscore GameObjects ändern
        text.text = highscore;
    }
}
