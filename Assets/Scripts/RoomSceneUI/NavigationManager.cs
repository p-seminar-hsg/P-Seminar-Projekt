/*
 Ersteller: Rene Jokiel
 Zuletzt geändert am: 1.8.2019
 Funktion: Dieses Script regelt die Navigationsselection               
*/

using UnityEngine;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    public Color deactivated;   //Farben der Buttons je nach Zustand
    public Color activated;


    public void Toggle(GameObject ui)    // Aktiviert ein UI element. Kann es auch deaktivieren
    {
        ui.SetActive(!ui.activeSelf);   //UI wird verändert
    }

    public void colorSwap(Button butt)
    {
        // Hier werden die Farben verändert. Je nachdem, welchen Zustand die UI hat
        if (butt.GetComponent<Image>().color == activated)
        {
            butt.GetComponent<Image>().color = new Color(deactivated.r, deactivated.g, deactivated.b, deactivated.a);
        }
        else
        {
            butt.GetComponent<Image>().color = new Color(activated.r, activated.g, activated.b, activated.a);
        }
    }
}
