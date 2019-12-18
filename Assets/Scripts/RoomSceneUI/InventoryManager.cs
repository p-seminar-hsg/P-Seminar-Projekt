using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 30.06.2019 <br/>
/// Dieses Script aktiviert das Inventar/ Pausemenü und deaktiviert die PlayerUI.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public GameObject ui;   //Zu aktivierende ui
    public GameObject ui2;    // Zu deaktivierende ui. Nur notwendig, wenn die UIs in
                              // separaten Canvas(es?) sind
    public Text scoreNumber;
    public Joystick joystick;
    public bool gameOver = false;

    /// <summary>
    /// Aktiviert ui und deaktiviert ui2 und umgekehrt.
    /// </summary>
    public void Toggle()
    {
        //den Score im Pausemenü aktualisieren
        scoreNumber.text = GameManager.GetScore().ToString();

        //nur beim aktivieren von ui (PauseMenu), sonst wird eine Exception geworfen
        if (!ui.activeSelf)
        {
            //Den Joystick loslassen => Player hört auf zu laufen
            joystick.SendMessage("OnPointerUp", new PointerEventData(EventSystem.current));
        }

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

