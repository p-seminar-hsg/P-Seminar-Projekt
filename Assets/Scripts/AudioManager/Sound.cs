﻿using UnityEngine;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 01.04.2019 <br/>
/// Die Klasse Sound dient dazu Sounds zu erstellen, die durch den AudioManager abgespielt werden können.
/// </summary>

//Benötigt, um im Unity-Inspector als Liste angezeigt zu werden
[System.Serializable]
public class Sound
{

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
