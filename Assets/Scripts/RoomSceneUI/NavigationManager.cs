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

    public void ColorSwap(Button button)
    {
        var colors = button.colors;

        // Hier werden die Farben verändert. Je nachdem, welchen Zustand die UI hat
        if (button.colors.normalColor == activated)
        {
            colors.normalColor = deactivated;
            colors.pressedColor = deactivated;
            colors.selectedColor = deactivated;
            colors.highlightedColor = deactivated;
            button.colors = colors;
        }
        else
        {
            colors.normalColor = activated;
            colors.pressedColor = activated;
            colors.selectedColor = activated;
            colors.highlightedColor = activated;
            button.colors = colors;
        }
    }
}
