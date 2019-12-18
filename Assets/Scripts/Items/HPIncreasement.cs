using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 12.08.2019 <br/>
/// Dieses Script erhöht die maximalen Lebenspunkte des Players und ändert die Farbe der Healthbar.
/// </summary>
public class HPIncreasement : MonoBehaviour
{
    public float increasment;
    public Color newHealthBarColor;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Main player = other.GetComponent<Player_Main>();
            player.enhanceMaxHP(increasment, newHealthBarColor);
        }
    }
}
