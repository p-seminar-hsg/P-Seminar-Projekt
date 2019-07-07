using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz
/// Zuletzt geändert am 01.07.2019
/// Mainklasse für den Player
/// </summary>
public class Player_Main : MonoBehaviour
{
    private float HP; //Aktuelle Lebenspunkte des Players
    public float maxHP; //Maximale Lebenspunkte des Charakters
    public float strength; //Angriffswert des Players
    private GameObject healthBar; //Referenz zur HealthBar

    [Header("Unity Stuff")]
    public Image healthBarPauseMenu;//Referenz zur Healthbar im PauseMenu (Geändert von Rene Jokiel)

    private void Start()
    {
        HP = maxHP;
        healthBar = GameObject.Find("Bar"); //Referenz wird hergestellt
    }

    /// <summary>
    /// Schadensmethode des Players - Reduziert die HP des Players um den übergebenen Wert und stößt den Player in die übergebene Richtung weg
    /// </summary>
    /// <param name="damage">Der Schadenswert, der zugeführt werden soll</param>
    /// <param name="knockbackDirection">Richtung des Knockbacks</param>
    public void takeHit(GameObject enemy)
    {
        if (HP > 0)
        {
            HP -= enemy.GetComponent<Enemy>().strength;
        }
        Debug.Log("Health reduced to: " + HP);

        StartCoroutine(GetComponent<Player_Movement>().KnockbackCo((transform.position - enemy.transform.position), enemy.GetComponent<Enemy>().knockbackStrength));

    }

    /// <summary>
    /// Heilmethode des Players - Erhöht die HP des Players um den übergebenen Wert
    /// </summary>
    /// <param name="heal">Der Heilwert, der zugeführt werden soll</param>
    public void heal(float heal)
    {
        HP += heal;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        Debug.Log("Health set to: " + HP);
    }

    /// <summary>
    /// Stärkemethode des Players - Erhöht den Schadenswert des Players um den übergebenen Wert
    /// </summary>
    /// <param name="strengthening">Der Schadenswert, der zugeführt werden soll</param>
    public void strengthen(float strengthening)
    {
        strength += strengthening;
    }

    /// <summary>
    /// Sterbemethode des Players - Das GameObject wird zerstört
    /// </summary>
    public void die()
    {
        Destroy(this);
    }

    /// <summary>
    /// Angriffsmethode des Players - muss dann von der Waffe aus getriggert werden
    /// </summary>
    /// <param name="enemy">Der Gegner übergibt sich komplett, damit man nich 20000 Einzelparameter hat</param>
    public void attack(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().TakeHit(enemy.transform.position - transform.position, strength);
    }

    // FixedUpdate wird einmal pro Frame aufgerufen
    private void FixedUpdate()
    {
        //Wenn der Player keine Leben mehr hat wird die Methode die() aufgerufen
        if (HP <= 0)
        {
            die();
        }

        //Die Health Bar wird aktualisiert
        healthBar.transform.localScale = new Vector3(HP / maxHP, 1f, 1f);
        healthBarPauseMenu.fillAmount = HP / maxHP;// Von Rene Jokiel

        //Selbstgeißelung auf Leertaste zu Demonstrationszwecken
        if (Input.GetKey(KeyCode.Space))
        {
            HP -= 1;
        }
    }


    //Nur für Testzwecke
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision with Enemy");
        }
    }

    



}
