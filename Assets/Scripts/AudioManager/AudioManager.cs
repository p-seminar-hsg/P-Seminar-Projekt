
/*Ersteller: Luca Kellermann
    Zuletzt geändert am: 1.04.2019
    Funktion: Der AudioManager ist die Klasse, die für das Abspielen aller Sounds verantwortlich ist.
                Um Sounds verwenden zu können, sollte das AudioManager-Prefab zu jeder Scene, die
                Sounds abspielen soll, hinzugefügt werden.

                Jeder einzelne Sound muss in der Liste der Sounds im AudioManager-Prefab
                hinzugefügt werden.
                Dafür einfach "Size" um 1 erhöhen, dem untersten Element einen neuen Namen geben,
                den entsprechenden Audio Clip reinziehen, Volume auf 1 stellen und ggf. bei
                Loop (d.h. der Sound wird in einer Schleife gespielt) und Is Music einen Haken setzen.
                
                Um einen Sound in einem anderen Script abzuspielen, einfach
                FindObjectOfType<AudioManager>().Play("SoundName");  aufrufen.*/

using UnityEngine;

public class AudioManager : MonoBehaviour{

    //Es gibt genau eine Instanz des AudioManagers (Singleton pattern)
    public static AudioManager instance;

    //speichert alle Sounds des AudioManagers
    public Sound[] sounds;

    //Wird beim erzeugen eines neuen AudioManager aufgerufen
    void Awake(){

        if(instance == null){
            //Wenn es noch keinen AudioManager gibt, den gerade erzeugten als die einzige Instanz festlegen
            instance = this;
        } else{
            //Sonst den gerade erzeugten AudioManager direkt wieder löschen und die Methode Awake beenden,
            //damit keine weiteren unerwünschten Methoden aufgerufen werden
            Destroy(gameObject);
            return;
        }

        //Die Instanz des AudioManager nicht löschen, wenn eine neue Scene geladen wird
        DontDestroyOnLoad(gameObject);

        //Geht jeden Sound, der im Unity-Inspector zum AudioManager hinzugefügt wurde durch und
        foreach(Sound s in sounds){

            //fügt
            //1. eine neue AudioSource-Component zum AudioManager hinzu, mit der dann
            //   letztendlich der Sound abgespielt werden kann
            s.source = gameObject.AddComponent<AudioSource>();

            //und übergibt
            //2. der neu hinzugefügten Component die Werte, die im Unity-Inspector eingestellt
            //   wurden
            s.source.clip = s.clip;
            s.source.volume  = s.volume;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    //Spielt einen Sound ab
   public void Play(string name){

       foreach(Sound s in sounds){
           if(s.name == name){

               //Die AudioSource-Component des gefundenen Sounds abspielen und die Methode beenden
               s.source.Play();
               return;
           }
       }

        //Wenn der Sound nicht gefunden wurde, soll eine Warnung ausgegeben werden
       Debug.LogWarning("Kein Sound mit folgendem Namen gefunden: " + name);
   }

    //Pausiert einen Sound
   public void Pause(string name){

       foreach(Sound s in sounds){
           if(s.name == name){

               //Die AudioSource-Component des gefundenen Sounds pausieren und die Methode beenden
               s.source.Pause();
               return;
           }
       }

       //Wenn der Sound nicht gefunden wurde, soll eine Warnung ausgegeben werden
       Debug.LogWarning("Kein Sound mit folgendem Namen gefunden: " + name);
   }

    //Stoppt einen Sound
   public void Stop(string name){

       foreach(Sound s in sounds){
           if(s.name == name){

               //Die AudioSource-Component des gefundenen Sounds stoppen und die Methode beenden
               s.source.Stop();
               return;
           }
       }

       //Wenn der Sound nicht gefunden wurde, soll eine Warnung ausgegeben werden
       Debug.LogWarning("Kein Sound mit folgendem Namen gefunden: " + name);
   }

    //Methode, um die Lautstärke aller Sounds zu ändern
   public  void ChangeSoundVolume(float f){

       foreach(Sound s in sounds){
           
           if(!s.isMusic){
               s.source.volume = f;
           }
       }
   }

    //Methode, um die Lautstärke der Musik zu ändern
   public  void ChangeMusicVolume(float f){

       foreach(Sound s in sounds){
           
           if(s.isMusic){
               s.source.volume = f;
           }
       }
   }
}
