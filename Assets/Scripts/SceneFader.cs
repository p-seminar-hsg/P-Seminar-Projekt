
/*Ersteller: Rene Jokiel (abgeändert von Luca Kellermann)
    Zuletzt geändert am: 25.05.2019
    Funktion: Dieses Script kann Scenes laden und sorgt für einen Fade Effekt.*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{

    //Referenz auf das GameObject mit Image-Component,
    //welches zum Faden verwendet wird
    public GameObject imgObject;
    //der Verlauf des Fade Effekts
    public AnimationCurve curve;

    private void Start(){
        StartCoroutine(FadeIn());
    }

    public void FadeTo(int sceneIndex){
        Time.timeScale = 1f;
        StartCoroutine(FadeOut(sceneIndex));
    }

    IEnumerator FadeIn(){

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 1f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while(t > 0){
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            imgObject.GetComponent<Image>().color = new Color(0f,0f, 0f, a);
            yield return 0;
        }

        //deaktivieren, damit es keine Interaktionen mit dem GUI einschränkt/verhindert
        imgObject.SetActive(false);
    }

    IEnumerator FadeOut(int sceneIndex){

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 0f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while (t < 1)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            imgObject.GetComponent<Image>().color = new Color(0f,0f, 0f, a);
            yield return 0;
        }

        //nach dem Fade Effekt die neue Scene laden
        SceneManager.LoadScene(sceneIndex);
    }
}
