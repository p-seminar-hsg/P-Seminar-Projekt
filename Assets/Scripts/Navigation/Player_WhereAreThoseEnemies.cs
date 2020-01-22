using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Florian Müller-Martin <br/>
/// Mitarbeiter: Luca Kellermann (SpriteRenderer zu Image geändert, Farbfehler behoben) <br/>
/// Zuletzt geändert am: 21.01.2020 <br/>
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
            if (MapManager.instance.currentRoomScript.enemies != null && MapManager.instance.currentRoomScript.GetEnemiesAlive() > 0)
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
                RefreshAreas(new Color(0.67f, 0.09f, 0.09f, 0));

            }

            //Wenn der Teleporter aktiv ist, werden die Balken hellblau statt rot gefärbt 
            if (GameObject.Find("Teleporter") != null)
            {
                //Vektor doppelt, um einen höheren Alphawert als den Minimumswert für den Teleporter zu erhalten
                directions = new Vector2[2];
                directions[0] = directions[1] = (GameObject.Find("Teleporter").transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized;
                RefreshAreas(new Color(0.12f, 0.84f, 1, 0));
            }
            cooldown = true;
            StartCoroutine(ResetCooldown());
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
    private void RefreshAreas(Color color)
    {
        foreach (GameObject area in areas)
        {
            StartCoroutine(FadeAlpha(area, color, (alphaChange * EvaluateVectors(area))));
        }
    }

    #region Einstellen der Balken
    /// <summary>
    /// Gibt die Anzahl der Gegner zurück, die in Richtung des gegebenen Balken liegen.
    /// </summary>
    /// <param name="area">Balken, dessen Richtung geprüft werden soll.</param>
    private int EvaluateVectors(GameObject area)
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
    private IEnumerator FadeAlpha(GameObject area, Color color, float newAlpha)
    {
        float currentAlpha = area.GetComponent<Image>().color.a;

        if (currentAlpha < newAlpha)
        {
            for (float alpha = currentAlpha; alpha < newAlpha; alpha += 0.05f)
            {
                color.a = alpha;
                area.GetComponent<Image>().color = color;
                yield return new WaitForSeconds(fadeSpeed);
            }
        }
        else if (currentAlpha > newAlpha)
        {
            for (float alpha = currentAlpha; alpha > newAlpha; alpha -= 0.05f)
            {
                color.a = alpha;
                area.GetComponent<Image>().color = color;
                yield return new WaitForSeconds(fadeSpeed);
            }
            //sichergehen, dass bei areas, die nicht angezeigt werden sollen, alpha = 0 ist (nach dem Faden)
            if (newAlpha == 0)
            {
                color.a = 0;
                area.GetComponent<Image>().color = color;
            }
        }
    }

    /// <summary>
    /// Setzt den Cooldown nach der definierten Zeít zurück.
    /// </summary>
    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldownLength);
        cooldown = false;
    }
    #endregion
}
