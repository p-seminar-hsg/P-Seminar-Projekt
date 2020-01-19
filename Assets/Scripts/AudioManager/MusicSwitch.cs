using UnityEngine;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 19.01.2020 <br/>
/// Script mit Methode um die Musik zu ändern.
/// </summary>
public class MusicSwitch : MonoBehaviour
{
    /// <summary>
    /// Wechselt die Musik vom MainTheme zu einem anderen Song.
    /// </summary>
    /// <param name="switchTo">Der Name des Songs, zu dem gewechselt werden soll.</param>
    public void SwitchMusicFromMainTheme(string switchTo)
    {
        AudioManager.instance.Stop("MainTheme");
        AudioManager.instance.Play(switchTo);
    }

    /// <summary>
    /// Wechselt die Musik von einem anderen Song zum MainTheme.
    /// </summary>
    /// <param name="switchFrom">Der Name des Songs, von dem aus zum MainTheme gewechselt werden soll.</param>
    public void SwitchMusicToMainTheme(string switchFrom)
    {
        AudioManager.instance.Stop(switchFrom);
        AudioManager.instance.Play("MainTheme");
    }
}
