
using UnityEngine;
using UnityEngine.UI;

/*
 Ersteller: Rene Jokiel
 Zuletzt geändert am: 30.6.2019
 Funktion: Diese Datei aktiviert das Inventar/ Pausemenü und deaktiviert die PlayerUI.
                         
*/

public class InventoryManager : MonoBehaviour
{
    public GameObject ui;   //Zu aktivierende ui
    public GameObject ui2;    // Zu deaktivierende ui. Nur notwendig, wenn die UIs in
                              // separaten Canvas(es?) sind
    public Text highscoreNumber;
    public bool gameOver = false;

    public void Toggle()    // Aktiviert ui1 und deaktiviert ui2
    {
        //den Highscore im Pausemenü aktualisieren
        highscoreNumber.text = GameManager.GetScore().ToString();

        if (gameOver == false)
        {
            ui.SetActive(!ui.activeSelf);
            ui2.SetActive(!ui2.activeSelf);

            if (ui.activeSelf)
            {
                Time.timeScale = 0f;    //Zeit wird gestoppt
            }
            else
            {
                Time.timeScale = 1;     //Zeit wird normalisiert
            }
        }
    }
}
    
