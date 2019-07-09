using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Ersteller: Rene Jokiel
 Zuletzt geändert am: 7.7.2019
 Funktion: Diese Datei aktiviert eine Ui und deaktiviert eine andere               
*/

public class UIToggler : MonoBehaviour
{
    public GameObject ui;   //Zu aktivierende ui
    public GameObject ui2;    // Zu deaktivierende ui. 

    public void Toggle()    // Aktiviert ui1 und deaktiviert ui2
    {
        ui.SetActive(!ui.activeSelf);
        ui2.SetActive(!ui2.activeSelf);
    }
}
