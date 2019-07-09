
/*Ersteller: Luca Kellermann (Vorlage von Rene Jokiel)
    Zuletzt geändert am: 26.05.2019
    Funktion: Dieses Script sorgt für einen Fade Effekt während des Ladens neuer Räume.*/


using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomFader : MonoBehaviour
{
    //Referenz auf das GameObject mit Image-Component,
    //welches zum Faden verwendet wird
    public GameObject imgObject;
    //der Verlauf des Fade Effekts
    public AnimationCurve curve;

    public void FadeToRoom(){
        StartCoroutine(FadeTo());
    }

    public void FadeFromRoom(){
        StartCoroutine(FadeFrom());
    }

    public void FadeToScene(int sceneIndex){
        StartCoroutine(FadeOut(sceneIndex));
    }

    IEnumerator FadeFrom(){

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 0f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while(t < 0.5f){
            t += Time.deltaTime;
            float a = curve.Evaluate(2*t);
            imgObject.GetComponent<Image>().color = new Color(0f,0f, 0f, a);
            yield return 0;
        }

        //deaktivieren, damit es keine Interaktionen mit dem GUI einschränkt/verhindert
        imgObject.SetActive(false);
    }

    IEnumerator FadeTo(){

        //aktivieren, damit es sichtbar wird
        imgObject.SetActive(true);

        float t = 0.5f;
        //Alpha-Wert der Image-Component entsprechend dem Kurvenverlauf verändern
        while(t > 0){
            t -= Time.deltaTime;
            float a = curve.Evaluate(2*t);
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
