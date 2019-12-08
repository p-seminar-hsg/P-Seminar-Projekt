/*
 Ersteller: Rene Jokiel
 Mitarbeiter: Luca Kellermann (Speicherung der Navigationsselection)
 Zuletzt geändert am: 1.8.2019
 Funktion: Dieses Script regelt die Navigationsselection               
*/

using UnityEngine;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    public Color deactivated;   //Farben der Buttons je nach Zustand
    public Color activated;

    //Navigations-GameObjects
    public GameObject minimap;
    public GameObject arrow;
    public GameObject border;

    //Navigationsselection-Buttons
    public Button buttonMinimap;
    public Button buttonArrow;
    public Button buttonBorder;

    //Keys zur Speicherung der Navigationsselection in den PlayerPrefs
    private static readonly string keyMinimap = "MinimapKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";
    private static readonly string keyArrow = "ArrowKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";
    private static readonly string keyBorder = "BorderKey_EasterEgg_;-)_PSeminarAppHansSachsGymnasium";

    void Start()
    {
        //für alle Navigationsoptionen überprüfen, ob diese aktiv sein sollen (-1 = inaktiv, 1 = aktiv (keine bools in PlayerPrefs speicherbar))
        if (PlayerPrefs.GetInt(keyMinimap, -1) == 1)
        {
            //Wert umkehren, da dieser bei Toggle() wieder umgekehrt wird
            PlayerPrefs.SetInt(keyMinimap, -1);
            //Navigationsoption aktivieren
            Toggle(minimap);
            //Farben des entsprechenden Buttons tauschen
            ColorSwap(buttonMinimap);
        }
        if (PlayerPrefs.GetInt(keyArrow, -1) == 1)
        {
            PlayerPrefs.SetInt(keyArrow, -1);
            Toggle(arrow);
            ColorSwap(buttonArrow);
        }
        if (PlayerPrefs.GetInt(keyBorder, -1) == 1)
        {
            PlayerPrefs.SetInt(keyBorder, -1);
            Toggle(border);
            ColorSwap(buttonBorder);
        }
    }

    public void Toggle(GameObject ui)    // Aktiviert ein UI element. Kann es auch deaktivieren
    {
        ui.SetActive(!ui.activeSelf);   //UI wird verändert

        string key = "defaultKey";

        if (ui.Equals(minimap))
        {
            key = keyMinimap;
        } else if (ui.Equals(arrow))
        {
            key = keyArrow;
        } else if (ui.Equals(border))
        {
            key = keyBorder;
        }

        //Wert in PlayerPrefs umkehren; default: -1 (-1 * -1 = 1 => wird beim ersten Aktivieren das erste Mal gespeichert, muss also 1 sein)
        PlayerPrefs.SetInt(key, (-1 * PlayerPrefs.GetInt(key, -1)));
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
