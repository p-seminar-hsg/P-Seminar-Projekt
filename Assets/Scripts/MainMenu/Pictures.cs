using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Joshua Brandes und Rene Jokiel <br/>
/// Zuletzt geändert am: 25.09.2019 <br/>
/// Bilder für das Tutorial.
/// </summary>
public class Pictures : MonoBehaviour
{
    public Sprite[] pictures;   // Alle Bilder, die angezeigt werden können
    private Sprite pic; // Aktuelles Bild
    private int count; //Zähler

    private void Start()
    {       
        pic = pictures[0];
        this.GetComponent<Image>().sprite = pic;    // Startbild wird gesetzt
        count = 0;
    }

    public void skip()
    {
        count++;    //Zähler wird erhöht
        if (count >= pictures.Length)   //Bilderreihenfolge wird resetet, wenn alle Bilder einmal durchgelaufen sind
        {
            count = 0;
            pic = pictures[0];
            this.GetComponent<Image>().sprite = pic;
        }
        else
        {
            pic = pictures[count];  
            this.GetComponent<Image>().sprite = pic;    // Das nächste Bild wird gesetzt
        }
    }
}
