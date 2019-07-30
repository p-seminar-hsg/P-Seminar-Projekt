using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille (27.07.2019)
/// Richtungen (v.a. für die Blickrichtungsbestimmung des Spielers)
/// </summary>
public enum Direction
{
    UP, LEFT, DOWN, RIGHT
}

/// <summary>
/// Erstellt von Benedikt
/// Diese Klasse stellt nützliche statische Methoden 
/// z.B. zum Testen oder für bestimmte Probleme bereit.
/// Bedient euch! Was am Ende nicht gebraucht wird, nehme ich vor der
/// Veröffentlichung raus. Die Doku sollte alles ausreichend erklären.
/// </summary>
public class Utility
{
    /// <summary>
    /// Gibt ein Array mit allen Components einer bestimmten Art
    /// an den übergebenen GameObjects zurück
    /// </summary>
    /// <typeparam name="T">Der Typ des Components</typeparam>
    /// <param name="gameobjects">Die GameObjects, deren Components gesucht werden</param>
    /// <returns>Ein Array mit allen Components des Typs T an den GameObjects</returns>
    public static T[] GetAllComponents<T>(GameObject[] gameobjects) where T : Component
    {
        if (gameobjects.Length == 0)
            return new T[0];

        List<T> components = new List<T>();
        foreach (GameObject go in gameobjects)
        {
            components.Add(go.GetComponent<T>());
        }
        return components.ToArray();
    }

    /// <summary>
    /// (Re)Mapt einen Float-Wert von einem Bereich/Intervall in ein(en) anderen/-es,
    /// Vgl. http://rosettacode.org/wiki/Map_range
    /// </summary>
    /// <param name="value">Der Wert</param>
    /// <param name="currentMin">Die derzeitige untere Grenze des Wertebereichs</param>
    /// <param name="currentMax">Die derzeitige obere Grenze des Wertebereichs</param>
    /// <param name="newMin">Die neue untere Grenze des Wertebereichs</param>
    /// <param name="newMax">Die neue obere Grenze des Wertebereichs</param>
    /// <returns>Der neue Wert</returns>
    public static float Map(float value, float currentMin, float currentMax, float newMin, float newMax) 
        => newMin + (value - currentMin) * (newMax - newMin) / (currentMax - currentMin);

    /// <summary>
    /// Wählt ein zufälliges Element aus einem Array aus
    /// </summary>
    /// <typeparam name="T">Der Typ des Arrays</typeparam>
    /// <param name="array">Das Array</param>
    /// <returns>Ein zufälliges Element aus dem Array</returns>
    public static T ChooseRandom <T> (T [] array)
    {
        int randomNumber = Random.Range(0, array.Length);
        return array[randomNumber];
    }

    /// <summary>
    /// Gibt ein Array in der Debug-Konsole aus
    /// </summary>
    /// <typeparam name="T">Der Datentyp des Arrays</typeparam>
    /// <param name="array">Das Array</param>
    public static void PrintArray <T> (T [] array) 
    {
        foreach (T element in array)
            Debug.Log(element.ToString());
    }

    /// <summary>
    /// Gibt ein Array als korrekt formatierten String
    /// als Liste aller enthaltenen Elementen zurück
    /// </summary>
    /// <typeparam name="T">Der Datentyp des Arrays</typeparam>
    /// <param name="array">Das Array</param>
    public static string ArrayToString <T> (T [] array)
        => string.Join(",", array);

    /// <summary>
    /// Gibt eine Liste in der Debug-Konsole aus
    /// </summary>
    /// <typeparam name="T">Der Datentyp der Liste</typeparam>
    /// <param name="array">Die Liste</param>
    public static void PrintList <T> (List<T> list)
    {
        Debug.Log(string.Join(",", list));
        //list.ForEach(element => Debug.Log(element.ToString()));
    }

    /// <summary>
    /// Gibt eine Liste als korrekt formatierten String
    /// als Liste aller enthaltenen Elementen zurück
    /// </summary>
    /// <typeparam name="T">Der Datentyp der Liste</typeparam>
    /// <param name="array">DIe Liste</param>
    public static string ListToString <T> (List<T> list)
        => string.Join(",", list);
}
