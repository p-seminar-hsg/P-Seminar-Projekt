using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Mitarbeiter: Luca Kellermann <br/>
/// Zuletzt geändert am: 11.07.2019 <br/>
/// Dieses Script kann Scenes laden und sorgt für einen Fade Effekt.
/// </summary>
public class SceneFader : MonoBehaviour
{

    //Referenz auf das GameObject mit Image-Component, welches zum Faden verwendet wird
    public GameObject imgObject;

    //der Verlauf des Fade Effekts
    public AnimationCurve curve;

    //Image Component des imgObjects
    private Image img;

    void Awake()
    {
        img = imgObject.GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Lädt eine Scene mit Fade Effekt.
    /// </summary>
    /// <param name="sceneIndex">Index der Scene, die geladen werden soll.</param>
    public void FadeTo(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
    }

    /// <summary>
    /// Lässt das FadePanel mit Fade Effekt verschwinden.
    /// </summary>
    IEnumerator FadeIn()
    {

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 2f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
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
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        //nach dem Fade Effekt die neue Scene laden
        SceneManager.LoadScene(sceneIndex);
    }
}
