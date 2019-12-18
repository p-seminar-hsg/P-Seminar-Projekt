﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Florian Müller-Martin <br/>
/// Mitarbeiter: Luca Kellermann (SpriteRenderer zu Image geändert, Farbfehler behoben) <br/>
/// Zuletzt geändert am: 22.11.2019 <br/>
/// Klasse zur Steuerung des farbigen Randes, der anzeigt, wo sich noch Gegner befinden.
/// </summary>
public class Player_WhereAreThoseEnemies : MonoBehaviour
{
    #region Variablen
    [Header("Behaviour Settings")]
    public float alphaChange; //Wert um den der Alpha-Wert eines Balkens pro Gegner erhöht wird
    public float fadeSpeed; //Wie schnell die Balken sich anpassen


    public GameObject[] enemies; //Array mit allen Gegner, die aktuell leben, kommt aus dem Room Skript
    public Vector2[] directions; //Array mit allen normalisierten Vektoren zu den Gegnern
    #endregion

    #region Lifecycle-Methoden
    private void FixedUpdate()
    {
        if (MapManager.instance.currentRoomScript.enemies != null)
        {
            //Reset the Fields
            setAllFields(0.67f, 0.09f, 0.09f, 0);

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
            evaluateVectors(new Color(0.67f, 0.09f, 0.09f, 0));

        }

        if (GameObject.Find("Teleporter") != null)
        {
            directions = new Vector2[1];
            directions[0] = (GameObject.Find("Teleporter").transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized;
            evaluateVectors(new Color(0.12f, 0.84f, 1, 0));
        }

    }
    #endregion

    #region Einstellen der Balken
    public void evaluateVectors(Color color)
    {
        //Alle Vektoren durchgehen und beim entsprechenden Balken den Alpha-Wert erhöhen
        foreach (Vector2 direction in directions)
        {
            //Gegner ist Unten
            if (direction.y < -0.5 && direction.x < 0.5 && direction.x > -0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesBot", color));
            }
            //Gegner ist Rechts
            else if (direction.y < 0.5 && direction.y > -0.5 && direction.x > 0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesRight", color));
            }
            //Gegner ist Oben
            else if (direction.y > 0.5 && direction.x > -0.5 && direction.x < 0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesTop", color));
            }
            //Gegner ist Links
            else if (direction.y > -0.5 && direction.y < 0.5 && direction.x < -0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesLeft", color));
            }
            //Gegner ist Links-Oben
            else if (direction.y > 0.5 && direction.x > 0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesRightTop", color));
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesTopRight", color));
            }
            //Gegner ist Rechts-Unten
            else if (direction.y < -0.5 && direction.x > 0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesRightBot", color));
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesBotRight", color));
            }
            //Gegner ist Link-Unten
            else if (direction.y < -0.5 && direction.x < -0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesBotLeft", color));
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesLeftBot", color));
            }
            //Gegner ist Links-Oben
            else if (direction.y > 0.5 && direction.x < -0.5)
            {
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesLeftTop", color));
                StartCoroutine(fadeAlpha("WhereAreThoseEnemiesTopLeft", color));
            }
        }
    }

    /// <summary>
    /// Erhöht den Alpha-Wert des angegebenen Balkens um alphaChange über einen Zeitraum abhängig von fadeSpeed.
    /// </summary>
    /// <param name="area">Der Balken, dessen Alpha erhöht werden soll.</param>
    public IEnumerator fadeAlpha(string area, Color color)
    {
        for (float alpha = 0; alpha < alphaChange; alpha += fadeSpeed)
        {
            color.a = alpha;
            GameObject.Find(area).GetComponent<Image>().color = color;
            yield return null;
        }
    }

    /// <summary>
    /// Setzt den Alpha-Wert und die RGB-Werte für alle Balken.
    /// </summary>
    /// <param name="alpha">Alpha-Wert, der eingestellt werden soll.</param>
    /// <param name="blue">Blau-Wert, der eingestellt werden soll.</param>
    /// <param name="red">Rot-Wert, der eingestellt werden soll.</param>
    /// <param name="green">Grün-Wert, der eingestellt werden soll.</param>
    public void setAllFields(float red, float blue, float green, float alpha)
    {
        Color color;
  
        Image[] children = GetComponentsInChildren<Image>();
        foreach (Image child in children)
        {
            color = child.color;
            color.a = alpha;
            color.r = red;
            color.b = blue;
            color.g = green;
            child.color = color;
        }
    }
    #endregion
}
