using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 18.12.2019 <br/>
/// Dieses Script regelt die Drehung des NavigationArrows.
/// </summary>
public class NavigationArrowOrientation : MonoBehaviour
{
    //Der benötigte Abstandsunterschied zwischen Gegnern, damit der Pfeil einen neuen, näheren Gegner anvisiert.
    //Dadurch wird "Zucken" des Pfeils verhindert.
    public float requiredDistanceDifference;

    //Die Farbe des Pfeils, wenn er auf einen Gegner zeigt.
    public Color colorEnemy;

    //Die Farbe des Pfeils, wenn er auf den Teleporter zeigt.
    public Color colorTeleporter;

    private GameObject player;
    private GameObject targetedEnemy;
    private Image arrow;

    //hier wird der geringste Abstand eines Gegners gespeichert
    private float minDistance;

    //der Gegner mit dem geringsten Abstand
    private GameObject nearestEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //Player und Pfeil finden
        player = GameObject.FindGameObjectWithTag("Player");
        arrow = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        //Gibt es Gegner?
        if (MapManager.instance.currentRoomScript.GetEnemiesAlive() > 0)
        {

            //zur Vermeidung von Exceptions
            if (MapManager.instance.currentRoomScript.enemies.Length > 0)
            {
                //Farbe des Pfeils anpassen
                arrow.color = colorEnemy;

                minDistance = -1;
                nearestEnemy = null;

                SetNearestEnemy();

                //                              Ist die neue kürzeste Distanz auch mit requiredDistanceDifference kleiner als der Abstand zum bisher anvisierten Gegner?
                if ((targetedEnemy == null) || (minDistance + requiredDistanceDifference) < (targetedEnemy.transform.position - player.transform.position).magnitude)
                {
                    targetedEnemy = nearestEnemy;
                }

                TurnArrowToTargetedEnemy();

            }

        }
        //Wenn nicht soll zum Teleporter gezeigt werden.
        else
        {
            //Farbe des Pfeils anpassen
            arrow.color = colorTeleporter;

            TurnArrowToTeleporter();
        }
    }

    /// <summary>
    /// Ermittelt minDistance und nearestEnemy;
    /// </summary>
    private void SetNearestEnemy()
    {
        //bei allen Gegnern den Abstand überprüfen
        foreach (GameObject enemy in MapManager.instance.currentRoomScript.enemies)
        {

            //zur Vermeidung von Exceptions
            if (enemy != null)
            {
                //der Vektor vom Player zum aktuellen Gegner
                Vector3 playerToEnemy = enemy.transform.position - player.transform.position;

                float distance = playerToEnemy.magnitude;

                //falls es der erste Gegner ist: minDistance = -1
                if ((minDistance == -1) || (distance < minDistance))
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
    }

    /// <summary>
    /// Dreht den Pfeil in Richtung des anvisierten Gegners.
    /// </summary>
    private void TurnArrowToTargetedEnemy()
    {
        //der Vektor vom Player zum nähesten Gegner
        Vector3 playerToNearestEnemy = targetedEnemy.transform.position - player.transform.position;

        //             gibt Winkel des Vektors im Bogenmaß       =>       Umwandlung in Gradmaß ;   - 90 (Grad) für den richtigen Winkel
        float angle = (Mathf.Atan2(playerToNearestEnemy.y, playerToNearestEnemy.x) * Mathf.Rad2Deg) - 90;

        //dreht den Pfeil mit dem oben berechneten Winkel um die z-Achse
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Dreht den Pfeil in Richtung des Teleporters.
    /// </summary>
    private void TurnArrowToTeleporter()
    {
        //der Vektor vom Player zum Teleporter
        Vector3 playerToTeleporter = MapManager.instance.currentRoomScript.GetTeleporterPosition() - player.transform.position;

        //             gibt Winkel des Vektors im Bogenmaß     =>     Umwandlung in Gradmaß ;   - 90 (Grad) für den richtigen Winkel
        float angle = (Mathf.Atan2(playerToTeleporter.y, playerToTeleporter.x) * Mathf.Rad2Deg) - 90;

        //dreht den Pfeil mit dem oben berechneten Winkel um die z-Achse
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
