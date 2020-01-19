using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 12.07.2019 <br/>
/// Script, das für das Erscheinen des GameOver Screens zuständig ist.
/// </summary>
public class GameOver : MonoBehaviour
{
    public GameObject PlayerUI;
    public GameObject PauseMenuButton;
    public GameObject GameOverUI;
    public Text scoreNumber;
    public Joystick joystick;


    public RoomFader sceneFader;

    /// <summary>
    /// Zuständig für den Retry Button. Lädt die aktuelle Spielszene neu.
    /// <summary>
    public void Retry()
    {
        sceneFader.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Toggelt den GameOver Screen.
    /// <summary>
    public void GoGameOver()
    {
        // Den Joystick loslassen => Player hört auf zu laufen
        joystick.SendMessage("OnPointerUp", new PointerEventData(EventSystem.current));
        
        GameManager.gameOver = true;
        PlayerUI.SetActive(!PlayerUI.activeSelf);
        PauseMenuButton.SetActive(false);
        //den Score im GameOver Screen aktualisieren
        scoreNumber.text = GameManager.GetScore().ToString();
        GameOverUI.SetActive(true);
    }

    /// <summary>
    /// Lädt das Hauptmenü.
    /// <summary>
    public void MainMenu()
    {
        sceneFader.FadeToScene(0);
    }
}
