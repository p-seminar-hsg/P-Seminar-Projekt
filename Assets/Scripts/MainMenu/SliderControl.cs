
/*Ersteller: Luca Kellermann
    Zuletzt geändert am: 1.04.2019
    Funktion: Dieses Script dient dazu, die Einstellungen der Lautstärke-Slider im MainMenu
                auf dem Gerät zu speichern, sodass sie auch nach dem Schließen der App noch
                vorhanden sind.
                Außerdem werden durch dieses Script die Methoden zum Ändern der Lautstärken
                aufgerufen.*/

using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour{

    //Die Slider-Component des GameObjects
    public Slider slider;

    //Bei Erstellung des Sliders
    void Awake(){
        //Slider-Component finden
        slider = GetComponent<Slider>();

        //Key für Speicherung des Slider-Werts
        string key = gameObject.name + "Slider1234";

        //Den Wert des Sliders auf den gespeicherten Wert in den PlayerPrefs setzen
        //Wenn noch nicht vorhanden, auf defaultValue 1 setzen
        slider.value = PlayerPrefs.GetFloat(key, 1f);
    }

    //Wird durch OnValueChanged des Sliders augfgerufen
    public void ChangeVolume(float f){

        //Key für Speicherung des Slider-Werts
        string key = gameObject.name + "Slider1234";

        //Entweder Musik- oder Sound-Lautstärke ändern und in PlayerPrefs speichern
        if(gameObject.name == "MusicVolumeSlider"){
            FindObjectOfType<AudioManager>().ChangeMusicVolume(f);
            PlayerPrefs.SetFloat(key, f);
        } else{
            FindObjectOfType<AudioManager>().ChangeSoundVolume(f);
            PlayerPrefs.SetFloat(key, f);
        }
    }
}
