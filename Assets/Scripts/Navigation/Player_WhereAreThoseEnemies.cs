using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Florian Müller-Martin <br/>
/// Mitarbeiter: Luca Kellermann (SpriteRenderer zu Image geändert, Farbfehler behoben) <br/>
/// Zuletzt geändert am: 09.01.2020 <br/>
/// Klasse zur Steuerung des farbigen Randes, der anzeigt, wo sich noch Gegner befinden.
/// </summary>
public class Player_WhereAreThoseEnemies : MonoBehaviour
{
    #region Variablen
    [Header("Behaviour Settings")]
    public float alphaChange; //Wert um den der Alpha-Wert eines Balkens erhöht wird, wenn ein Gegner in dieser Richtung ist
    public float fadeSpeed; //Wie schnell die Balken sich anpassen
    public float cooldownLength; //Der Rand wird nur in bestimmten Zeitintervallen aktualisiert, um die Performance zu verbessern

    public GameObject[] areas; //Array, dass alle Balken enthält

    private bool cooldown; 
    private GameObject[] enemies; //Array mit allen Gegner, die aktuell leben, kommt aus dem Room Skript
    private Vector2[] directions; //Array mit allen normalisierten Vektoren zu den Gegnern
    

    #endregion

    #region Lifecycle-Methoden

    private void FixedUpdate()
    {
        if (!cooldown)
        {
            if (MapManager.instance.currentRoomScript.enemies != null)
            {
                enemies = MapManager.instance.currentRoomScript.enemies; // Abrufen des Arrays vom Room-Skript
                directions = new Vector2[enemies.Length]; //Das directions Array braucht die gleiche Länge, wie das enemies Array

                //Einmal alle Vektoren zu den Gegnern ausrechnen und normalisieren
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i] != null)
                    {
                        directions[i] = (enemies[i].transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized;
                    }
                }
                refreshAreas(new Color(0.67f, 0.09f, 0.09f, 0));

            }

            //Wenn der Teleporter aktiv ist, werden die Balken hellblau statt rot gefärbt 
            if (GameObject.Find("Teleporter") != null)
            {
                directions = new Vector2[1];
                directions[0] = (GameObject.Find("Teleporter").transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized;
                refreshAreas(new Color(0.12f, 0.84f, 1, 0));
            }
            cooldown = true;
            StartCoroutine("resetCooldown");
        }

    }

    /// <summary>
    /// Setzt den Cooldown beim Deaktivieren zurück, damit der Rand wieder aktiviert werden kann
    /// </summary>
    private void OnDisable()
    {
        cooldown = false;
    }
    #endregion

    /// <summary>
    /// Aktualisiert die Alpha-Werte aller Balken.
    /// </summary>
    /// <param name="color">Farbe, in der die Balken gefärbt sein sollen.</param>
    public void refreshAreas(Color color)
    {
        foreach (GameObject area in areas)
        {
            StartCoroutine(fadeAlpha(area, color, (alphaChange * evaluateVectors(area))));
        }
    }

    #region Einstellen der Balken
    /// <summary>
    /// Gibt die Anzahl der Gegner zurück, die in Richtung des gegebenen Balken liegen.
    /// </summary>
    /// <param name="area">Balken, dessen Richtung geprüft werden soll.</param>
    public int evaluateVectors(GameObject area)
    {
        int count = 0;

        //Alle Vektoren durchgehen und beim entsprechenden Balken den Alpha-Wert erhöhen
        foreach (Vector2 direction in directions)
        {
            
            //Gegner ist Unten
            if (direction.y < -0.5 && direction.x < 0.5 && direction.x > -0.5 && area.name == "WhereAreThoseEnemiesBot")
            {
                count++;
            }
            //Gegner ist Rechts
            else if (direction.y < 0.5 && direction.y > -0.5 && direction.x > 0.5 && area.name == "WhereAreThoseEnemiesRight")
            {
                count++;
            }
            //Gegner ist Oben
            else if (direction.y > 0.5 && direction.x > -0.5 && direction.x < 0.5 && area.name == "WhereAreThoseEnemiesTop")
            {
                count++;
            }
            //Gegner ist Links
            else if (direction.y > -0.5 && direction.y < 0.5 && direction.x < -0.5 && area.name == "WhereAreThoseEnemiesLeft")
            {
                count++;
            }
            //Gegner ist Links-Oben
            else if (direction.y > 0.5 && direction.x > 0.5 && (area.name == "WhereAreThoseEnemiesRightTop" || area.name == "WhereAreThoseEnemiesTopRight"))
            {
                count++;
            }
            //Gegner ist Rechts-Unten
            else if (direction.y < -0.5 && direction.x > 0.5 && (area.name == "WhereAreThoseEnemiesRightBot" || area.name == "WhereAreThoseEnemiesBotRight"))
            {
                count++;
            }
            //Gegner ist Links-Unten
            else if (direction.y < -0.5 && direction.x < -0.5 && (area.name == "WhereAreThoseEnemiesBotLeft" || area.name == "WhereAreThoseEnemiesLeftBot"))
            {
                count++;
            }
            //Gegner ist Links-Oben
            else if (direction.y > 0.5 && direction.x < -0.5 && (area.name == "WhereAreThoseEnemiesLeftTop" || area.name == "WhereAreThoseEnemiesTopLeft"))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Erhöht den Alpha-Wert des angegebenen Balkens um alphaChange über einen Zeitraum abhängig von fadeSpeed.
    /// </summary>
    /// <param name="area">Der Balken, dessen Alpha angepasst werden soll.</param>
    /// <param name="color">Farbe, die der Balken bekommen soll.</param>
    /// <param name="newAlpha">Alpha-Wert, den der Balken bekommen soll.</param>
    public IEnumerator fadeAlpha(GameObject area, Color color, float newAlpha)
    {
        Debug.Log("Faded");
        if (area.GetComponent<Image>().color.a < newAlpha)
        {            
            while (area.GetComponent<Image>().color.a < newAlpha)
            {
                color.a = area.GetComponent<Image>().color.a;
                color.a += fadeSpeed;
                area.GetComponent<Image>().color = color;
                yield return new WaitForEndOfFrame();
            }
        }

        if (area.GetComponent<Image>().color.a > newAlpha)
        {            
            while (area.GetComponent<Image>().color.a > newAlpha)
            {
                color.a = area.GetComponent<Image>().color.a;
                color.a -= fadeSpeed;
                area.GetComponent<Image>().color = color;
                yield return new WaitForEndOfFrame();
            }
        }

    }

    /// <summary>
    /// Setzt den Cooldown nach der definierten Zeít zurück.
    /// </summary>
    public IEnumerator resetCooldown()
    {
        yield return new WaitForSeconds(cooldownLength);
        cooldown = false;
    } 
    #endregion
}
