using UnityEngine;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 17.12.2019 <br/>
/// Script mit Methode um den Button Sound abzuspielen.
/// </summary>
public class ButtonSound : MonoBehaviour
{
    /// <summary>
    /// Spielt den Button Sound ab.
    /// </summary>
    public void PlayButtonSound()
    {
        AudioManager.instance.Play("Button");
    }
}
