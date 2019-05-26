
/*Ersteller: Luca Kellermann
    Zuletzt geändert am: 22.03.2019
    Funktion: Dieses Script dient zum Öffnen einer anderen Scene durch das MainMenu*/

using UnityEngine;

public class LoadScene : MonoBehaviour{

    private SceneFader fader;

    void Start(){
        fader = FindObjectOfType<SceneFader>();
    }

    //Läd die Scene mit dem übergebenen Index
    public void LoadSceneByIndex(int sceneIndex){

        fader.FadeTo(sceneIndex);
        //SceneManager.LoadScene(sceneIndex);
    }
}
