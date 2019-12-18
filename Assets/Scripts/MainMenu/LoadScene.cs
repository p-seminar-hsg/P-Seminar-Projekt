using UnityEngine;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 22.03.2019 <br/>
/// Dieses Script dient zum Öffnen einer anderen Scene durch das MainMenu.
/// </summary>
public class LoadScene : MonoBehaviour
{
    //SceneFader, der beim Laden der Scene verwendet werden soll
    private SceneFader fader;

    void Start()
    {
        fader = FindObjectOfType<SceneFader>();
    }

    /// <summary>
    /// Lädt eine Scene. Verwendet dabei den SceneFader.
    /// </summary>
    /// <param name="sceneIndex">Index der Scene, die geladen werden soll.</param>
    public void LoadSceneByIndex(int sceneIndex)
    {
        fader.FadeTo(sceneIndex);
    }
}
