using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 01.06.2019 <br/>
/// Dieses Script dient dazu, die Einstellungen der Lautstärke-Slider im MainMenu auf dem
/// Gerät zu speichern, sodass sie auch nach dem Schließen der App noch vorhanden sind. <br/>
/// Außerdem werden durch dieses Script die Methoden zum Ändern der Lautstärken aufgerufen.
/// </summary>
public class SliderControl : MonoBehaviour
{

    //Die Slider-Component des GameObjects
    Slider slider;
    //Key für Speicherung des Slider-Werts
    private string key;

    //Bei Erstellung des Sliders
    void Awake()
    {
        //Slider-Component finden
        slider = GetComponent<Slider>();

        //key setzen
        key = gameObject.name + GameManager.KEY_VOLUME;

        //Den Wert des Sliders auf den gespeicherten Wert in den PlayerPrefs setzen
        //Wenn noch nicht vorhanden, auf defaultValue 1 setzen
        slider.value = PlayerPrefs.GetFloat(key, 1f);
    }

    //Wird durch OnValueChanged des Sliders augfgerufen
    /// <summary>
    /// Ändert die Lautstärke auf einen neune Wert und speichert diesen in den PlayerPrefs ab.
    /// </summary>
    /// <param name="f">Die neue Lautstärke (sollte zwischen 0 und 1 sein).</param>
    public void ChangeVolume(float f)
    {

        //Entweder Musik- oder Sound-Lautstärke ändern und in PlayerPrefs speichern
        if (gameObject.name == "MusicVolumeSlider")
        {
            AudioManager.instance.ChangeMusicVolume(f);
            PlayerPrefs.SetFloat(key, f);
        }
        else
        {
            AudioManager.instance.ChangeSoundVolume(f);
            PlayerPrefs.SetFloat(key, f);
        }
    }
}
