
/*Ersteller: Luca Kellermann
    Zuletzt geändert am: 1.04.2019
    Funktion: Die Klasse Sound dient dazu, Sounds zu erstellen, die durch den AudioManager
                abgespielt werden können*/

using UnityEngine;

//Benötigt, um im Unity-Inspector als Liste angezeigt zu werden
[System.Serializable]
public class Sound{
    
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    public bool loop;

    //Benötigt, um die Lautstärken von Sounds und Musik unabhängig einzustellen
    public bool isMusic;

    //Soll nicht im Unity-Inspector sichtbar sein, da die Component AudioSorce
    //im AudioManager konfiguriert wird
    [HideInInspector]
    //Component, die zum AudioManager hinzugefügt wird
    public AudioSource source;

}
