using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{

    public GameObject imgObject;
    public AnimationCurve curve;

    private void Start(){
        StartCoroutine(FadeIn());
    }

    public void FadeTo(int sceneIndex){
        StartCoroutine(FadeOut(sceneIndex));
    }

    IEnumerator FadeIn(){

        imgObject.SetActive(true);

        float t = 1f;
        while(t > 0){
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            imgObject.GetComponent<Image>().color = new Color(0f,0f, 0f, a);
            yield return 0;
        }

        imgObject.SetActive(false);
    }

    IEnumerator FadeOut(int sceneIndex){

        imgObject.SetActive(true);

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            imgObject.GetComponent<Image>().color = new Color(0f,0f, 0f, a);
            yield return 0;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
