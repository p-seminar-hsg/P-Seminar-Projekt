
/*Ersteller: Luca Kellermann
    Zuletzt geändert am: 22.03.2019
    Funktion: Dieses Script dient zum Öffnen einer anderen Scene durch das MainMenu*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour{
    
    //Läd die Scene mit dem übergebenen Index
    public void LoadSceneByIndex(int sceneIndex){

        SceneManager.LoadScene(sceneIndex);
    }
}
