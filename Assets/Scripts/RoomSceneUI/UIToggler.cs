using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 7.07.2019 <br/>
/// Dieses Script aktiviert eine Ui und deaktiviert eine andere.
/// </summary>
public class UIToggler : MonoBehaviour
{
    public GameObject ui;   //Zu aktivierende ui
    public GameObject ui2;    // Zu deaktivierende ui. 

    /// <summary>
    /// Aktiviert ui und deaktiviert ui2 und umgekehrt.
    /// </summary>
    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        ui2.SetActive(!ui2.activeSelf);
    }
}
