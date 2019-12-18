using UnityEngine;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 1.04.2019 <br/>
/// Der AudioManager ist die Klasse, die für das Abspielen aller Sounds verantwortlich ist. <br/>
/// 
/// Jeder einzelne Sound muss in der Liste der Sounds im AudioManager-Prefab hinzugefügt werden. <br/>
/// Dafür einfach "Size" um 1 erhöhen, dem untersten Element einen neuen Namen geben,
/// den entsprechenden Audio Clip reinziehen, Volume auf 1 stellen und ggf. bei
/// Loop (d.h. der Sound wird in einer Schleife gespielt) und Is Music einen Haken setzen. <br/>
/// 
/// Um einen Sound in einem anderen Script abzuspielen, einfach <br/>
/// FindObjectOfType&lt;AudioManager&gt;().Play("SoundName"); <br/>
/// aufrufen.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Es gibt genau eine Instanz des AudioManagers (Singleton pattern)
    public static AudioManager instance;

    // Speichert alle Sounds des AudioManagers
    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            // Wenn es noch keinen AudioManager gibt, den gerade erzeugten als die einzige Instanz festlegen
            instance = this;
        }
        else
        {
            //Sonst den gerade erzeugten AudioManager direkt wieder löschen
            Destroy(gameObject);
            return;
        }

        //Die Instanz des AudioManager nicht löschen, wenn eine neue Scene geladen wird
        DontDestroyOnLoad(gameObject);

        //Geht jeden Sound, der im Unity-Inspector zum AudioManager hinzugefügt wurde durch
        foreach (Sound s in sounds)
        {

            //fügt eine neue AudioSource-Component zum AudioManager hinzu, mit der dann
            //letztendlich der Sound abgespielt werden kann
            s.source = gameObject.AddComponent<AudioSource>();

            //und übergibt der neu hinzugefügten Component die Werte, die im Unity-Inspector eingestellt wurden
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    /// <summary>
    /// Spielt einen Sound ab.
    /// </summary>
    /// <param name="name">Der Name des Sounds, der abgespielt werden soll.</param>
    public void Play(string name)
    {

        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                //Die AudioSource-Component des gefundenen Sounds abspielen und die Methode beenden
                s.source.Play();
                return;
            }
        }

        //Wenn der Sound nicht gefunden wurde, soll eine Warnung ausgegeben werden
        Debug.LogWarning("Kein Sound mit folgendem Namen gefunden: " + name);
    }

    /// <summary>
    /// Pausiert einen Sound.
    /// </summary>
    /// <param name="name">Der Name des Sounds, der pausiert werden soll.</param>
    public void Pause(string name)
    {

        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                //Die AudioSource-Component des gefundenen Sounds pausieren und die Methode beenden
                s.source.Pause();
                return;
            }
        }

        //Wenn der Sound nicht gefunden wurde, soll eine Warnung ausgegeben werden
        Debug.LogWarning("Kein Sound mit folgendem Namen gefunden: " + name);
    }

    /// <summary>
    /// Stoppt einen Sound
    /// </summary>
    /// <param name="name">Der Name des Sounds, der gestoppt werden soll.</param>
    public void Stop(string name)
    {

        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                //Die AudioSource-Component des gefundenen Sounds stoppen und die Methode beenden
                s.source.Stop();
                return;
            }
        }

        //Wenn der Sound nicht gefunden wurde, soll eine Warnung ausgegeben werden
        Debug.LogWarning("Kein Sound mit folgendem Namen gefunden: " + name);
    }

    /// <summary>
    /// Ändert die Lautstärke aller Sounds auf einen neuen Wert.
    /// </summary>
    /// <param name="f">Die neue Lautstärke (sollte zwischen 0 und 1 sein).</param>
    public void ChangeSoundVolume(float f)
    {

        foreach (Sound s in sounds)
        {

            if (!s.isMusic)
            {
                s.source.volume = f;
            }
        }
    }

    /// <summary>
    /// Ändert die Lautstärke der Musik auf einen neuen Wert.
    /// </summary>
    /// <param name="f">Die neue Lautstärke (sollte zwischen 0 und 1 sein).</param>
    public void ChangeMusicVolume(float f)
    {

        foreach (Sound s in sounds)
        {

            if (s.isMusic)
            {
                s.source.volume = f;
            }
        }
    }
}
