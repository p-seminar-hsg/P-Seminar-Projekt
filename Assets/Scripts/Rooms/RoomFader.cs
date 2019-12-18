using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Ersteller: Luca Kellermann (Vorlage von Rene Jokiel) <br/>
/// Zuletzt geändert am: 8.12.2019 <br/>
/// Dieses Script sorgt für einen Fade Effekt während des Ladens neuer Räume.
/// </summary>
public class RoomFader : MonoBehaviour
{
    //Referenz auf das GameObject mit Image-Component,
    //welches zum Faden verwendet wird
    public GameObject imgObject;
    //der Verlauf des Fade Effekts
    public AnimationCurve curve;

    /// <summary>
    /// Speichert den Highscore und startet den Fade Effekt in einen neuen Raum hinein.
    /// </summary>
    public void FadeToRoom()
    {
        if (GameManager.GetScore() > GameManager.GetHighscore())
        {
            GameManager.SetHighscore(GameManager.GetScore());
        }
        StartCoroutine(FadeTo());
    }

    /// <summary>
    /// Startet den Fade Effekt aus einem Raum heraus.
    /// </summary>
    public void FadeFromRoom()
    {
        StartCoroutine(FadeFrom());
    }

    /// <summary>
    /// Lädt eine Scene mit Fade Effekt.
    /// </summary>
    /// <param name="sceneIndex">Index der Scene, die geladen werden soll.</param>
    public void FadeToScene(int sceneIndex)
    {
        if (GameManager.GetScore() > GameManager.GetHighscore())
        {
            GameManager.SetHighscore(GameManager.GetScore());
        }
        StartCoroutine(FadeOut(sceneIndex));
    }

    /// <summary>
    /// Lässt das FadePanel mit Fade Effekt verschwinden.
    /// </summary>
    IEnumerator FadeFrom()
    {

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 0f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(2 * t);
            imgObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        //deaktivieren, damit es keine Interaktionen mit dem GUI einschränkt/verhindert
        imgObject.SetActive(false);
    }

    /// <summary>
    /// Lässt das FadePanel mit Fade Effekt erscheinen.
    /// </summary>
    IEnumerator FadeTo()
    {

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 0.5f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(2 * t);
            imgObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        //deaktivieren, damit es keine Interaktionen mit dem GUI einschränkt/verhindert
        imgObject.SetActive(false);
    }

    /// <summary>
    /// Lässt das FadePanel mit Fade Effekt erscheinen und lädt anschließend eine Scene.
    /// </summary>
    /// <param name="sceneIndex">Index der Scene, die geladen werden soll.</param>
    IEnumerator FadeOut(int sceneIndex)
    {

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 0f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while (t < 1)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            imgObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        GameManager.StopSound("Steps1");
        GameManager.StopSound("Steps2");
        GameManager.StopSound("Steps3");

        //nach dem Fade Effekt die neue Scene laden
        SceneManager.LoadScene(sceneIndex);
    }
}
